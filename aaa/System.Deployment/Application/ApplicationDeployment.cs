using System;
using System.ComponentModel;
using System.Deployment.Application.Manifest;
using System.Deployment.Internal;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Threading;

namespace System.Deployment.Application
{
	// Token: 0x02000008 RID: 8
	public sealed class ApplicationDeployment
	{
		// Token: 0x06000026 RID: 38 RVA: 0x00003CF0 File Offset: 0x00002CF0
		private ApplicationDeployment(string fullAppId)
		{
			if (fullAppId.Length > 65536)
			{
				throw new InvalidDeploymentException(Resources.GetString("Ex_AppIdTooLong"));
			}
			try
			{
				this._fullAppId = new DefinitionAppId(fullAppId);
			}
			catch (COMException ex)
			{
				throw new InvalidDeploymentException(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_SubAppIdNotValid"), new object[] { fullAppId }), ex);
			}
			catch (SEHException ex2)
			{
				throw new InvalidDeploymentException(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_SubAppIdNotValid"), new object[] { fullAppId }), ex2);
			}
			DefinitionIdentity deploymentIdentity = this._fullAppId.DeploymentIdentity;
			this._currentVersion = deploymentIdentity.Version;
			DefinitionIdentity definitionIdentity = deploymentIdentity.ToSubscriptionId();
			this._subStore = SubscriptionStore.CurrentUser;
			this._subState = this._subStore.GetSubscriptionState(definitionIdentity);
			if (!this._subState.IsInstalled)
			{
				throw new InvalidDeploymentException(Resources.GetString("Ex_SubNotInstalled"));
			}
			if (!this._fullAppId.Equals(this._subState.CurrentBind))
			{
				throw new InvalidDeploymentException(Resources.GetString("Ex_AppIdNotMatchInstalled"));
			}
			Uri uri = new Uri(this._fullAppId.Codebase);
			if (uri.IsFile)
			{
				this.accessPermission = new FileIOPermission(FileIOPermissionAccess.Read, uri.LocalPath);
			}
			else
			{
				this.accessPermission = new WebPermission(NetworkAccess.Connect, this._fullAppId.Codebase);
			}
			this.accessPermission.Demand();
			this._events = new EventHandlerList();
			this.asyncOperation = AsyncOperationManager.CreateOperation(null);
		}

		// Token: 0x17000004 RID: 4
		// (get) Token: 0x06000027 RID: 39 RVA: 0x00003E8C File Offset: 0x00002E8C
		public static ApplicationDeployment CurrentDeployment
		{
			[PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
			get
			{
				bool flag = false;
				if (ApplicationDeployment._currentDeployment == null)
				{
					lock (ApplicationDeployment.lockObject)
					{
						if (ApplicationDeployment._currentDeployment == null)
						{
							string text = null;
							ActivationContext activationContext = AppDomain.CurrentDomain.ActivationContext;
							if (activationContext != null)
							{
								text = activationContext.Identity.FullName;
							}
							if (string.IsNullOrEmpty(text))
							{
								throw new InvalidDeploymentException(Resources.GetString("Ex_AppIdNotSet"));
							}
							ApplicationDeployment._currentDeployment = new ApplicationDeployment(text);
							flag = true;
						}
					}
				}
				if (!flag)
				{
					ApplicationDeployment._currentDeployment.DemandPermission();
				}
				return ApplicationDeployment._currentDeployment;
			}
		}

		// Token: 0x17000005 RID: 5
		// (get) Token: 0x06000028 RID: 40 RVA: 0x00003F24 File Offset: 0x00002F24
		public static bool IsNetworkDeployed
		{
			get
			{
				bool flag = true;
				try
				{
					ApplicationDeployment currentDeployment = ApplicationDeployment.CurrentDeployment;
				}
				catch (InvalidDeploymentException)
				{
					flag = false;
				}
				return flag;
			}
		}

		// Token: 0x17000006 RID: 6
		// (get) Token: 0x06000029 RID: 41 RVA: 0x00003F54 File Offset: 0x00002F54
		public Version CurrentVersion
		{
			get
			{
				return this._currentVersion;
			}
		}

		// Token: 0x17000007 RID: 7
		// (get) Token: 0x0600002A RID: 42 RVA: 0x00003F5C File Offset: 0x00002F5C
		public Version UpdatedVersion
		{
			[PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
			get
			{
				this._subState.Invalidate();
				return this._subState.CurrentDeployment.Version;
			}
		}

		// Token: 0x17000008 RID: 8
		// (get) Token: 0x0600002B RID: 43 RVA: 0x00003F79 File Offset: 0x00002F79
		public string UpdatedApplicationFullName
		{
			[PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
			get
			{
				this._subState.Invalidate();
				return this._subState.CurrentBind.ToString();
			}
		}

		// Token: 0x17000009 RID: 9
		// (get) Token: 0x0600002C RID: 44 RVA: 0x00003F96 File Offset: 0x00002F96
		public DateTime TimeOfLastUpdateCheck
		{
			[PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
			get
			{
				this._subState.Invalidate();
				return this._subState.LastCheckTime;
			}
		}

		// Token: 0x1700000A RID: 10
		// (get) Token: 0x0600002D RID: 45 RVA: 0x00003FAE File Offset: 0x00002FAE
		public Uri UpdateLocation
		{
			[PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
			get
			{
				this._subState.Invalidate();
				return this._subState.DeploymentProviderUri;
			}
		}

		// Token: 0x1700000B RID: 11
		// (get) Token: 0x0600002E RID: 46 RVA: 0x00003FC8 File Offset: 0x00002FC8
		public Uri ActivationUri
		{
			[PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
			get
			{
				this._subState.Invalidate();
				if (!this._subState.CurrentDeploymentManifest.Deployment.TrustURLParameters)
				{
					return null;
				}
				string[] activationData = AppDomain.CurrentDomain.SetupInformation.ActivationArguments.ActivationData;
				if (activationData == null || activationData[0] == null)
				{
					return null;
				}
				Uri uri = new Uri(activationData[0]);
				if (uri.IsFile || uri.IsUnc)
				{
					return null;
				}
				return uri;
			}
		}

		// Token: 0x1700000C RID: 12
		// (get) Token: 0x0600002F RID: 47 RVA: 0x00004034 File Offset: 0x00003034
		public string DataDirectory
		{
			get
			{
				object data = AppDomain.CurrentDomain.GetData("DataDirectory");
				if (data == null)
				{
					return null;
				}
				return data.ToString();
			}
		}

		// Token: 0x1700000D RID: 13
		// (get) Token: 0x06000030 RID: 48 RVA: 0x0000405C File Offset: 0x0000305C
		public bool IsFirstRun
		{
			[PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
			get
			{
				ActivationContext activationContext = AppDomain.CurrentDomain.ActivationContext;
				return InternalActivationContextHelper.IsFirstRun(activationContext);
			}
		}

		// Token: 0x06000031 RID: 49 RVA: 0x0000407A File Offset: 0x0000307A
		public UpdateCheckInfo CheckForDetailedUpdate()
		{
			return this.CheckForDetailedUpdate(true);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00004084 File Offset: 0x00003084
		public UpdateCheckInfo CheckForDetailedUpdate(bool persistUpdateCheckResult)
		{
			new NamedPermissionSet("FullTrust").Demand();
			if (Interlocked.CompareExchange(ref this._guard, 2, 0) != 0)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_SingleOperation"));
			}
			this._cancellationPending = false;
			UpdateCheckInfo updateCheckInfo = null;
			try
			{
				DeploymentManager deploymentManager = this.CreateDeploymentManager();
				try
				{
					deploymentManager.Bind();
					deploymentManager.DetermineTrust(new TrustParams
					{
						NoPrompt = true
					});
					deploymentManager.DeterminePlatformRequirements();
					updateCheckInfo = this.DetermineUpdateCheckResult(deploymentManager.ActivationDescription);
					if (persistUpdateCheckResult)
					{
						this.ProcessUpdateCheckResult(updateCheckInfo, deploymentManager.ActivationDescription);
					}
				}
				finally
				{
					deploymentManager.Dispose();
				}
			}
			finally
			{
				Interlocked.Exchange(ref this._guard, 0);
			}
			return updateCheckInfo;
		}

		// Token: 0x06000033 RID: 51 RVA: 0x00004144 File Offset: 0x00003144
		public bool CheckForUpdate()
		{
			return this.CheckForUpdate(true);
		}

		// Token: 0x06000034 RID: 52 RVA: 0x00004150 File Offset: 0x00003150
		public bool CheckForUpdate(bool persistUpdateCheckResult)
		{
			UpdateCheckInfo updateCheckInfo = this.CheckForDetailedUpdate(persistUpdateCheckResult);
			return updateCheckInfo.UpdateAvailable;
		}

		// Token: 0x06000035 RID: 53 RVA: 0x0000416C File Offset: 0x0000316C
		public void CheckForUpdateAsync()
		{
			new NamedPermissionSet("FullTrust").Demand();
			if (Interlocked.CompareExchange(ref this._guard, 1, 0) != 0)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_SingleOperation"));
			}
			this._cancellationPending = false;
			DeploymentManager deploymentManager = this.CreateDeploymentManager();
			deploymentManager.ProgressChanged += this.CheckForUpdateProgressChangedEventHandler;
			deploymentManager.BindCompleted += this.CheckForUpdateBindCompletedEventHandler;
			deploymentManager.BindAsync();
		}

		// Token: 0x06000036 RID: 54 RVA: 0x000041DF File Offset: 0x000031DF
		public void CheckForUpdateAsyncCancel()
		{
			if (this._guard == 1)
			{
				this._cancellationPending = true;
			}
		}

		// Token: 0x14000001 RID: 1
		// (add) Token: 0x06000037 RID: 55 RVA: 0x000041F1 File Offset: 0x000031F1
		// (remove) Token: 0x06000038 RID: 56 RVA: 0x00004204 File Offset: 0x00003204
		public event DeploymentProgressChangedEventHandler CheckForUpdateProgressChanged
		{
			add
			{
				this.Events.AddHandler(ApplicationDeployment.checkForUpdateProgressChangedKey, value);
			}
			remove
			{
				this.Events.RemoveHandler(ApplicationDeployment.checkForUpdateProgressChangedKey, value);
			}
		}

		// Token: 0x14000002 RID: 2
		// (add) Token: 0x06000039 RID: 57 RVA: 0x00004217 File Offset: 0x00003217
		// (remove) Token: 0x0600003A RID: 58 RVA: 0x0000422A File Offset: 0x0000322A
		public event CheckForUpdateCompletedEventHandler CheckForUpdateCompleted
		{
			add
			{
				this.Events.AddHandler(ApplicationDeployment.checkForUpdateCompletedKey, value);
			}
			remove
			{
				this.Events.RemoveHandler(ApplicationDeployment.checkForUpdateCompletedKey, value);
			}
		}

		// Token: 0x0600003B RID: 59 RVA: 0x00004240 File Offset: 0x00003240
		public bool Update()
		{
			new NamedPermissionSet("FullTrust").Demand();
			if (Interlocked.CompareExchange(ref this._guard, 2, 0) != 0)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_SingleOperation"));
			}
			this._cancellationPending = false;
			try
			{
				DeploymentManager deploymentManager = this.CreateDeploymentManager();
				try
				{
					deploymentManager.Bind();
					deploymentManager.DetermineTrust(new TrustParams
					{
						NoPrompt = true
					});
					deploymentManager.DeterminePlatformRequirements();
					UpdateCheckInfo updateCheckInfo = this.DetermineUpdateCheckResult(deploymentManager.ActivationDescription);
					this.ProcessUpdateCheckResult(updateCheckInfo, deploymentManager.ActivationDescription);
					if (!updateCheckInfo.UpdateAvailable)
					{
						return false;
					}
					deploymentManager.Synchronize();
				}
				finally
				{
					deploymentManager.Dispose();
				}
			}
			finally
			{
				Interlocked.Exchange(ref this._guard, 0);
			}
			return true;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00004310 File Offset: 0x00003310
		public void UpdateAsync()
		{
			new NamedPermissionSet("FullTrust").Demand();
			if (Interlocked.CompareExchange(ref this._guard, 1, 0) != 0)
			{
				throw new InvalidOperationException(Resources.GetString("Ex_SingleOperation"));
			}
			this._cancellationPending = false;
			DeploymentManager deploymentManager = this.CreateDeploymentManager();
			deploymentManager.ProgressChanged += this.UpdateProgressChangedEventHandler;
			deploymentManager.BindCompleted += this.UpdateBindCompletedEventHandler;
			deploymentManager.SynchronizeCompleted += this.SynchronizeNullCompletedEventHandler;
			deploymentManager.BindAsync();
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00004395 File Offset: 0x00003395
		public void UpdateAsyncCancel()
		{
			if (this._guard == 1)
			{
				this._cancellationPending = true;
			}
		}

		// Token: 0x14000003 RID: 3
		// (add) Token: 0x0600003E RID: 62 RVA: 0x000043A7 File Offset: 0x000033A7
		// (remove) Token: 0x0600003F RID: 63 RVA: 0x000043BA File Offset: 0x000033BA
		public event DeploymentProgressChangedEventHandler UpdateProgressChanged
		{
			add
			{
				this.Events.AddHandler(ApplicationDeployment.updateProgressChangedKey, value);
			}
			remove
			{
				this.Events.RemoveHandler(ApplicationDeployment.updateProgressChangedKey, value);
			}
		}

		// Token: 0x14000004 RID: 4
		// (add) Token: 0x06000040 RID: 64 RVA: 0x000043CD File Offset: 0x000033CD
		// (remove) Token: 0x06000041 RID: 65 RVA: 0x000043E0 File Offset: 0x000033E0
		public event AsyncCompletedEventHandler UpdateCompleted
		{
			add
			{
				this.Events.AddHandler(ApplicationDeployment.updateCompletedKey, value);
			}
			remove
			{
				this.Events.RemoveHandler(ApplicationDeployment.updateCompletedKey, value);
			}
		}

		// Token: 0x06000042 RID: 66 RVA: 0x000043F4 File Offset: 0x000033F4
		[PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
		public void DownloadFileGroup(string groupName)
		{
			if (groupName == null)
			{
				throw new ArgumentNullException("groupName");
			}
			this._subState.Invalidate();
			if (!this._fullAppId.Equals(this._subState.CurrentBind))
			{
				throw new InvalidOperationException(Resources.GetString("Ex_DownloadGroupAfterUpdate"));
			}
			this.SyncGroupDeploymentManager.Synchronize(groupName);
		}

		// Token: 0x06000043 RID: 67 RVA: 0x0000444E File Offset: 0x0000344E
		public void DownloadFileGroupAsync(string groupName)
		{
			this.DownloadFileGroupAsync(groupName, null);
		}

		// Token: 0x06000044 RID: 68 RVA: 0x00004458 File Offset: 0x00003458
		[PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
		public void DownloadFileGroupAsync(string groupName, object userState)
		{
			if (groupName == null)
			{
				throw new ArgumentNullException("groupName");
			}
			this._subState.Invalidate();
			if (!this._fullAppId.Equals(this._subState.CurrentBind))
			{
				throw new InvalidOperationException(Resources.GetString("Ex_DownloadGroupAfterUpdate"));
			}
			this.SyncGroupDeploymentManager.SynchronizeAsync(groupName, userState);
		}

		// Token: 0x06000045 RID: 69 RVA: 0x000044B3 File Offset: 0x000034B3
		[PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
		public bool IsFileGroupDownloaded(string groupName)
		{
			return this._subStore.CheckGroupInstalled(this._subState, this._fullAppId, groupName);
		}

		// Token: 0x06000046 RID: 70 RVA: 0x000044CD File Offset: 0x000034CD
		[PermissionSet(SecurityAction.Assert, Name = "FullTrust")]
		public void DownloadFileGroupAsyncCancel(string groupName)
		{
			if (groupName == null)
			{
				throw new ArgumentNullException("groupName");
			}
			this.SyncGroupDeploymentManager.CancelAsync(groupName);
		}

		// Token: 0x14000005 RID: 5
		// (add) Token: 0x06000047 RID: 71 RVA: 0x000044E9 File Offset: 0x000034E9
		// (remove) Token: 0x06000048 RID: 72 RVA: 0x000044FC File Offset: 0x000034FC
		public event DeploymentProgressChangedEventHandler DownloadFileGroupProgressChanged
		{
			add
			{
				this.Events.AddHandler(ApplicationDeployment.downloadFileGroupProgressChangedKey, value);
			}
			remove
			{
				this.Events.RemoveHandler(ApplicationDeployment.downloadFileGroupProgressChangedKey, value);
			}
		}

		// Token: 0x14000006 RID: 6
		// (add) Token: 0x06000049 RID: 73 RVA: 0x0000450F File Offset: 0x0000350F
		// (remove) Token: 0x0600004A RID: 74 RVA: 0x00004522 File Offset: 0x00003522
		public event DownloadFileGroupCompletedEventHandler DownloadFileGroupCompleted
		{
			add
			{
				this.Events.AddHandler(ApplicationDeployment.downloadFileGroupCompletedKey, value);
			}
			remove
			{
				this.Events.RemoveHandler(ApplicationDeployment.downloadFileGroupCompletedKey, value);
			}
		}

		// Token: 0x1700000E RID: 14
		// (get) Token: 0x0600004B RID: 75 RVA: 0x00004535 File Offset: 0x00003535
		private EventHandlerList Events
		{
			get
			{
				return this._events;
			}
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00004540 File Offset: 0x00003540
		private DeploymentManager CreateDeploymentManager()
		{
			this._subState.Invalidate();
			return new DeploymentManager(this._subState.DeploymentProviderUri, true, true, null, this.asyncOperation)
			{
				Callertype = DeploymentManager.CallerType.ApplicationDeployment
			};
		}

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600004D RID: 77 RVA: 0x0000457C File Offset: 0x0000357C
		private DeploymentManager SyncGroupDeploymentManager
		{
			get
			{
				if (this._syncGroupDeploymentManager == null)
				{
					DeploymentManager deploymentManager = null;
					bool flag = false;
					try
					{
						deploymentManager = new DeploymentManager(this._fullAppId.ToString(), true, true, null, this.asyncOperation);
						deploymentManager.Callertype = DeploymentManager.CallerType.ApplicationDeployment;
						deploymentManager.Bind();
						flag = Interlocked.CompareExchange(ref this._syncGroupDeploymentManager, deploymentManager, null) == null;
					}
					finally
					{
						if (!flag && deploymentManager != null)
						{
							deploymentManager.Dispose();
						}
					}
					if (flag)
					{
						deploymentManager.ProgressChanged += this.DownloadFileGroupProgressChangedEventHandler;
						deploymentManager.SynchronizeCompleted += this.SynchronizeGroupCompletedEventHandler;
					}
				}
				return (DeploymentManager)this._syncGroupDeploymentManager;
			}
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00004620 File Offset: 0x00003620
		private void CheckForUpdateProgressChangedEventHandler(object sender, DeploymentProgressChangedEventArgs e)
		{
			if (this._cancellationPending)
			{
				((DeploymentManager)sender).CancelAsync();
			}
			DeploymentProgressChangedEventHandler deploymentProgressChangedEventHandler = (DeploymentProgressChangedEventHandler)this.Events[ApplicationDeployment.checkForUpdateProgressChangedKey];
			if (deploymentProgressChangedEventHandler != null)
			{
				deploymentProgressChangedEventHandler(this, e);
			}
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00004664 File Offset: 0x00003664
		private void UpdateProgressChangedEventHandler(object sender, DeploymentProgressChangedEventArgs e)
		{
			if (this._cancellationPending)
			{
				((DeploymentManager)sender).CancelAsync();
			}
			DeploymentProgressChangedEventHandler deploymentProgressChangedEventHandler = (DeploymentProgressChangedEventHandler)this.Events[ApplicationDeployment.updateProgressChangedKey];
			if (deploymentProgressChangedEventHandler != null)
			{
				deploymentProgressChangedEventHandler(this, e);
			}
		}

		// Token: 0x06000050 RID: 80 RVA: 0x000046A8 File Offset: 0x000036A8
		private void DownloadFileGroupProgressChangedEventHandler(object sender, DeploymentProgressChangedEventArgs e)
		{
			DeploymentProgressChangedEventHandler deploymentProgressChangedEventHandler = (DeploymentProgressChangedEventHandler)this.Events[ApplicationDeployment.downloadFileGroupProgressChangedKey];
			if (deploymentProgressChangedEventHandler != null)
			{
				deploymentProgressChangedEventHandler(this, e);
			}
		}

		// Token: 0x06000051 RID: 81 RVA: 0x000046D8 File Offset: 0x000036D8
		private void CheckForUpdateBindCompletedEventHandler(object sender, BindCompletedEventArgs e)
		{
			Exception ex = null;
			DeploymentManager deploymentManager = null;
			bool flag = false;
			Version version = null;
			bool flag2 = false;
			Version version2 = null;
			long num = 0L;
			new NamedPermissionSet("FullTrust").Assert();
			try
			{
				deploymentManager = (DeploymentManager)sender;
				if (e.Error == null && !e.Cancelled)
				{
					deploymentManager.DetermineTrust(new TrustParams
					{
						NoPrompt = true
					});
					deploymentManager.DeterminePlatformRequirements();
					UpdateCheckInfo updateCheckInfo = this.DetermineUpdateCheckResult(deploymentManager.ActivationDescription);
					this.ProcessUpdateCheckResult(updateCheckInfo, deploymentManager.ActivationDescription);
					if (updateCheckInfo.UpdateAvailable)
					{
						flag = true;
						version = updateCheckInfo.AvailableVersion;
						flag2 = updateCheckInfo.IsUpdateRequired;
						version2 = updateCheckInfo.MinimumRequiredVersion;
						num = updateCheckInfo.UpdateSizeBytes;
					}
				}
				else
				{
					ex = e.Error;
				}
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
				Interlocked.Exchange(ref this._guard, 0);
				CheckForUpdateCompletedEventArgs checkForUpdateCompletedEventArgs = new CheckForUpdateCompletedEventArgs(ex, e.Cancelled, null, flag, version, flag2, version2, num);
				CheckForUpdateCompletedEventHandler checkForUpdateCompletedEventHandler = (CheckForUpdateCompletedEventHandler)this.Events[ApplicationDeployment.checkForUpdateCompletedKey];
				if (checkForUpdateCompletedEventHandler != null)
				{
					checkForUpdateCompletedEventHandler(this, checkForUpdateCompletedEventArgs);
				}
				if (deploymentManager != null)
				{
					deploymentManager.ProgressChanged -= this.CheckForUpdateProgressChangedEventHandler;
					deploymentManager.BindCompleted -= this.CheckForUpdateBindCompletedEventHandler;
					new NamedPermissionSet("FullTrust").Assert();
					try
					{
						deploymentManager.Dispose();
					}
					finally
					{
						CodeAccessPermission.RevertAssert();
					}
				}
			}
		}

		// Token: 0x06000052 RID: 82 RVA: 0x00004858 File Offset: 0x00003858
		private void UpdateBindCompletedEventHandler(object sender, BindCompletedEventArgs e)
		{
			Exception ex = null;
			DeploymentManager deploymentManager = null;
			bool flag = false;
			new NamedPermissionSet("FullTrust").Assert();
			try
			{
				deploymentManager = (DeploymentManager)sender;
				if (e.Error == null && !e.Cancelled)
				{
					deploymentManager.DetermineTrust(new TrustParams
					{
						NoPrompt = true
					});
					deploymentManager.DeterminePlatformRequirements();
					UpdateCheckInfo updateCheckInfo = this.DetermineUpdateCheckResult(deploymentManager.ActivationDescription);
					this.ProcessUpdateCheckResult(updateCheckInfo, deploymentManager.ActivationDescription);
					if (updateCheckInfo.UpdateAvailable)
					{
						flag = true;
						deploymentManager.SynchronizeAsync();
					}
				}
				else
				{
					ex = e.Error;
				}
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
				if (!flag)
				{
					this.EndUpdateAsync(deploymentManager, ex, e.Cancelled);
				}
			}
		}

		// Token: 0x06000053 RID: 83 RVA: 0x00004920 File Offset: 0x00003920
		private void EndUpdateAsync(DeploymentManager dm, Exception error, bool cancelled)
		{
			Interlocked.Exchange(ref this._guard, 0);
			AsyncCompletedEventArgs asyncCompletedEventArgs = new AsyncCompletedEventArgs(error, cancelled, null);
			AsyncCompletedEventHandler asyncCompletedEventHandler = (AsyncCompletedEventHandler)this.Events[ApplicationDeployment.updateCompletedKey];
			if (asyncCompletedEventHandler != null)
			{
				asyncCompletedEventHandler(this, asyncCompletedEventArgs);
			}
			if (dm != null)
			{
				dm.ProgressChanged -= this.UpdateProgressChangedEventHandler;
				dm.BindCompleted -= this.UpdateBindCompletedEventHandler;
				dm.SynchronizeCompleted -= this.SynchronizeNullCompletedEventHandler;
				new NamedPermissionSet("FullTrust").Assert();
				try
				{
					dm.Dispose();
				}
				finally
				{
					CodeAccessPermission.RevertAssert();
				}
			}
		}

		// Token: 0x06000054 RID: 84 RVA: 0x000049CC File Offset: 0x000039CC
		private void SynchronizeNullCompletedEventHandler(object sender, SynchronizeCompletedEventArgs e)
		{
			Exception ex = null;
			DeploymentManager deploymentManager = null;
			new NamedPermissionSet("FullTrust").Assert();
			try
			{
				deploymentManager = (DeploymentManager)sender;
				ex = e.Error;
			}
			catch (Exception ex2)
			{
				ex = ex2;
			}
			finally
			{
				CodeAccessPermission.RevertAssert();
				this.EndUpdateAsync(deploymentManager, ex, e.Cancelled);
			}
		}

		// Token: 0x06000055 RID: 85 RVA: 0x00004A34 File Offset: 0x00003A34
		private void SynchronizeGroupCompletedEventHandler(object sender, SynchronizeCompletedEventArgs e)
		{
			try
			{
				DeploymentManager deploymentManager = (DeploymentManager)sender;
				Exception error = e.Error;
			}
			catch (Exception ex)
			{
			}
			finally
			{
				DownloadFileGroupCompletedEventArgs downloadFileGroupCompletedEventArgs = new DownloadFileGroupCompletedEventArgs(e.Error, e.Cancelled, e.UserState, e.Group);
				DownloadFileGroupCompletedEventHandler downloadFileGroupCompletedEventHandler = (DownloadFileGroupCompletedEventHandler)this.Events[ApplicationDeployment.downloadFileGroupCompletedKey];
				if (downloadFileGroupCompletedEventHandler != null)
				{
					downloadFileGroupCompletedEventHandler(this, downloadFileGroupCompletedEventArgs);
				}
			}
		}

		// Token: 0x06000056 RID: 86 RVA: 0x00004AB4 File Offset: 0x00003AB4
		private UpdateCheckInfo DetermineUpdateCheckResult(ActivationDescription actDesc)
		{
			bool flag = false;
			Version version = null;
			bool flag2 = false;
			Version version2 = null;
			long num = 0L;
			AssemblyManifest deployManifest = actDesc.DeployManifest;
			this._subState.Invalidate();
			Version version3 = this._subStore.CheckUpdateInManifest(this._subState, actDesc.DeploySourceUri, deployManifest, this._currentVersion);
			if (version3 != null && !deployManifest.Identity.Equals(this._subState.ExcludedDeployment))
			{
				flag = true;
				version = version3;
				version2 = deployManifest.Deployment.MinimumRequiredVersion;
				if (version2 != null && version2.CompareTo(this._currentVersion) > 0)
				{
					flag2 = true;
				}
				ulong num2 = actDesc.AppManifest.CalculateDependenciesSize();
				if (num2 > 9223372036854775807UL)
				{
					num = long.MaxValue;
				}
				else
				{
					num = (long)num2;
				}
			}
			return new UpdateCheckInfo(flag, version, flag2, version2, num);
		}

		// Token: 0x06000057 RID: 87 RVA: 0x00004B88 File Offset: 0x00003B88
		private void ProcessUpdateCheckResult(UpdateCheckInfo info, ActivationDescription actDesc)
		{
			if (!this._subState.IsShellVisible)
			{
				return;
			}
			AssemblyManifest deployManifest = actDesc.DeployManifest;
			DefinitionIdentity definitionIdentity = (info.UpdateAvailable ? deployManifest.Identity : null);
			this._subStore.SetPendingDeployment(this._subState, definitionIdentity, DateTime.UtcNow);
		}

		// Token: 0x06000058 RID: 88 RVA: 0x00004BD3 File Offset: 0x00003BD3
		private void DemandPermission()
		{
			this.accessPermission.Demand();
		}

		// Token: 0x04000028 RID: 40
		private const int guardInitial = 0;

		// Token: 0x04000029 RID: 41
		private const int guardAsync = 1;

		// Token: 0x0400002A RID: 42
		private const int guardSync = 2;

		// Token: 0x0400002B RID: 43
		private static readonly object checkForUpdateCompletedKey = new object();

		// Token: 0x0400002C RID: 44
		private static readonly object updateCompletedKey = new object();

		// Token: 0x0400002D RID: 45
		private static readonly object downloadFileGroupCompletedKey = new object();

		// Token: 0x0400002E RID: 46
		private static readonly object checkForUpdateProgressChangedKey = new object();

		// Token: 0x0400002F RID: 47
		private static readonly object updateProgressChangedKey = new object();

		// Token: 0x04000030 RID: 48
		private static readonly object downloadFileGroupProgressChangedKey = new object();

		// Token: 0x04000031 RID: 49
		private static readonly object lockObject = new object();

		// Token: 0x04000032 RID: 50
		private static ApplicationDeployment _currentDeployment = null;

		// Token: 0x04000033 RID: 51
		private readonly AsyncOperation asyncOperation;

		// Token: 0x04000034 RID: 52
		private readonly CodeAccessPermission accessPermission;

		// Token: 0x04000035 RID: 53
		private int _guard;

		// Token: 0x04000036 RID: 54
		private bool _cancellationPending;

		// Token: 0x04000037 RID: 55
		private SubscriptionStore _subStore;

		// Token: 0x04000038 RID: 56
		private EventHandlerList _events;

		// Token: 0x04000039 RID: 57
		private DefinitionAppId _fullAppId;

		// Token: 0x0400003A RID: 58
		private Version _currentVersion;

		// Token: 0x0400003B RID: 59
		private SubscriptionState _subState;

		// Token: 0x0400003C RID: 60
		private object _syncGroupDeploymentManager;
	}
}
