using System;
using System.Collections;
using System.Deployment.Application.Manifest;
using System.Deployment.Application.Win32InterOp;
using System.Deployment.Internal.Isolation;
using System.Deployment.Internal.Isolation.Manifest;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;

namespace System.Deployment.Application
{
	// Token: 0x020000CA RID: 202
	internal class SubscriptionStore
	{
		// Token: 0x1700013F RID: 319
		// (get) Token: 0x06000528 RID: 1320 RVA: 0x0001B270 File Offset: 0x0001A270
		public static SubscriptionStore CurrentUser
		{
			get
			{
				if (SubscriptionStore._userStore == null)
				{
					lock (SubscriptionStore._currentUserLock)
					{
						if (SubscriptionStore._userStore == null)
						{
							string folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
							string text = Path.Combine(folderPath, "Deployment");
							string text2 = Path.Combine(Path.GetTempPath(), "Deployment");
							SubscriptionStore._userStore = new SubscriptionStore(text, text2, ComponentStoreType.UserStore);
						}
					}
				}
				return SubscriptionStore._userStore;
			}
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001B2E8 File Offset: 0x0001A2E8
		private SubscriptionStore(string deployPath, string tempPath, ComponentStoreType storeType)
		{
			this._deployPath = deployPath;
			this._tempPath = tempPath;
			Directory.CreateDirectory(this._deployPath);
			Directory.CreateDirectory(this._tempPath);
			using (this.AcquireStoreWriterLock())
			{
				this._compStore = ComponentStore.GetStore(storeType, this);
			}
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001B354 File Offset: 0x0001A354
		public void RefreshStorePointer()
		{
			using (this.AcquireStoreWriterLock())
			{
				this._compStore.RefreshStorePointer();
			}
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001B390 File Offset: 0x0001A390
		public void CleanOnlineAppCache()
		{
			using (this.AcquireStoreWriterLock())
			{
				this._compStore.RefreshStorePointer();
				this._compStore.CleanOnlineAppCache();
			}
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001B3D8 File Offset: 0x0001A3D8
		public void CommitApplication(ref SubscriptionState subState, CommitApplicationParams commitParams)
		{
			using (this.AcquireSubscriptionWriterLock(subState))
			{
				if (commitParams.CommitDeploy)
				{
					UriHelper.ValidateSupportedScheme(commitParams.DeploySourceUri);
					this.CheckDeploymentSubscriptionState(subState, commitParams.DeployManifest);
					this.ValidateFileAssoctiation(subState, commitParams);
					if (commitParams.IsUpdate)
					{
						SubscriptionStore.CheckInstalled(subState);
					}
				}
				if (commitParams.CommitApp)
				{
					UriHelper.ValidateSupportedScheme(commitParams.AppSourceUri);
					if (commitParams.AppGroup != null)
					{
						SubscriptionStore.CheckInstalled(subState);
					}
					this.CheckApplicationPayload(commitParams);
				}
				bool flag = false;
				bool flag2 = false;
				bool flag3 = false;
				string text = "";
				ArrayList arrayList = this._compStore.CollectCrossGroupApplications(commitParams.DeploySourceUri, commitParams.DeployManifest.Identity, ref flag2, ref flag3, ref text);
				if (arrayList.Count > 0)
				{
					flag = true;
				}
				if (subState.IsShellVisible && flag2 && flag3)
				{
					throw new DeploymentException(ExceptionTypes.GroupMultipleMatch, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_GroupMultipleMatch"), new object[] { text }));
				}
				subState = this.GetSubscriptionState(commitParams.DeployManifest);
				this._compStore.CommitApplication(subState, commitParams);
				if (flag)
				{
					IActContext actContext = IsolationInterop.CreateActContext(subState.CurrentBind.ComPointer);
					actContext.PrepareForExecution(IntPtr.Zero, IntPtr.Zero);
					uint num;
					actContext.SetApplicationRunningState(0U, 1U, out num);
					actContext.SetApplicationRunningState(0U, 2U, out num);
					foreach (object obj in arrayList)
					{
						ComponentStore.CrossGroupApplicationData crossGroupApplicationData = (ComponentStore.CrossGroupApplicationData)obj;
						if (crossGroupApplicationData.CrossGroupType == ComponentStore.CrossGroupApplicationData.GroupType.LocationGroup)
						{
							if (crossGroupApplicationData.SubState.IsShellVisible)
							{
								this.UninstallSubscription(crossGroupApplicationData.SubState);
							}
							else if (crossGroupApplicationData.SubState.appType == AppType.CustomHostSpecified)
							{
								this.UninstallCustomHostSpecifiedSubscription(crossGroupApplicationData.SubState);
							}
							else if (crossGroupApplicationData.SubState.appType == AppType.CustomUX)
							{
								this.UninstallCustomUXSubscription(crossGroupApplicationData.SubState);
							}
						}
						else
						{
							ComponentStore.CrossGroupApplicationData.GroupType crossGroupType = crossGroupApplicationData.CrossGroupType;
						}
					}
				}
				if (commitParams.IsConfirmed && subState.IsInstalled && subState.IsShellVisible && commitParams.appType != AppType.CustomUX)
				{
					this.UpdateSubscriptionExposure(subState);
				}
				if (commitParams.appType == AppType.CustomUX)
				{
					ShellExposure.ShellExposureInformation shellExposureInformation = ShellExposure.ShellExposureInformation.CreateShellExposureInformation(subState.SubscriptionId);
					ShellExposure.UpdateShellExtensions(subState, ref shellExposureInformation);
				}
				SubscriptionStore.OnDeploymentAdded(subState);
			}
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001B660 File Offset: 0x0001A660
		public void RollbackSubscription(SubscriptionState subState)
		{
			using (this.AcquireSubscriptionWriterLock(subState))
			{
				this.CheckInstalledAndShellVisible(subState);
				if (subState.RollbackDeployment == null)
				{
					throw new DeploymentException(ExceptionTypes.SubscriptionState, Resources.GetString("Ex_SubNoRollbackDeployment"));
				}
				if (subState.CurrentApplicationManifest != null)
				{
					string text = null;
					if (subState.CurrentDeploymentManifest != null && subState.CurrentDeploymentManifest.Description != null)
					{
						text = subState.CurrentDeploymentManifest.Description.Product;
					}
					if (text == null)
					{
						text = subState.SubscriptionId.Name;
					}
					ShellExposure.RemoveShellExtensions(subState.SubscriptionId, subState.CurrentApplicationManifest, text);
				}
				this._compStore.RollbackSubscription(subState);
				this.UpdateSubscriptionExposure(subState);
				SubscriptionStore.OnDeploymentRemoved(subState);
			}
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0001B71C File Offset: 0x0001A71C
		public void UninstallSubscription(SubscriptionState subState)
		{
			using (this.AcquireSubscriptionWriterLock(subState))
			{
				this.CheckInstalledAndShellVisible(subState);
				if (subState.CurrentApplicationManifest != null)
				{
					string text = null;
					if (subState.CurrentDeploymentManifest != null && subState.CurrentDeploymentManifest.Description != null)
					{
						text = subState.CurrentDeploymentManifest.Description.Product;
					}
					if (text == null)
					{
						text = subState.SubscriptionId.Name;
					}
					ShellExposure.RemoveShellExtensions(subState.SubscriptionId, subState.CurrentApplicationManifest, text);
					ShellExposure.RemovePins(subState);
				}
				this._compStore.RemoveSubscription(subState);
				SubscriptionStore.RemoveSubscriptionExposure(subState);
				SubscriptionStore.OnDeploymentRemoved(subState);
			}
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001B7C4 File Offset: 0x0001A7C4
		public void UninstallCustomUXSubscription(SubscriptionState subState)
		{
			using (this.AcquireSubscriptionWriterLock(subState))
			{
				SubscriptionStore.CheckInstalled(subState);
				if (subState.appType != AppType.CustomUX)
				{
					throw new InvalidOperationException(Resources.GetString("Ex_CannotCallUninstallCustomUXApplication"));
				}
				if (subState.CurrentApplicationManifest != null)
				{
					string text = null;
					if (subState.CurrentDeploymentManifest != null && subState.CurrentDeploymentManifest.Description != null)
					{
						text = subState.CurrentDeploymentManifest.Description.Product;
					}
					if (text == null)
					{
						text = subState.SubscriptionId.Name;
					}
					ShellExposure.RemoveShellExtensions(subState.SubscriptionId, subState.CurrentApplicationManifest, text);
				}
				this._compStore.RemoveSubscription(subState);
				SubscriptionStore.OnDeploymentRemoved(subState);
			}
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0001B878 File Offset: 0x0001A878
		public void UninstallCustomHostSpecifiedSubscription(SubscriptionState subState)
		{
			using (this.AcquireSubscriptionWriterLock(subState))
			{
				SubscriptionStore.CheckInstalled(subState);
				if (subState.appType != AppType.CustomHostSpecified)
				{
					throw new InvalidOperationException(Resources.GetString("Ex_CannotCallUninstallCustomAddIn"));
				}
				this._compStore.RemoveSubscription(subState);
				SubscriptionStore.OnDeploymentRemoved(subState);
			}
		}

		// Token: 0x06000531 RID: 1329 RVA: 0x0001B8DC File Offset: 0x0001A8DC
		public void SetPendingDeployment(SubscriptionState subState, DefinitionIdentity deployId, DateTime checkTime)
		{
			using (this.AcquireSubscriptionWriterLock(subState))
			{
				this.CheckInstalledAndShellVisible(subState);
				this._compStore.SetPendingDeployment(subState, deployId, checkTime);
			}
		}

		// Token: 0x06000532 RID: 1330 RVA: 0x0001B924 File Offset: 0x0001A924
		public void SetLastCheckTimeToNow(SubscriptionState subState)
		{
			using (this.AcquireSubscriptionWriterLock(subState))
			{
				SubscriptionStore.CheckInstalled(subState);
				this._compStore.SetPendingDeployment(subState, null, DateTime.UtcNow);
			}
		}

		// Token: 0x06000533 RID: 1331 RVA: 0x0001B970 File Offset: 0x0001A970
		public void SetUpdateSkipTime(SubscriptionState subState, DefinitionIdentity updateSkippedDeployment, DateTime updateSkipTime)
		{
			using (this.AcquireSubscriptionWriterLock(subState))
			{
				this.CheckInstalledAndShellVisible(subState);
				this._compStore.SetUpdateSkipTime(subState, updateSkippedDeployment, updateSkipTime);
			}
		}

		// Token: 0x06000534 RID: 1332 RVA: 0x0001B9B8 File Offset: 0x0001A9B8
		public bool CheckAndReferenceApplication(SubscriptionState subState, DefinitionAppId appId, long transactionId)
		{
			DefinitionIdentity deploymentIdentity = appId.DeploymentIdentity;
			DefinitionIdentity applicationIdentity = appId.ApplicationIdentity;
			if (!subState.IsInstalled || !this.IsAssemblyInstalled(deploymentIdentity))
			{
				return false;
			}
			if (this.IsAssemblyInstalled(applicationIdentity))
			{
				return appId.Equals(subState.CurrentBind) || appId.Equals(subState.PreviousBind);
			}
			throw new DeploymentException(ExceptionTypes.Subscription, Resources.GetString("Ex_IllegalApplicationId"));
		}

		// Token: 0x06000535 RID: 1333 RVA: 0x0001BA1C File Offset: 0x0001AA1C
		public void ActivateApplication(DefinitionAppId appId, string activationParameter, bool useActivationParameter)
		{
			using (this.AcquireStoreReaderLock())
			{
				this._compStore.ActivateApplication(appId, activationParameter, useActivationParameter);
			}
		}

		// Token: 0x06000536 RID: 1334 RVA: 0x0001BA5C File Offset: 0x0001AA5C
		public FileStream AcquireReferenceTransaction(out long transactionId)
		{
			transactionId = 0L;
			return null;
		}

		// Token: 0x06000537 RID: 1335 RVA: 0x0001BA63 File Offset: 0x0001AA63
		public SubscriptionState GetSubscriptionState(DefinitionIdentity subId)
		{
			return new SubscriptionState(this, subId);
		}

		// Token: 0x06000538 RID: 1336 RVA: 0x0001BA6C File Offset: 0x0001AA6C
		public SubscriptionState GetSubscriptionState(AssemblyManifest deployment)
		{
			return new SubscriptionState(this, deployment);
		}

		// Token: 0x06000539 RID: 1337 RVA: 0x0001BA78 File Offset: 0x0001AA78
		public SubscriptionStateInternal GetSubscriptionStateInternal(SubscriptionState subState)
		{
			SubscriptionStateInternal subscriptionStateInternal;
			using (this.AcquireSubscriptionReaderLock(subState))
			{
				subscriptionStateInternal = this._compStore.GetSubscriptionStateInternal(subState);
			}
			return subscriptionStateInternal;
		}

		// Token: 0x0600053A RID: 1338 RVA: 0x0001BAB8 File Offset: 0x0001AAB8
		public void CheckForDeploymentUpdate(SubscriptionState subState)
		{
			this.CheckInstalledAndShellVisible(subState);
			Uri deploymentProviderUri = subState.DeploymentProviderUri;
			TempFile tempFile = null;
			try
			{
				AssemblyManifest assemblyManifest = DownloadManager.DownloadDeploymentManifest(subState.SubscriptionStore, ref deploymentProviderUri, out tempFile);
				Version version = this.CheckUpdateInManifest(subState, deploymentProviderUri, assemblyManifest, subState.CurrentDeployment.Version);
				DefinitionIdentity definitionIdentity = ((version != null) ? assemblyManifest.Identity : null);
				this.SetPendingDeployment(subState, definitionIdentity, DateTime.UtcNow);
				if (version != null && assemblyManifest.Identity.Equals(subState.PendingDeployment))
				{
					Logger.AddPhaseInformation(Resources.GetString("Upd_FoundUpdate"), new object[]
					{
						subState.SubscriptionId.ToString(),
						assemblyManifest.Identity.Version.ToString(),
						deploymentProviderUri.AbsoluteUri
					});
				}
			}
			finally
			{
				if (tempFile != null)
				{
					tempFile.Dispose();
				}
			}
		}

		// Token: 0x0600053B RID: 1339 RVA: 0x0001BB9C File Offset: 0x0001AB9C
		public Version CheckUpdateInManifest(SubscriptionState subState, Uri updateCodebaseUri, AssemblyManifest deployment, Version currentVersion)
		{
			SubscriptionStore.CheckOnlineShellVisibleConflict(subState, deployment);
			SubscriptionStore.CheckInstalledAndUpdateableConflict(subState, deployment);
			SubscriptionStore.CheckMinimumRequiredVersion(subState, deployment);
			SubscriptionState subscriptionState = this.GetSubscriptionState(deployment);
			if (!subscriptionState.SubscriptionId.Equals(subState.SubscriptionId) && (!updateCodebaseUri.Equals(subState.DeploymentProviderUri) || !subState.PKTGroupId.Equals(subscriptionState.PKTGroupId)))
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, Resources.GetString("Ex_DeploymentIdentityNotInSubscription"));
			}
			Version version = deployment.Identity.Version;
			if (version.CompareTo(currentVersion) == 0)
			{
				return null;
			}
			return version;
		}

		// Token: 0x0600053C RID: 1340 RVA: 0x0001BC24 File Offset: 0x0001AC24
		public void CheckDeploymentSubscriptionState(SubscriptionState subState, AssemblyManifest deployment)
		{
			if (subState.IsInstalled)
			{
				SubscriptionStore.CheckOnlineShellVisibleConflict(subState, deployment);
				SubscriptionStore.CheckInstalledAndUpdateableConflict(subState, deployment);
				SubscriptionStore.CheckMinimumRequiredVersion(subState, deployment);
			}
		}

		// Token: 0x0600053D RID: 1341 RVA: 0x0001BC44 File Offset: 0x0001AC44
		public void CheckCustomUXFlag(SubscriptionState subState, AssemblyManifest application)
		{
			if (subState.IsInstalled)
			{
				if (application.EntryPoints[0].CustomUX && subState.appType != AppType.CustomUX)
				{
					throw new DeploymentException(Resources.GetString("Ex_CustomUXAlready"));
				}
				if (!application.EntryPoints[0].CustomUX && subState.appType == AppType.CustomUX)
				{
					throw new DeploymentException(Resources.GetString("Ex_NotCustomUXAlready"));
				}
			}
		}

		// Token: 0x0600053E RID: 1342 RVA: 0x0001BCAC File Offset: 0x0001ACAC
		public void ValidateFileAssoctiation(SubscriptionState subState, CommitApplicationParams commitParams)
		{
			if (commitParams.DeployManifest != null && commitParams.AppManifest != null && !commitParams.DeployManifest.Deployment.Install && commitParams.AppManifest.FileAssociations.Length > 0)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, Resources.GetString("Ex_OnlineAppWithFileAssociation"));
			}
		}

		// Token: 0x0600053F RID: 1343 RVA: 0x0001BCFC File Offset: 0x0001ACFC
		public void CheckInstalledAndShellVisible(SubscriptionState subState)
		{
			SubscriptionStore.CheckInstalled(subState);
			SubscriptionStore.CheckShellVisible(subState);
		}

		// Token: 0x06000540 RID: 1344 RVA: 0x0001BD0A File Offset: 0x0001AD0A
		public static void CheckInstalled(SubscriptionState subState)
		{
			if (!subState.IsInstalled)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, Resources.GetString("Ex_SubNotInstalled"));
			}
		}

		// Token: 0x06000541 RID: 1345 RVA: 0x0001BD25 File Offset: 0x0001AD25
		public static void CheckShellVisible(SubscriptionState subState)
		{
			if (!subState.IsShellVisible)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, Resources.GetString("Ex_SubNotShellVisible"));
			}
		}

		// Token: 0x06000542 RID: 1346 RVA: 0x0001BD40 File Offset: 0x0001AD40
		public bool CheckGroupInstalled(SubscriptionState subState, DefinitionAppId appId, string groupName)
		{
			bool flag;
			using (this.AcquireSubscriptionReaderLock(subState))
			{
				flag = this._compStore.CheckGroupInstalled(appId, groupName);
			}
			return flag;
		}

		// Token: 0x06000543 RID: 1347 RVA: 0x0001BD80 File Offset: 0x0001AD80
		public bool CheckGroupInstalled(SubscriptionState subState, DefinitionAppId appId, AssemblyManifest appManifest, string groupName)
		{
			bool flag;
			using (this.AcquireSubscriptionReaderLock(subState))
			{
				flag = this._compStore.CheckGroupInstalled(appId, appManifest, groupName);
			}
			return flag;
		}

		// Token: 0x06000544 RID: 1348 RVA: 0x0001BDC4 File Offset: 0x0001ADC4
		public IDisposable AcquireSubscriptionReaderLock(SubscriptionState subState)
		{
			subState.Invalidate();
			return this.AcquireStoreReaderLock();
		}

		// Token: 0x06000545 RID: 1349 RVA: 0x0001BDD2 File Offset: 0x0001ADD2
		public IDisposable AcquireSubscriptionWriterLock(SubscriptionState subState)
		{
			subState.Invalidate();
			return this.AcquireStoreWriterLock();
		}

		// Token: 0x06000546 RID: 1350 RVA: 0x0001BDE0 File Offset: 0x0001ADE0
		public IDisposable AcquireStoreReaderLock()
		{
			return this.AcquireLock(this.SubscriptionStoreLock, false);
		}

		// Token: 0x06000547 RID: 1351 RVA: 0x0001BDEF File Offset: 0x0001ADEF
		public IDisposable AcquireStoreWriterLock()
		{
			return this.AcquireLock(this.SubscriptionStoreLock, true);
		}

		// Token: 0x06000548 RID: 1352 RVA: 0x0001BDFE File Offset: 0x0001ADFE
		public TempDirectory AcquireTempDirectory()
		{
			return new TempDirectory(this._tempPath);
		}

		// Token: 0x06000549 RID: 1353 RVA: 0x0001BE0B File Offset: 0x0001AE0B
		public TempFile AcquireTempFile(string suffix)
		{
			return new TempFile(this._tempPath, suffix);
		}

		// Token: 0x0600054A RID: 1354 RVA: 0x0001BE1C File Offset: 0x0001AE1C
		internal ulong GetPrivateSize(DefinitionAppId appId)
		{
			ArrayList arrayList = new ArrayList();
			arrayList.Add(appId);
			ulong privateSize;
			using (this.AcquireStoreReaderLock())
			{
				privateSize = this._compStore.GetPrivateSize(arrayList);
			}
			return privateSize;
		}

		// Token: 0x0600054B RID: 1355 RVA: 0x0001BE68 File Offset: 0x0001AE68
		internal ulong GetSharedSize(DefinitionAppId appId)
		{
			ArrayList arrayList = new ArrayList();
			arrayList.Add(appId);
			ulong sharedSize;
			using (this.AcquireStoreReaderLock())
			{
				sharedSize = this._compStore.GetSharedSize(arrayList);
			}
			return sharedSize;
		}

		// Token: 0x0600054C RID: 1356 RVA: 0x0001BEB4 File Offset: 0x0001AEB4
		internal ulong GetOnlineAppQuotaInBytes()
		{
			return this._compStore.GetOnlineAppQuotaInBytes();
		}

		// Token: 0x0600054D RID: 1357 RVA: 0x0001BEC4 File Offset: 0x0001AEC4
		internal ulong GetSizeLimitInBytesForSemiTrustApps()
		{
			ulong onlineAppQuotaInBytes = this.GetOnlineAppQuotaInBytes();
			return onlineAppQuotaInBytes / 2UL;
		}

		// Token: 0x0600054E RID: 1358 RVA: 0x0001BEDC File Offset: 0x0001AEDC
		internal Store.IPathLock LockApplicationPath(DefinitionAppId definitionAppId)
		{
			Store.IPathLock pathLock;
			using (this.AcquireStoreReaderLock())
			{
				pathLock = this._compStore.LockApplicationPath(definitionAppId);
			}
			return pathLock;
		}

		// Token: 0x0600054F RID: 1359 RVA: 0x0001BF1C File Offset: 0x0001AF1C
		private static void CheckOnlineShellVisibleConflict(SubscriptionState subState, AssemblyManifest deployment)
		{
			if (!deployment.Deployment.Install && subState.IsShellVisible)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, Resources.GetString("Ex_OnlineAlreadyShellVisible"));
			}
		}

