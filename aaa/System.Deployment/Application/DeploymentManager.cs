using System;
using System.Collections;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Deployment.Application.Manifest;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Runtime.Remoting;
using System.Security.Permissions;
using System.Security.Policy;
using System.Threading;

namespace System.Deployment.Application
{
	// Token: 0x0200003A RID: 58
	internal class DeploymentManager : IDisposable, IDownloadNotification
	{
		// Token: 0x060001CF RID: 463 RVA: 0x0000C326 File Offset: 0x0000B326
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public DeploymentManager(string appId)
			: this(appId, false, true, null, null)
		{
			if (appId == null)
			{
				throw new ArgumentNullException("appId");
			}
		}

		// Token: 0x060001D0 RID: 464 RVA: 0x0000C341 File Offset: 0x0000B341
		[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
		public DeploymentManager(Uri deploymentSource)
			: this(deploymentSource, false, true, null, null)
		{
			if (deploymentSource == null)
			{
				throw new ArgumentNullException("deploymentSource");
			}
			UriHelper.ValidateSupportedSchemeInArgument(deploymentSource, "deploymentSource");
		}

		// Token: 0x060001D1 RID: 465 RVA: 0x0000C370 File Offset: 0x0000B370
		internal DeploymentManager(Uri deploymentSource, bool isUpdate, bool isConfirmed, DownloadOptions downloadOptions, AsyncOperation optionalAsyncOp)
		{
			this._deploySource = deploymentSource;
			this._isupdate = isUpdate;
			this._isConfirmed = isConfirmed;
			this._downloadOptions = downloadOptions;
			this._events = new EventHandlerList();
			this._syncGroupMap = CollectionsUtil.CreateCaseInsensitiveHashtable();
			this._subStore = SubscriptionStore.CurrentUser;
			this.bindWorker = new ThreadStart(this.BindAsyncWorker);
			this.synchronizeWorker = new ThreadStart(this.SynchronizeAsyncWorker);
			this.synchronizeGroupWorker = new WaitCallback(this.SynchronizeGroupAsyncWorker);
			this.bindCompleted = new SendOrPostCallback(this.BindAsyncCompleted);
			this.synchronizeCompleted = new SendOrPostCallback(this.SynchronizeAsyncCompleted);
			this.progressReporter = new SendOrPostCallback(this.ProgressReporter);
			if (optionalAsyncOp == null)
			{
				this.asyncOperation = AsyncOperationManager.CreateOperation(null);
			}
			else
			{
				this.asyncOperation = optionalAsyncOp;
			}
			this._log = Logger.StartLogging();
			if (deploymentSource != null)
			{
				Logger.SetSubscriptionUrl(this._log, deploymentSource);
			}
			this._assertApplicationReqEvents = new ManualResetEvent[3];
			this._assertApplicationReqEvents[0] = this._trustNotGrantedEvent;
			this._assertApplicationReqEvents[1] = this._platformRequirementsFailedEvent;
			this._assertApplicationReqEvents[2] = this._platformRequirementsSucceededEvent;
			this._callerType = DeploymentManager.CallerType.Other;
			PolicyKeys.SkipApplicationDependencyHashCheck();
			PolicyKeys.SkipDeploymentProvider();
			PolicyKeys.SkipSchemaValidation();
			PolicyKeys.SkipSemanticValidation();
			PolicyKeys.SkipSignatureValidation();
		}

		// Token: 0x060001D2 RID: 466 RVA: 0x0000C4F4 File Offset: 0x0000B4F4
		internal DeploymentManager(string appId, bool isUpdate, bool isConfirmed, DownloadOptions downloadOptions, AsyncOperation optionalAsyncOp)
			: this(null, isUpdate, isConfirmed, downloadOptions, optionalAsyncOp)
		{
			this._bindAppId = new DefinitionAppId(appId);
		}

		// Token: 0x17000098 RID: 152
		// (get) Token: 0x060001D3 RID: 467 RVA: 0x0000C50F File Offset: 0x0000B50F
		// (set) Token: 0x060001D4 RID: 468 RVA: 0x0000C517 File Offset: 0x0000B517
		public DeploymentManager.CallerType Callertype
		{
			get
			{
				return this._callerType;
			}
			set
			{
				this._callerType = value;
			}
		}

		// Token: 0x060001D5 RID: 469 RVA: 0x0000C520 File Offset: 0x0000B520
		public void BindAsync()
		{
			if (this._cancellationPending)
			{
				return;
			}
			int num = Interlocked.Exchange(ref this._bindGuard, 1);
			if (num != 0)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_BindOnce"));
			}
			this.bindWorker.BeginInvoke(null, null);
		}

		// Token: 0x060001D6 RID: 470 RVA: 0x0000C564 File Offset: 0x0000B564
		public ActivationContext Bind()
		{
			int num = Interlocked.Exchange(ref this._bindGuard, 1);
			if (num != 0)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_BindOnce"));
			}
			bool flag = false;
			TempFile tempFile = null;
			TempDirectory tempDirectory = null;
			FileStream fileStream = null;
			try
			{
				string text = null;
				this.BindCore(true, ref tempFile, ref tempDirectory, ref fileStream, ref text);
			}
			catch (Exception)
			{
				flag = true;
				throw;
			}
			finally
			{
				this._state = DeploymentProgressState.DownloadingApplicationFiles;
				if (flag)
				{
					if (tempDirectory != null)
					{
						tempDirectory.Dispose();
					}
					if (tempFile != null)
					{
						tempFile.Dispose();
					}
					if (fileStream != null)
					{
						fileStream.Close();
					}
				}
			}
			return this._actCtx;
		}