		// Token: 0x06000550 RID: 1360 RVA: 0x0001BF44 File Offset: 0x0001AF44
		private static void CheckInstalledAndUpdateableConflict(SubscriptionState subState, AssemblyManifest deployment)
		{
		}

		// Token: 0x06000551 RID: 1361 RVA: 0x0001BF48 File Offset: 0x0001AF48
		private static void CheckMinimumRequiredVersion(SubscriptionState subState, AssemblyManifest deployment)
		{
			if (subState.MinimumRequiredVersion != null)
			{
				if (deployment.Identity.Version < subState.MinimumRequiredVersion)
				{
					throw new DeploymentException(ExceptionTypes.SubscriptionState, Resources.GetString("Ex_DeploymentBelowMinimumRequiredVersion"));
				}
				if (deployment.Deployment.MinimumRequiredVersion != null && deployment.Deployment.MinimumRequiredVersion < subState.MinimumRequiredVersion)
				{
					throw new DeploymentException(ExceptionTypes.SubscriptionState, Resources.GetString("Ex_DecreasingMinimumRequiredVersion"));
				}
			}
		}

		// Token: 0x06000552 RID: 1362 RVA: 0x0001BFC8 File Offset: 0x0001AFC8
		private void CheckApplicationPayload(CommitApplicationParams commitParams)
		{
			if (commitParams.AppGroup == null && commitParams.appType != AppType.CustomHostSpecified)
			{
				string text = Path.Combine(commitParams.AppPayloadPath, commitParams.AppManifest.EntryPoints[0].CommandFile);
				SystemUtils.CheckSupportedImageAndCLRVersions(text);
			}
			string text2 = null;
			Store.IPathLock pathLock = null;
			try
			{
				pathLock = this._compStore.LockAssemblyPath(commitParams.AppManifest.Identity);
				text2 = Path.GetDirectoryName(pathLock.Path);
				text2 = Path.Combine(text2, "manifests");
				text2 = Path.Combine(text2, Path.GetFileName(pathLock.Path) + ".manifest");
			}
			catch (DeploymentException)
			{
			}
			catch (COMException)
			{
			}
			finally
			{
				if (pathLock != null)
				{
					pathLock.Dispose();
				}
			}
			if (!string.IsNullOrEmpty(text2) && global::System.IO.File.Exists(text2) && !string.IsNullOrEmpty(commitParams.AppManifestPath) && global::System.IO.File.Exists(commitParams.AppManifestPath))
			{
				byte[] array = ComponentVerifier.GenerateDigestValue(text2, CMS_HASH_DIGESTMETHOD.CMS_HASH_DIGESTMETHOD_SHA1, CMS_HASH_TRANSFORM.CMS_HASH_TRANSFORM_IDENTITY);
				byte[] array2 = ComponentVerifier.GenerateDigestValue(commitParams.AppManifestPath, CMS_HASH_DIGESTMETHOD.CMS_HASH_DIGESTMETHOD_SHA1, CMS_HASH_TRANSFORM.CMS_HASH_TRANSFORM_IDENTITY);
				bool flag = false;
				if (array.Length == array2.Length)
				{
					int num = 0;
					while (num < array.Length && array[num] == array2[num])
					{
						num++;
					}
					if (num >= array.Length)
					{
						flag = true;
					}
				}
				if (!flag)
				{
					throw new DeploymentException(ExceptionTypes.Subscription, Resources.GetString("Ex_ApplicationInplaceUpdate"));
				}
			}
		}

		// Token: 0x06000553 RID: 1363 RVA: 0x0001C124 File Offset: 0x0001B124
		private void UpdateSubscriptionExposure(SubscriptionState subState)
		{
			this.CheckInstalledAndShellVisible(subState);
			ShellExposure.UpdateSubscriptionShellExposure(subState);
		}

		// Token: 0x06000554 RID: 1364 RVA: 0x0001C133 File Offset: 0x0001B133
		private static void RemoveSubscriptionExposure(SubscriptionState subState)
		{
			ShellExposure.RemoveSubscriptionShellExposure(subState);
		}

		// Token: 0x06000555 RID: 1365 RVA: 0x0001C13C File Offset: 0x0001B13C
		private bool IsAssemblyInstalled(DefinitionIdentity asmId)
		{
			bool flag;
			using (this.AcquireStoreReaderLock())
			{
				flag = this._compStore.IsAssemblyInstalled(asmId);
			}
			return flag;
		}

		// Token: 0x06000556 RID: 1366 RVA: 0x0001C17C File Offset: 0x0001B17C
		private IDisposable AcquireLock(DefinitionIdentity asmId, bool writer)
		{
			string keyForm = asmId.KeyForm;
			Directory.CreateDirectory(this._deployPath);
			return LockedFile.AcquireLock(Path.Combine(this._deployPath, keyForm), Constants.LockTimeout, writer);
		}