		// Token: 0x060001D7 RID: 471 RVA: 0x0000C600 File Offset: 0x0000B600
		public void DeterminePlatformRequirements()
		{
			try
			{
				if (this._actDesc == null)
				{
					throw new InvalidOperationException(Resources.GetString("Ex_BindFirst"));
				}
				this.DeterminePlatformRequirementsCore(true);
				this._platformRequirementsSucceededEvent.Set();
			}
			catch (Exception)
			{
				this._platformRequirementsFailedEvent.Set();
				throw;
			}
		}

		// Token: 0x060001D8 RID: 472 RVA: 0x0000C65C File Offset: 0x0000B65C
		public void DetermineTrust(TrustParams trustParams)
		{
			try
			{
				if (this._actDesc == null)
				{
					throw new InvalidOperationException(Resources.GetString("Ex_BindFirst"));
				}
				this.DetermineTrustCore(true, trustParams);
			}
			catch (Exception)
			{
				this._trustNotGrantedEvent.Set();
				throw;
			}
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0000C6AC File Offset: 0x0000B6AC
		public void SynchronizeAsync()
		{
			if (this._cancellationPending)
			{
				return;
			}
			if (this._actDesc == null)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_BindFirst"));
			}
			int num = Interlocked.Exchange(ref this._syncGuard, 1);
			if (num != 0)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_SyncNullOnce"));
			}
			this.synchronizeWorker.BeginInvoke(null, null);
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0000C708 File Offset: 0x0000B708
		public void Synchronize()
		{
			if (this._actDesc == null)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_BindFirst"));
			}
			int num = Interlocked.Exchange(ref this._syncGuard, 1);
			if (num != 0)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_SyncNullOnce"));
			}
			this.SynchronizeCore(true);
		}

		// Token: 0x060001DB RID: 475 RVA: 0x0000C755 File Offset: 0x0000B755
		public void SynchronizeAsync(string groupName)
		{
			this.SynchronizeAsync(groupName, null);
		}

		// Token: 0x060001DC RID: 476 RVA: 0x0000C760 File Offset: 0x0000B760
		public void SynchronizeAsync(string groupName, object userState)
		{
			if (groupName == null)
			{
				this.SynchronizeAsync();
				return;
			}
			if (this._actDesc == null)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_BindFirst"));
			}
			if (!this._cached)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_SyncNullFirst"));
			}
			bool flag;
			SyncGroupHelper syncGroupHelper = this.AttachToGroup(groupName, userState, out flag);
			if (flag)
			{
				ThreadPool.QueueUserWorkItem(this.synchronizeGroupWorker, syncGroupHelper);
				return;
			}
			throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_SyncGroupOnce"), new object[] { groupName }));
		}

		// Token: 0x060001DD RID: 477 RVA: 0x0000C7E8 File Offset: 0x0000B7E8
		public void Synchronize(string groupName)
		{
			if (groupName == null)
			{
				this.Synchronize();
				return;
			}
			if (this._actDesc == null)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_BindFirst"));
			}
			if (!this._cached)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_SyncNullFirst"));
			}
			bool flag;
			SyncGroupHelper syncGroupHelper = this.AttachToGroup(groupName, null, out flag);
			if (flag)
			{
				this.SynchronizeGroupCore(true, syncGroupHelper);
				return;
			}
			throw new InvalidOperationException(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_SyncGroupOnce"), new object[] { groupName }));
		}

		// Token: 0x060001DE RID: 478 RVA: 0x0000C86C File Offset: 0x0000B86C
		public ObjectHandle ExecuteNewDomain()
		{
			if (this._actDesc == null)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_BindFirst"));
			}
			if (!this._cached)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_SyncNullFirst"));
			}
			return Activator.CreateInstance(this._actCtx);
		}

		// Token: 0x060001DF RID: 479 RVA: 0x0000C8AC File Offset: 0x0000B8AC
		public void ExecuteNewProcess()
		{
			if (this._actDesc == null)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_BindFirst"));
			}
			if (!this._cached)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_SyncNullFirst"));
			}
			this._subStore.ActivateApplication(this._actDesc.AppId, null, false);
		}

		// Token: 0x060001E0 RID: 480 RVA: 0x0000C901 File Offset: 0x0000B901
		public void CancelAsync()
		{
			this._cancellationPending = true;
		}

		// Token: 0x17000099 RID: 153
		// (get) Token: 0x060001E1 RID: 481 RVA: 0x0000C90A File Offset: 0x0000B90A
		public bool CancellationPending
		{
			get
			{
				return this._cancellationPending;
			}
		}

		// Token: 0x060001E2 RID: 482 RVA: 0x0000C914 File Offset: 0x0000B914
		public void CancelAsync(string groupName)
		{
			if (groupName == null)
			{
				this.CancelAsync();
				return;
			}
			lock (this._syncGroupMap.SyncRoot)
			{
				SyncGroupHelper syncGroupHelper = (SyncGroupHelper)this._syncGroupMap[groupName];
				if (syncGroupHelper != null)
				{
					syncGroupHelper.CancelAsync();
				}
			}
		}

		// Token: 0x1700009A RID: 154
		// (get) Token: 0x060001E3 RID: 483 RVA: 0x0000C974 File Offset: 0x0000B974
		public string ShortcutAppId
		{
			get
			{
				AssemblyManifest deployManifest = this._actDesc.DeployManifest;
				SubscriptionState subscriptionState = this._subStore.GetSubscriptionState(deployManifest);
				string text = null;
				if (subscriptionState.IsInstalled)
				{
					text = string.Format("{0}#{1}", subscriptionState.DeploymentProviderUri.AbsoluteUri, subscriptionState.SubscriptionId.ToString());
				}
				return text;
			}
		}

		// Token: 0x1700009B RID: 155
		// (get) Token: 0x060001E4 RID: 484 RVA: 0x0000C9C8 File Offset: 0x0000B9C8
		public string LogFilePath
		{
			get
			{
				string text = Logger.GetLogFilePath(this._log);
				if (!Logger.FlushLog(this._log))
				{
					text = null;
				}
				return text;
			}
		}

		// Token: 0x14000007 RID: 7
		// (add) Token: 0x060001E5 RID: 485 RVA: 0x0000C9F1 File Offset: 0x0000B9F1
		// (remove) Token: 0x060001E6 RID: 486 RVA: 0x0000CA04 File Offset: 0x0000BA04
		public event BindCompletedEventHandler BindCompleted
		{
			add
			{
				this.Events.AddHandler(DeploymentManager.bindCompletedKey, value);
			}
			remove
			{
				this.Events.RemoveHandler(DeploymentManager.bindCompletedKey, value);
			}
		}

		// Token: 0x14000008 RID: 8
		// (add) Token: 0x060001E7 RID: 487 RVA: 0x0000CA17 File Offset: 0x0000BA17
		// (remove) Token: 0x060001E8 RID: 488 RVA: 0x0000CA2A File Offset: 0x0000BA2A
		public event SynchronizeCompletedEventHandler SynchronizeCompleted
		{
			add
			{
				this.Events.AddHandler(DeploymentManager.synchronizeCompletedKey, value);
			}
			remove
			{
				this.Events.RemoveHandler(DeploymentManager.synchronizeCompletedKey, value);
			}
		}

		// Token: 0x14000009 RID: 9
		// (add) Token: 0x060001E9 RID: 489 RVA: 0x0000CA3D File Offset: 0x0000BA3D
		// (remove) Token: 0x060001EA RID: 490 RVA: 0x0000CA50 File Offset: 0x0000BA50
		public event DeploymentProgressChangedEventHandler ProgressChanged
		{
			add
			{
				this.Events.AddHandler(DeploymentManager.progressChangedKey, value);
			}
			remove
			{
				this.Events.RemoveHandler(DeploymentManager.progressChangedKey, value);
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0000CA63 File Offset: 0x0000BA63
		public void Dispose()
		{
			this._events.Dispose();
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0000CA80 File Offset: 0x0000BA80
		void IDownloadNotification.DownloadModified(object sender, DownloadEventArgs e)
		{
			if (this._cancellationPending)
			{
				((FileDownloader)sender).Cancel();
			}
			this.asyncOperation.Post(this.progressReporter, new DeploymentProgressChangedEventArgs(e.Progress, null, e.BytesCompleted, e.BytesTotal, this._state, null));
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0000CAD0 File Offset: 0x0000BAD0
		void IDownloadNotification.DownloadCompleted(object sender, DownloadEventArgs e)
		{
			this._downloadedAppSize = e.BytesCompleted;
		}

		// Token: 0x1700009C RID: 156
		// (get) Token: 0x060001EE RID: 494 RVA: 0x0000CADE File Offset: 0x0000BADE
		internal ActivationDescription ActivationDescription
		{
			get
			{
				return this._actDesc;
			}
		}

		// Token: 0x1700009D RID: 157
		// (get) Token: 0x060001EF RID: 495 RVA: 0x0000CAE6 File Offset: 0x0000BAE6
		private EventHandlerList Events
		{
			get
			{
				return this._events;
			}
		}

		// Token: 0x060001F0 RID: 496 RVA: 0x0000CAF0 File Offset: 0x0000BAF0
		private void BindAsyncCompleted(object arg)
		{
			BindCompletedEventArgs bindCompletedEventArgs = (BindCompletedEventArgs)arg;
			BindCompletedEventHandler bindCompletedEventHandler = (BindCompletedEventHandler)this.Events[DeploymentManager.bindCompletedKey];
			if (bindCompletedEventHandler != null)
			{
				bindCompletedEventHandler(this, bindCompletedEventArgs);
			}
		}

		// Token: 0x060001F1 RID: 497 RVA: 0x0000CB28 File Offset: 0x0000BB28
		private void SynchronizeAsyncCompleted(object arg)
		{
			SynchronizeCompletedEventArgs synchronizeCompletedEventArgs = (SynchronizeCompletedEventArgs)arg;
			SynchronizeCompletedEventHandler synchronizeCompletedEventHandler = (SynchronizeCompletedEventHandler)this.Events[DeploymentManager.synchronizeCompletedKey];
			if (synchronizeCompletedEventHandler != null)
			{
				synchronizeCompletedEventHandler(this, synchronizeCompletedEventArgs);
			}
		}

		// Token: 0x060001F2 RID: 498 RVA: 0x0000CB60 File Offset: 0x0000BB60
		private void ProgressReporter(object arg)
		{
			DeploymentProgressChangedEventArgs deploymentProgressChangedEventArgs = (DeploymentProgressChangedEventArgs)arg;
			DeploymentProgressChangedEventHandler deploymentProgressChangedEventHandler = (DeploymentProgressChangedEventHandler)this.Events[DeploymentManager.progressChangedKey];
			if (deploymentProgressChangedEventHandler != null)
			{
				deploymentProgressChangedEventHandler(this, deploymentProgressChangedEventArgs);
			}
		}

		// Token: 0x060001F3 RID: 499 RVA: 0x0000CB98 File Offset: 0x0000BB98
		private void BindAsyncWorker()
		{
			Exception ex = null;
			bool flag = false;
			string text = null;
			TempFile tempFile = null;
			TempDirectory tempDirectory = null;
			FileStream fileStream = null;
			try
			{
				flag = this.BindCore(false, ref tempFile, ref tempDirectory, ref fileStream, ref text);
			}
			catch (Exception ex2)
			{
				if (ex2 is DownloadCancelledException)
				{
					flag = true;
				}
				else
				{
					ex = ex2;
				}
			}
			finally
			{
				this._state = DeploymentProgressState.DownloadingApplicationFiles;
				if (ex != null || flag)
				{
					if (tempDirectory != null)
					{
						tempDirectory.Dispose();
					}
					if (tempFile != null)
					{
						tempFile.Dispose();
					}
					if (fileStream != null)
					{
						fileStream.Close();
					}
				}
				BindCompletedEventArgs bindCompletedEventArgs = new BindCompletedEventArgs(ex, flag, null, this._actCtx, text, this._cached);
				this.asyncOperation.Post(this.bindCompleted, bindCompletedEventArgs);
			}
		}

		// Token: 0x060001F4 RID: 500 RVA: 0x0000CC50 File Offset: 0x0000BC50
		private bool BindCore(bool blocking, ref TempFile tempDeploy, ref TempDirectory tempAppDir, ref FileStream refTransaction, ref string productName)
		{
			try
			{
				if (this._deploySource == null)
				{
					return this.BindCoreWithAppId(blocking, ref refTransaction, ref productName);
				}
				Uri deploySource = this._deploySource;
				this._state = DeploymentProgressState.DownloadingDeploymentInformation;
				AssemblyManifest assemblyManifest = DownloadManager.DownloadDeploymentManifest(this._subStore, ref deploySource, out tempDeploy, blocking ? null : this, this._downloadOptions);
				string path = tempDeploy.Path;
				ActivationDescription activationDescription = new ActivationDescription();
				activationDescription.SetDeploymentManifest(assemblyManifest, deploySource, path);
				Logger.SetDeploymentManifest(this._log, assemblyManifest);
				activationDescription.IsUpdate = this._isupdate;
				if (activationDescription.DeployManifest.Deployment == null)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_NotDeploymentOrShortcut"));
				}
				if (!blocking && this._cancellationPending)
				{
					return true;
				}
				long num;
				refTransaction = this._subStore.AcquireReferenceTransaction(out num);
				SubscriptionState subscriptionState = this._subStore.GetSubscriptionState(activationDescription.DeployManifest);
				if (activationDescription.DeployManifest.Deployment.Install && activationDescription.DeployManifest.Deployment.ProviderCodebaseUri == null && subscriptionState != null && subscriptionState.DeploymentProviderUri != null && !subscriptionState.DeploymentProviderUri.Equals(deploySource))
				{
					throw new DeploymentException(ExceptionTypes.DeploymentUriDifferent, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_DeploymentUriDifferentExText"), new object[]
					{
						activationDescription.DeployManifest.Description.FilteredProduct,
						deploySource.AbsoluteUri,
						subscriptionState.DeploymentProviderUri.AbsoluteUri
					}));
				}
				DefinitionAppId definitionAppId = null;
				try
				{
					definitionAppId = new DefinitionAppId(activationDescription.ToAppCodebase(), new DefinitionIdentity[]
					{
						activationDescription.DeployManifest.Identity,
						new DefinitionIdentity(activationDescription.DeployManifest.MainDependentAssembly.Identity)
					});
				}
				catch (COMException ex)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_IdentityIsNotValid"), ex);
				}
				catch (SEHException ex2)
				{
					throw new InvalidDeploymentException(ExceptionTypes.InvalidManifest, Resources.GetString("Ex_IdentityIsNotValid"), ex2);
				}
				bool flag = this._subStore.CheckAndReferenceApplication(subscriptionState, definitionAppId, num);
				if (flag && definitionAppId.Equals(subscriptionState.CurrentBind))
				{
					this._bindAppId = definitionAppId;
					return this.BindCoreWithAppId(blocking, ref refTransaction, ref productName);
				}
				if (!blocking && this._cancellationPending)
				{
					return true;
				}
				this._state = DeploymentProgressState.DownloadingApplicationInformation;
				tempAppDir = this._subStore.AcquireTempDirectory();
				Uri uri;
				string text;
				AssemblyManifest assemblyManifest2 = DownloadManager.DownloadApplicationManifest(activationDescription.DeployManifest, tempAppDir.Path, activationDescription.DeploySourceUri, blocking ? null : this, this._downloadOptions, out uri, out text);
				AssemblyManifest.ReValidateManifestSignatures(activationDescription.DeployManifest, assemblyManifest2);
				Logger.SetApplicationManifest(this._log, assemblyManifest2);
				Logger.SetApplicationUrl(this._log, uri);
				activationDescription.SetApplicationManifest(assemblyManifest2, uri, text);
				activationDescription.AppId = new DefinitionAppId(activationDescription.ToAppCodebase(), new DefinitionIdentity[]
				{
					activationDescription.DeployManifest.Identity,
					activationDescription.AppManifest.Identity
				});
				flag = this._subStore.CheckAndReferenceApplication(subscriptionState, activationDescription.AppId, num);
				if (!blocking && this._cancellationPending)
				{
					return true;
				}
				Description effectiveDescription = activationDescription.EffectiveDescription;
				productName = effectiveDescription.Product;
				this._cached = flag;
				this._tempApplicationDirectory = tempAppDir;
				this._tempDeployment = tempDeploy;
				this._referenceTransaction = refTransaction;
				this._actCtx = DeploymentManager.ConstructActivationContext(activationDescription);
				this._actDesc = activationDescription;
			}
			catch (Exception ex3)
			{
				this.LogError(Resources.GetString("Ex_FailedToDownloadManifest"), ex3);
				throw;
			}
			return false;
		}

		// Token: 0x060001F5 RID: 501 RVA: 0x0000D02C File Offset: 0x0000C02C
		private bool BindCoreWithAppId(bool blocking, ref FileStream refTransaction, ref string productName)
		{
			DefinitionIdentity definitionIdentity = this._bindAppId.DeploymentIdentity.ToSubscriptionId();
			SubscriptionState subscriptionState = this._subStore.GetSubscriptionState(definitionIdentity);
			if (!subscriptionState.IsInstalled)
			{
				throw new InvalidDeploymentException(Resources.GetString("Ex_BindAppIdNotInstalled"));
			}
			if (!this._bindAppId.Equals(subscriptionState.CurrentBind))
			{
				throw new InvalidDeploymentException(Resources.GetString("Ex_BindAppIdNotCurrrent"));
			}
			if (!blocking && this._cancellationPending)
			{
				return true;
			}
			long num;
			refTransaction = this._subStore.AcquireReferenceTransaction(out num);
			bool flag = this._subStore.CheckAndReferenceApplication(subscriptionState, this._bindAppId, num);
			ActivationDescription activationDescription = new ActivationDescription();
			activationDescription.SetDeploymentManifest(subscriptionState.CurrentDeploymentManifest, subscriptionState.CurrentDeploymentSourceUri, null);
			Logger.SetDeploymentManifest(this._log, subscriptionState.CurrentDeploymentManifest);
			activationDescription.IsUpdate = this._isupdate;
			activationDescription.SetApplicationManifest(subscriptionState.CurrentApplicationManifest, subscriptionState.CurrentApplicationSourceUri, null);
			Logger.SetApplicationManifest(this._log, subscriptionState.CurrentApplicationManifest);
			Logger.SetApplicationUrl(this._log, subscriptionState.CurrentApplicationSourceUri);
			activationDescription.AppId = new DefinitionAppId(activationDescription.ToAppCodebase(), new DefinitionIdentity[]
			{
				activationDescription.DeployManifest.Identity,
				activationDescription.AppManifest.Identity
			});
			if (!blocking && this._cancellationPending)
			{
				return true;
			}
			Description effectiveDescription = subscriptionState.EffectiveDescription;
			productName = effectiveDescription.Product;
			this._cached = flag;
			this._referenceTransaction = refTransaction;
			this._actCtx = DeploymentManager.ConstructActivationContextFromStore(activationDescription.AppId);
			this._actDesc = activationDescription;
			return false;
		}

		// Token: 0x060001F6 RID: 502 RVA: 0x0000D1B8 File Offset: 0x0000C1B8
		private bool DeterminePlatformRequirementsCore(bool blocking)
		{
			try
			{
				if (!blocking && this._cancellationPending)
				{
					return true;
				}
				using (TempDirectory tempDirectory = this._subStore.AcquireTempDirectory())
				{
					PlatformDetector.VerifyPlatformDependencies(this._actDesc.AppManifest, this._actDesc.DeployManifest.Description.SupportUri, tempDirectory.Path);
				}
			}
			catch (Exception ex)
			{
				this.LogError(Resources.GetString("Ex_DeterminePlatformRequirementsFailed"), ex);
				throw;
			}
			return false;
		}

		// Token: 0x060001F7 RID: 503 RVA: 0x0000D24C File Offset: 0x0000C24C
		private bool DetermineTrustCore(bool blocking, TrustParams tp)
		{
			try
			{
				SubscriptionState subscriptionState = this._subStore.GetSubscriptionState(this._actDesc.DeployManifest);
				TrustManagerContext trustManagerContext = new TrustManagerContext();
				trustManagerContext.IgnorePersistedDecision = false;
				trustManagerContext.NoPrompt = false;
				trustManagerContext.Persist = true;
				if (tp != null)
				{
					trustManagerContext.NoPrompt = tp.NoPrompt;
				}
				if (!blocking && this._cancellationPending)
				{
					return true;
				}
				if (subscriptionState.IsInstalled && !string.Equals(subscriptionState.EffectiveCertificatePublicKeyToken, this._actDesc.EffectiveCertificatePublicKeyToken, StringComparison.Ordinal))
				{
					ApplicationTrust.RemoveCachedTrust(subscriptionState.CurrentBind);
				}
				this._actDesc.Trust = ApplicationTrust.RequestTrust(subscriptionState, this._actDesc.DeployManifest.Deployment.Install, this._actDesc.IsUpdate, this._actCtx, trustManagerContext);
			}
			catch (Exception ex)
			{
				this.LogError(Resources.GetString("Ex_DetermineTrustFailed"), ex);
				throw;
			}
			return false;
		}

		// Token: 0x060001F8 RID: 504 RVA: 0x0000D338 File Offset: 0x0000C338
		public void PersistTrustWithoutEvaluation()
		{
			this._actDesc.Trust = ApplicationTrust.PersistTrustWithoutEvaluation(this._actCtx);
		}

		// Token: 0x060001F9 RID: 505 RVA: 0x0000D350 File Offset: 0x0000C350
		private void SynchronizeAsyncWorker()
		{
			Exception ex = null;
			bool flag = false;
			try
			{
				flag = this.SynchronizeCore(false);
			}
			catch (Exception ex2)
			{
				if (ex2 is DownloadCancelledException)
				{
					flag = true;
				}
				else
				{
					ex = ex2;
				}
			}
			finally
			{
				SynchronizeCompletedEventArgs synchronizeCompletedEventArgs = new SynchronizeCompletedEventArgs(ex, flag, null, null);
				this.asyncOperation.Post(this.synchronizeCompleted, synchronizeCompletedEventArgs);
			}
		}

		// Token: 0x060001FA RID: 506 RVA: 0x0000D3B8 File Offset: 0x0000C3B8
		private bool SynchronizeCore(bool blocking)
		{
			try
			{
				AssemblyManifest deployManifest = this._actDesc.DeployManifest;
				SubscriptionState subscriptionState = this._subStore.GetSubscriptionState(deployManifest);
				this._subStore.CheckDeploymentSubscriptionState(subscriptionState, deployManifest);
				this._subStore.CheckCustomUXFlag(subscriptionState, this._actDesc.AppManifest);
				if (this._actDesc.DeployManifestPath != null)
				{
					this._actDesc.CommitDeploy = true;
					this._actDesc.IsConfirmed = this._isConfirmed;
					this._actDesc.TimeStamp = DateTime.UtcNow;
				}
				else
				{
					this._actDesc.CommitDeploy = false;
				}
				if (!blocking && this._cancellationPending)
				{
					return true;
				}
				if (!this._cached)
				{
					bool flag = false;
					if (this._actDesc.appType != AppType.CustomHostSpecified)
					{
						if (this._actDesc.Trust != null)
						{
							bool flag2 = this._actDesc.Trust.DefaultGrantSet.PermissionSet.IsUnrestricted();
							if (!flag2 && this._actDesc.AppManifest.FileAssociations.Length > 0)
							{
								throw new DeploymentException(ExceptionTypes.ManifestSemanticValidation, Resources.GetString("Ex_FileExtensionNotSupported"));
							}
							bool flag3 = !this._actDesc.DeployManifest.Deployment.Install;
							if (!flag2 && flag3)
							{
								if (this._downloadOptions == null)
								{
									this._downloadOptions = new DownloadOptions();
								}
								this._downloadOptions.EnforceSizeLimit = true;
								this._downloadOptions.SizeLimit = this._subStore.GetSizeLimitInBytesForSemiTrustApps();
								this._downloadOptions.Size = this._actDesc.DeployManifest.SizeInBytes + this._actDesc.AppManifest.SizeInBytes;
							}
						}
						else
						{
							flag = true;
						}
					}
					DownloadManager.DownloadDependencies(subscriptionState, this._actDesc.DeployManifest, this._actDesc.AppManifest, this._actDesc.AppSourceUri, this._tempApplicationDirectory.Path, null, blocking ? null : this, this._downloadOptions);
					if (!blocking && this._cancellationPending)
					{
						return true;
					}
					this.WaitForAssertApplicationRequirements();
					if (flag)
					{
						this.CheckSizeLimit();
					}
					this._actDesc.CommitApp = true;
					this._actDesc.AppPayloadPath = this._tempApplicationDirectory.Path;
				}
				if (this._actDesc.CommitDeploy || this._actDesc.CommitApp)
				{
					this._subStore.CommitApplication(ref subscriptionState, this._actDesc);
				}
				if (this._tempApplicationDirectory != null)
				{
					this._tempApplicationDirectory.Dispose();
					this._tempApplicationDirectory = null;
				}
				if (this._tempDeployment != null)
				{
					this._tempDeployment.Dispose();
					this._tempDeployment = null;
				}
				if (this._referenceTransaction != null)
				{
					this._referenceTransaction.Close();
					this._referenceTransaction = null;
				}
				ActivationContext actCtx = this._actCtx;
				this._actCtx = DeploymentManager.ConstructActivationContextFromStore(this._actDesc.AppId);
				actCtx.Dispose();
				this._cached = true;
			}
			catch (Exception ex)
			{
				this.LogError(Resources.GetString("Ex_DownloadApplicationFailed"), ex);
				throw;
			}
			return false;
		}

		// Token: 0x060001FB RID: 507 RVA: 0x0000D6BC File Offset: 0x0000C6BC
		private void WaitForAssertApplicationRequirements()
		{
			if (this._actDesc.appType == AppType.CustomHostSpecified)
			{
				return;
			}
			if (this._callerType == DeploymentManager.CallerType.ApplicationDeployment)
			{
				return;
			}
			int num = WaitHandle.WaitAny(this._assertApplicationReqEvents, Constants.AssertApplicationRequirementsTimeout, false);
			if (num == 258)
			{
				throw new DeploymentException(Resources.GetString("Ex_CannotCommitNoTrustDecision"));
			}
			if (num == 0)
			{
				throw new DeploymentException(Resources.GetString("Ex_CannotCommitTrustFailed"));
			}
			if (num == 1)
			{
				throw new DeploymentException(Resources.GetString("Ex_CannotCommitPlatformRequirementsFailed"));
			}
		}

		// Token: 0x060001FC RID: 508 RVA: 0x0000D734 File Offset: 0x0000C734
		private void CheckSizeLimit()
		{
			if (this._actDesc.appType == AppType.CustomHostSpecified)
			{
				return;
			}
			bool flag = this._actDesc.Trust.DefaultGrantSet.PermissionSet.IsUnrestricted();
			bool flag2 = !this._actDesc.DeployManifest.Deployment.Install;
			if (!flag && flag2)
			{
				ulong sizeLimitInBytesForSemiTrustApps = this._subStore.GetSizeLimitInBytesForSemiTrustApps();
				if (this._downloadedAppSize > (long)sizeLimitInBytesForSemiTrustApps)
				{
					throw new DeploymentDownloadException(ExceptionTypes.SizeLimitForPartialTrustOnlineAppExceeded, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_OnlineSemiTrustAppSizeLimitExceeded"), new object[] { sizeLimitInBytesForSemiTrustApps }));
				}
			}
		}

		// Token: 0x060001FD RID: 509 RVA: 0x0000D7CC File Offset: 0x0000C7CC
		private void SynchronizeGroupAsyncWorker(object arg)
		{
			Exception ex = null;
			bool flag = false;
			string text = null;
			object obj = null;
			try
			{
				SyncGroupHelper syncGroupHelper = (SyncGroupHelper)arg;
				text = syncGroupHelper.Group;
				obj = syncGroupHelper.UserState;
				flag = this.SynchronizeGroupCore(false, syncGroupHelper);
			}
			catch (Exception ex2)
			{
				if (ex2 is DownloadCancelledException)
				{
					flag = true;
				}
				else
				{
					ex = ex2;
				}
			}
			finally
			{
				SynchronizeCompletedEventArgs synchronizeCompletedEventArgs = new SynchronizeCompletedEventArgs(ex, flag, obj, text);
				this.asyncOperation.Post(this.synchronizeCompleted, synchronizeCompletedEventArgs);
			}
		}

		// Token: 0x060001FE RID: 510 RVA: 0x0000D858 File Offset: 0x0000C858
		private bool SynchronizeGroupCore(bool blocking, SyncGroupHelper sgh)
		{
			TempDirectory tempDirectory = null;
			try
			{
				string group = sgh.Group;
				SubscriptionState subscriptionState = this._subStore.GetSubscriptionState(this._actDesc.DeployManifest);
				if (this._subStore.CheckGroupInstalled(subscriptionState, this._actDesc.AppId, this._actDesc.AppManifest, group))
				{
					return false;
				}
				bool flag = AppDomain.CurrentDomain.ApplicationTrust.DefaultGrantSet.PermissionSet.IsUnrestricted();
				if (!flag && this._actDesc.AppManifest.FileAssociations.Length > 0)
				{
					throw new DeploymentException(ExceptionTypes.ManifestSemanticValidation, Resources.GetString("Ex_FileExtensionNotSupported"));
				}
				bool flag2 = !this._actDesc.DeployManifest.Deployment.Install;
				if (!flag && flag2)
				{
					if (this._downloadOptions == null)
					{
						this._downloadOptions = new DownloadOptions();
					}
					this._downloadOptions.EnforceSizeLimit = true;
					this._downloadOptions.SizeLimit = this._subStore.GetSizeLimitInBytesForSemiTrustApps();
					this._downloadOptions.Size = this._subStore.GetPrivateSize(this._actDesc.AppId);
				}
				tempDirectory = this._subStore.AcquireTempDirectory();
				DownloadManager.DownloadDependencies(subscriptionState, this._actDesc.DeployManifest, this._actDesc.AppManifest, this._actDesc.AppSourceUri, tempDirectory.Path, group, blocking ? null : sgh, this._downloadOptions);
				if (!blocking && sgh.CancellationPending)
				{
					return true;
				}
				CommitApplicationParams commitApplicationParams = new CommitApplicationParams(this._actDesc);
				commitApplicationParams.CommitApp = true;
				commitApplicationParams.AppPayloadPath = tempDirectory.Path;
				commitApplicationParams.AppManifestPath = null;
				commitApplicationParams.AppGroup = group;
				commitApplicationParams.CommitDeploy = false;
				this._subStore.CommitApplication(ref subscriptionState, commitApplicationParams);
			}
			finally
			{
				this.DetachFromGroup(sgh);
				if (tempDirectory != null)
				{
					tempDirectory.Dispose();
				}
			}
			return false;
		}

		// Token: 0x060001FF RID: 511 RVA: 0x0000DA40 File Offset: 0x0000CA40
		private SyncGroupHelper AttachToGroup(string groupName, object userState, out bool created)
		{
			created = false;
			SyncGroupHelper syncGroupHelper = null;
			lock (this._syncGroupMap.SyncRoot)
			{
				syncGroupHelper = (SyncGroupHelper)this._syncGroupMap[groupName];
				if (syncGroupHelper == null)
				{
					syncGroupHelper = new SyncGroupHelper(groupName, userState, this.asyncOperation, this.progressReporter);
					this._syncGroupMap[groupName] = syncGroupHelper;
					created = true;
				}
			}
			return syncGroupHelper;
		}

		// Token: 0x06000200 RID: 512 RVA: 0x0000DAB8 File Offset: 0x0000CAB8
		private void DetachFromGroup(SyncGroupHelper sgh)
		{
			string group = sgh.Group;
			lock (this._syncGroupMap.SyncRoot)
			{
				this._syncGroupMap.Remove(group);
			}
			sgh.SetComplete();
		}

		// Token: 0x06000201 RID: 513 RVA: 0x0000DB0C File Offset: 0x0000CB0C
		private void Dispose(bool disposing)
		{
			if (disposing)
			{
				Logger.EndLogging(this._log);
				if (this._tempDeployment != null)
				{
					this._tempDeployment.Dispose();
				}
				if (this._tempApplicationDirectory != null)
				{
					this._tempApplicationDirectory.Dispose();
				}
				if (this._referenceTransaction != null)
				{
					this._referenceTransaction.Close();
				}
				if (this._actCtx != null)
				{
					this._actCtx.Dispose();
				}
				if (this._events != null)
				{
					this._events.Dispose();
				}
			}
		}

		// Token: 0x06000202 RID: 514 RVA: 0x0000DB88 File Offset: 0x0000CB88
		private static ActivationContext ConstructActivationContext(ActivationDescription actDesc)
		{
			ApplicationIdentity applicationIdentity = actDesc.AppId.ToApplicationIdentity();
			return ActivationContext.CreatePartialActivationContext(applicationIdentity, new string[] { actDesc.DeployManifestPath, actDesc.AppManifestPath });
		}

		// Token: 0x06000203 RID: 515 RVA: 0x0000DBC1 File Offset: 0x0000CBC1
		private static ActivationContext ConstructActivationContextFromStore(DefinitionAppId defAppId)
		{
			return ActivationContext.CreatePartialActivationContext(defAppId.ToApplicationIdentity());
		}

		// Token: 0x06000204 RID: 516 RVA: 0x0000DBCE File Offset: 0x0000CBCE
		private void LogError(string message, Exception ex)
		{
			Logger.AddErrorInformation(this._log, message, ex);
			Logger.FlushLog(this._log);
		}

		// Token: 0x04000198 RID: 408
		private static readonly object bindCompletedKey = new object();

		// Token: 0x04000199 RID: 409
		private static readonly object synchronizeCompletedKey = new object();

		// Token: 0x0400019A RID: 410
		private static readonly object progressChangedKey = new object();

		// Token: 0x0400019B RID: 411
		private readonly ThreadStart bindWorker;

		// Token: 0x0400019C RID: 412
		private readonly ThreadStart synchronizeWorker;

		// Token: 0x0400019D RID: 413
		private readonly WaitCallback synchronizeGroupWorker;

		// Token: 0x0400019E RID: 414
		private readonly SendOrPostCallback bindCompleted;

		// Token: 0x0400019F RID: 415
		private readonly SendOrPostCallback synchronizeCompleted;

		// Token: 0x040001A0 RID: 416
		private readonly SendOrPostCallback progressReporter;

		// Token: 0x040001A1 RID: 417
		private readonly AsyncOperation asyncOperation;

		// Token: 0x040001A2 RID: 418
		private int _bindGuard;

		// Token: 0x040001A3 RID: 419
		private int _syncGuard;

		// Token: 0x040001A4 RID: 420
		private bool _cancellationPending;

		// Token: 0x040001A5 RID: 421
		private bool _cached;

		// Token: 0x040001A6 RID: 422
		private ManualResetEvent _trustNotGrantedEvent = new ManualResetEvent(false);

		// Token: 0x040001A7 RID: 423
		private ManualResetEvent _platformRequirementsSucceededEvent = new ManualResetEvent(false);

		// Token: 0x040001A8 RID: 424
		private ManualResetEvent _platformRequirementsFailedEvent = new ManualResetEvent(false);

		// Token: 0x040001A9 RID: 425
		private ManualResetEvent[] _assertApplicationReqEvents;

		// Token: 0x040001AA RID: 426
		private DeploymentManager.CallerType _callerType;

		// Token: 0x040001AB RID: 427
		private Uri _deploySource;

		// Token: 0x040001AC RID: 428
		private DefinitionAppId _bindAppId;

		// Token: 0x040001AD RID: 429
		private SubscriptionStore _subStore;

		// Token: 0x040001AE RID: 430
		private bool _isupdate;

		// Token: 0x040001AF RID: 431
		private bool _isConfirmed = true;

		// Token: 0x040001B0 RID: 432
		private DownloadOptions _downloadOptions;

		// Token: 0x040001B1 RID: 433
		private EventHandlerList _events;

		// Token: 0x040001B2 RID: 434
		private Hashtable _syncGroupMap;

		// Token: 0x040001B3 RID: 435
		private ActivationDescription _actDesc;

		// Token: 0x040001B4 RID: 436
		private ActivationContext _actCtx;

		// Token: 0x040001B5 RID: 437
		private DeploymentProgressState _state = DeploymentProgressState.DownloadingApplicationFiles;

		// Token: 0x040001B6 RID: 438
		private TempFile _tempDeployment;

		// Token: 0x040001B7 RID: 439
		private TempDirectory _tempApplicationDirectory;

		// Token: 0x040001B8 RID: 440
		private FileStream _referenceTransaction;

		// Token: 0x040001B9 RID: 441
		private Logger.LogIdentity _log;

		// Token: 0x040001BA RID: 442
		private long _downloadedAppSize;

		// Token: 0x0200003B RID: 59
		public enum CallerType
		{
			// Token: 0x040001BC RID: 444
			Other,
			// Token: 0x040001BD RID: 445
			ApplicationDeployment,
			// Token: 0x040001BE RID: 446
			InPlaceHostingManager
		}
	}
}