		// Token: 0x17000140 RID: 320
		// (get) Token: 0x06000557 RID: 1367 RVA: 0x0001C1B3 File Offset: 0x0001B1B3
		private DefinitionIdentity SubscriptionStoreLock
		{
			get
			{
				if (this._subscriptionStoreLock == null)
				{
					Interlocked.CompareExchange(ref this._subscriptionStoreLock, new DefinitionIdentity("__SubscriptionStoreLock__"), null);
				}
				return (DefinitionIdentity)this._subscriptionStoreLock;
			}
		}

		// Token: 0x06000558 RID: 1368 RVA: 0x0001C1DF File Offset: 0x0001B1DF
		private static void OnDeploymentAdded(SubscriptionState subState)
		{
		}

		// Token: 0x06000559 RID: 1369 RVA: 0x0001C1E1 File Offset: 0x0001B1E1
		private static void OnDeploymentRemoved(SubscriptionState subState)
		{
		}

		// Token: 0x0400046A RID: 1130
		private static SubscriptionStore _userStore;

		// Token: 0x0400046B RID: 1131
		private string _deployPath;

		// Token: 0x0400046C RID: 1132
		private string _tempPath;

		// Token: 0x0400046D RID: 1133
		private ComponentStore _compStore;

		// Token: 0x0400046E RID: 1134
		private object _subscriptionStoreLock;

		// Token: 0x0400046F RID: 1135
		private static object _currentUserLock = new object();
	}
}
