using System;
using System.Collections;
using System.Deployment.Application.Manifest;
using System.Deployment.Internal.Isolation;
using System.Deployment.Internal.Isolation.Manifest;
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using Microsoft.Internal.Performance;
using Microsoft.Win32;

namespace System.Deployment.Application
{
	// Token: 0x02000025 RID: 37
	internal class ComponentStore
	{
		// Token: 0x0600013B RID: 315 RVA: 0x00008844 File Offset: 0x00007844
		public static ComponentStore GetStore(ComponentStoreType storeType, SubscriptionStore subStore)
		{
			return new ComponentStore(storeType, subStore);
		}

		// Token: 0x0600013C RID: 316 RVA: 0x00008850 File Offset: 0x00007850
		private void RemoveInvalidCDFMSFiles()
		{
			RegistryKey registryKey = null;
			RegistryKey registryKey2 = null;
			RegistryKey registryKey3 = null;
			bool flag = false;
			try
			{
				registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\Software\\Microsoft\\Windows\\CurrentVersion\\Deployment", true);
				if (registryKey != null)
				{
					registryKey2 = registryKey.OpenSubKey("ClickOnce35SP1Update");
					registryKey3 = registryKey.OpenSubKey("SideBySide\\2.0");
					if (registryKey2 == null && registryKey3 != null)
					{
						string text = registryKey3.GetValue("ComponentStore_RandomString").ToString();
						string text2 = text.Substring(0, 8) + "." + text.Substring(8, 3);
						string text3 = text.Substring(11, 8) + "." + text.Substring(19, 3);
						string text4 = string.Empty;
						DirectoryInfo directoryInfo = new DirectoryInfo(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
						ArrayList arrayList = new ArrayList();
						foreach (object obj in new ArrayList { directoryInfo.Parent, directoryInfo })
						{
							DirectoryInfo directoryInfo2 = (DirectoryInfo)obj;
							foreach (DirectoryInfo directoryInfo3 in directoryInfo2.GetDirectories("Apps"))
							{
								foreach (DirectoryInfo directoryInfo4 in directoryInfo3.GetDirectories("2.0"))
								{
									foreach (DirectoryInfo directoryInfo5 in directoryInfo4.GetDirectories(text2))
									{
										foreach (DirectoryInfo directoryInfo6 in directoryInfo5.GetDirectories(text3))
										{
											arrayList.AddRange(this.CleanCDFMSFilesInDirectory(directoryInfo6));
											text4 = text4 + directoryInfo6.FullName + "\n";
										}
									}
								}
							}
						}
						foreach (object obj2 in arrayList)
						{
							FileInfo fileInfo = (FileInfo)obj2;
							if (fileInfo.Exists)
							{
								fileInfo.Delete();
							}
						}
						registryKey2 = registryKey.CreateSubKey("ClickOnce35SP1Update");
						if (registryKey2 != null)
						{
							registryKey2.SetValue("Action", "Purged CDF-MS Data");
							registryKey2.SetValue("AppData", directoryInfo.FullName.ToString());
							registryKey2.SetValue("Hits", text4);
						}
						flag = true;
					}
				}
				if (!flag)
				{
					if (registryKey == null)
					{
						registryKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\Software\\Microsoft\\Windows\\CurrentVersion\\Deployment");
					}
					if (registryKey != null && registryKey2 == null)
					{
						registryKey2 = registryKey.CreateSubKey("ClickOnce35SP1Update");
						if (registryKey2 != null)
						{
							registryKey2.SetValue("Action", "No cleanup required");
						}
					}
				}
			}
			catch (Exception)
			{
			}
			finally
			{
				if (registryKey3 != null)
				{
					registryKey3.Close();
				}
				if (registryKey2 != null)
				{
					registryKey2.Close();
				}
				if (registryKey != null)
				{
					registryKey.Close();
				}
			}
		}

		// Token: 0x0600013D RID: 317 RVA: 0x00008B84 File Offset: 0x00007B84
		private ArrayList CleanCDFMSFilesInDirectory(DirectoryInfo Folder)
		{
			ArrayList arrayList = new ArrayList();
			foreach (FileInfo fileInfo in Folder.GetFiles("*.cdf-ms", SearchOption.AllDirectories))
			{
				try
				{
					fileInfo.Delete();
				}
				catch (Exception)
				{
				}
				finally
				{
					if (fileInfo.Exists)
					{
						arrayList.Add(fileInfo);
					}
				}
			}
			return arrayList;
		}

		// Token: 0x0600013E RID: 318 RVA: 0x00008BF4 File Offset: 0x00007BF4
		private ComponentStore(ComponentStoreType storeType, SubscriptionStore subStore)
		{
			if (storeType == ComponentStoreType.UserStore)
			{
				this._storeType = storeType;
				this._subStore = subStore;
				this.RemoveInvalidCDFMSFiles();
				this._store = IsolationInterop.GetUserStore();
				Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IStateManager));
				this._stateMgr = IsolationInterop.GetUserStateManager(0U, IntPtr.Zero, ref guidOfType) as IStateManager;
				this._firstRefresh = true;
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x0600013F RID: 319 RVA: 0x00008C60 File Offset: 0x00007C60
		internal ulong GetOnlineAppQuotaInBytes()
		{
			uint num = 256000U;
			using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\Software\\Microsoft\\Windows\\CurrentVersion\\Deployment"))
			{
				if (registryKey != null)
				{
					object value = registryKey.GetValue("OnlineAppQuotaInKB");
					if (value is int)
					{
						int num2 = (int)value;
						num = (uint)((num2 >= 0) ? num2 : (-1 - -num2 + 1));
					}
				}
			}
			return (ulong)num * 1024UL;
		}

		// Token: 0x06000140 RID: 320 RVA: 0x00008CD4 File Offset: 0x00007CD4
		internal ulong GetPrivateSize(ArrayList deployAppIds)
		{
			ulong num;
			ulong num2;
			this.GetPrivateAndSharedSizes(deployAppIds, out num, out num2);
			return num;
		}

		// Token: 0x06000141 RID: 321 RVA: 0x00008CF0 File Offset: 0x00007CF0
		internal ulong GetSharedSize(ArrayList deployAppIds)
		{
			ulong num;
			ulong num2;
			this.GetPrivateAndSharedSizes(deployAppIds, out num, out num2);
			return num2;
		}

		// Token: 0x06000142 RID: 322 RVA: 0x00008D0C File Offset: 0x00007D0C
		internal ArrayList CollectCrossGroupApplications(Uri codebaseUri, DefinitionIdentity deploymentIdentity, ref bool identityGroupFound, ref bool locationGroupFound, ref string identityGroupProductName)
		{
			Hashtable hashtable = new Hashtable();
			ArrayList arrayList = new ArrayList();
			StoreAssemblyEnumeration storeAssemblyEnumeration = this._store.EnumAssemblies(Store.EnumAssembliesFlags.Nothing);
			foreach (object obj in storeAssemblyEnumeration)
			{
				DefinitionIdentity definitionIdentity = new DefinitionIdentity(((STORE_ASSEMBLY)obj).DefinitionIdentity);
				DefinitionIdentity definitionIdentity2 = definitionIdentity.ToSubscriptionId();
				SubscriptionState subscriptionState = this._subStore.GetSubscriptionState(definitionIdentity2);
				if (subscriptionState.IsInstalled)
				{
					bool flag = subscriptionState.DeploymentProviderUri.Equals(codebaseUri);
					bool flag2 = subscriptionState.PKTGroupId.Equals(deploymentIdentity.ToPKTGroupId());
					bool flag3 = subscriptionState.SubscriptionId.PublicKeyToken.Equals(deploymentIdentity.ToSubscriptionId().PublicKeyToken);
					if (!flag || !flag2 || !flag3)
					{
						if (flag && flag2 && !flag3)
						{
							if (!hashtable.Contains(definitionIdentity2))
							{
								hashtable.Add(definitionIdentity2, subscriptionState);
								arrayList.Add(new ComponentStore.CrossGroupApplicationData(subscriptionState, ComponentStore.CrossGroupApplicationData.GroupType.LocationGroup));
								locationGroupFound = true;
							}
						}
						else if (!flag && flag2 && flag3 && !hashtable.Contains(definitionIdentity2))
						{
							hashtable.Add(definitionIdentity2, subscriptionState);
							arrayList.Add(new ComponentStore.CrossGroupApplicationData(subscriptionState, ComponentStore.CrossGroupApplicationData.GroupType.IdentityGroup));
							if (subscriptionState.CurrentDeploymentManifest != null && subscriptionState.CurrentDeploymentManifest.Description != null && subscriptionState.CurrentDeploymentManifest.Description.Product != null)
							{
								identityGroupProductName = subscriptionState.CurrentDeploymentManifest.Description.Product;
							}
							identityGroupFound = true;
						}
					}
				}
			}
			return arrayList;
		}

		// Token: 0x06000143 RID: 323 RVA: 0x00008EB8 File Offset: 0x00007EB8
		internal void RemoveApplicationInstance(SubscriptionState subState, DefinitionAppId appId)
		{
			using (ComponentStore.StoreTransactionContext storeTransactionContext = new ComponentStore.StoreTransactionContext(this))
			{
				this.PrepareRemoveDeployment(storeTransactionContext, subState, appId);
				this.SubmitStoreTransaction(storeTransactionContext, subState);
			}
		}

		// Token: 0x06000144 RID: 324 RVA: 0x00008EFC File Offset: 0x00007EFC
		private void GetPrivateAndSharedSizes(ArrayList deployAppIds, out ulong privateSize, out ulong sharedSize)
		{
			privateSize = 0UL;
			sharedSize = 0UL;
			if (deployAppIds != null && deployAppIds.Count > 0)
			{
				IDefinitionAppId[] array = ComponentStore.DeployAppIdsToComPtrs(deployAppIds);
				this.CalculateDeploymentsUnderQuota(array.Length, array, ulong.MaxValue, ref privateSize, ref sharedSize);
			}
		}

		// Token: 0x06000145 RID: 325 RVA: 0x00008F34 File Offset: 0x00007F34
		private int CalculateDeploymentsUnderQuota(int numberOfDeployments, IDefinitionAppId[] deployAppIdPtrs, ulong quotaSize, ref ulong privateSize, ref ulong sharedSize)
		{
			uint num = 0U;
			StoreApplicationReference installReference = this.InstallReference;
			this._store.CalculateDelimiterOfDeploymentsBasedOnQuota(0U, (uint)numberOfDeployments, deployAppIdPtrs, ref installReference, quotaSize, ref num, ref sharedSize, ref privateSize);
			return (int)num;
		}

		// Token: 0x06000146 RID: 326 RVA: 0x00008F64 File Offset: 0x00007F64
		private static IDefinitionAppId[] DeployAppIdsToComPtrs(ArrayList deployAppIdList)
		{
			IDefinitionAppId[] array = new IDefinitionAppId[deployAppIdList.Count];
			for (int i = 0; i < deployAppIdList.Count; i++)
			{
				array[i] = ((DefinitionAppId)deployAppIdList[i]).ComPointer;
			}
			return array;
		}

		// Token: 0x06000147 RID: 327 RVA: 0x00008FA4 File Offset: 0x00007FA4
		public void RefreshStorePointer()
		{
			if (this._firstRefresh)
			{
				this._firstRefresh = false;
				return;
			}
			if (this._storeType == ComponentStoreType.UserStore)
			{
				Marshal.ReleaseComObject(this._store.InternalStore);
				Marshal.ReleaseComObject(this._stateMgr);
				this.RemoveInvalidCDFMSFiles();
				this._store = IsolationInterop.GetUserStore();
				Guid guidOfType = IsolationInterop.GetGuidOfType(typeof(IStateManager));
				this._stateMgr = IsolationInterop.GetUserStateManager(0U, IntPtr.Zero, ref guidOfType) as IStateManager;
				return;
			}
			throw new NotImplementedException();
		}

		// Token: 0x06000148 RID: 328 RVA: 0x00009028 File Offset: 0x00008028
		public void CleanOnlineAppCache()
		{
			using (ComponentStore.StoreTransactionContext storeTransactionContext = new ComponentStore.StoreTransactionContext(this))
			{
				storeTransactionContext.ScavengeContext.CleanOnlineAppCache();
			}
		}

		// Token: 0x06000149 RID: 329 RVA: 0x00009064 File Offset: 0x00008064
		public void CommitApplication(SubscriptionState subState, CommitApplicationParams commitParams)
		{
			try
			{
				using (ComponentStore.StoreTransactionContext storeTransactionContext = new ComponentStore.StoreTransactionContext(this))
				{
					this.PrepareCommitApplication(storeTransactionContext, subState, commitParams);
					this.SubmitStoreTransactionCheckQuota(storeTransactionContext, subState);
				}
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147024784)
				{
					throw new DeploymentException(ExceptionTypes.DiskIsFull, Resources.GetString("Ex_StoreOperationFailed"), ex);
				}
				if (ex.ErrorCode == -2147023590)
				{
					throw new DeploymentException(ExceptionTypes.ComponentStore, Resources.GetString("Ex_InplaceUpdateOfApplicationAttempted"), ex);
				}
				throw;
			}
		}

		// Token: 0x0600014A RID: 330 RVA: 0x000090F8 File Offset: 0x000080F8
		public void RemoveSubscription(SubscriptionState subState)
		{
			try
			{
				using (ComponentStore.StoreTransactionContext storeTransactionContext = new ComponentStore.StoreTransactionContext(this))
				{
					this.PrepareRemoveSubscription(storeTransactionContext, subState);
					this.SubmitStoreTransactionCheckQuota(storeTransactionContext, subState);
				}
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147024784)
				{
					throw new DeploymentException(ExceptionTypes.DiskIsFull, Resources.GetString("Ex_StoreOperationFailed"), ex);
				}
				throw;
			}
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0000916C File Offset: 0x0000816C
		public void RollbackSubscription(SubscriptionState subState)
		{
			try
			{
				using (ComponentStore.StoreTransactionContext storeTransactionContext = new ComponentStore.StoreTransactionContext(this))
				{
					this.PrepareRollbackSubscription(storeTransactionContext, subState);
					this.SubmitStoreTransactionCheckQuota(storeTransactionContext, subState);
				}
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147024784)
				{
					throw new DeploymentException(ExceptionTypes.DiskIsFull, Resources.GetString("Ex_StoreOperationFailed"), ex);
				}
				throw;
			}
		}

		// Token: 0x0600014C RID: 332 RVA: 0x000091E0 File Offset: 0x000081E0
		public void SetPendingDeployment(SubscriptionState subState, DefinitionIdentity deployId, DateTime checkTime)
		{
			try
			{
				using (ComponentStore.StoreTransactionContext storeTransactionContext = new ComponentStore.StoreTransactionContext(this))
				{
					this.PrepareSetPendingDeployment(storeTransactionContext, subState, deployId, checkTime);
					this.SubmitStoreTransaction(storeTransactionContext, subState);
				}
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147024784)
				{
					throw new DeploymentException(ExceptionTypes.DiskIsFull, Resources.GetString("Ex_StoreOperationFailed"), ex);
				}
				throw;
			}
		}

		// Token: 0x0600014D RID: 333 RVA: 0x00009254 File Offset: 0x00008254
		public void SetUpdateSkipTime(SubscriptionState subState, DefinitionIdentity updateSkippedDeployment, DateTime updateSkipTime)
		{
			try
			{
				using (ComponentStore.StoreTransactionContext storeTransactionContext = new ComponentStore.StoreTransactionContext(this))
				{
					this.PrepareUpdateSkipTime(storeTransactionContext, subState, updateSkippedDeployment, updateSkipTime);
					this.SubmitStoreTransaction(storeTransactionContext, subState);
				}
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147024784)
				{
					throw new DeploymentException(ExceptionTypes.DiskIsFull, Resources.GetString("Ex_StoreOperationFailed"), ex);
				}
				throw;
			}
		}

		// Token: 0x0600014E RID: 334 RVA: 0x000092C8 File Offset: 0x000082C8
		public SubscriptionStateInternal GetSubscriptionStateInternal(SubscriptionState subState)
		{
			return this.GetSubscriptionStateInternal(subState.SubscriptionId);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x000092D8 File Offset: 0x000082D8
		public SubscriptionStateInternal GetSubscriptionStateInternal(DefinitionIdentity subId)
		{
			SubscriptionStateInternal subscriptionStateInternal = new SubscriptionStateInternal();
			subscriptionStateInternal.IsInstalled = this.IsSubscriptionInstalled(subId);
			if (subscriptionStateInternal.IsInstalled)
			{
				DefinitionAppId definitionAppId = new DefinitionAppId(new DefinitionIdentity[] { subId });
				subscriptionStateInternal.IsShellVisible = this.GetPropertyBoolean(definitionAppId, "IsShellVisible");
				subscriptionStateInternal.CurrentBind = this.GetPropertyDefinitionAppId(definitionAppId, "CurrentBind");
				subscriptionStateInternal.PreviousBind = this.GetPropertyDefinitionAppId(definitionAppId, "PreviousBind");
				subscriptionStateInternal.PendingBind = this.GetPropertyDefinitionAppId(definitionAppId, "PendingBind");
				subscriptionStateInternal.ExcludedDeployment = this.GetPropertyDefinitionIdentity(definitionAppId, "ExcludedDeployment");
				subscriptionStateInternal.PendingDeployment = this.GetPropertyDefinitionIdentity(definitionAppId, "PendingDeployment");
				subscriptionStateInternal.DeploymentProviderUri = this.GetPropertyUri(definitionAppId, "DeploymentProviderUri");
				subscriptionStateInternal.MinimumRequiredVersion = this.GetPropertyVersion(definitionAppId, "MinimumRequiredVersion");
				subscriptionStateInternal.LastCheckTime = this.GetPropertyDateTime(definitionAppId, "LastCheckTime");
				subscriptionStateInternal.UpdateSkippedDeployment = this.GetPropertyDefinitionIdentity(definitionAppId, "UpdateSkippedDeployment");
				subscriptionStateInternal.UpdateSkipTime = this.GetPropertyDateTime(definitionAppId, "UpdateSkipTime");
				subscriptionStateInternal.appType = this.GetPropertyAppType(definitionAppId, "AppType");
				if (subscriptionStateInternal.CurrentBind == null)
				{
					throw new InvalidDeploymentException(Resources.GetString("Ex_NoCurrentBind"));
				}
				subscriptionStateInternal.CurrentDeployment = subscriptionStateInternal.CurrentBind.DeploymentIdentity;
				subscriptionStateInternal.CurrentDeploymentManifest = this.GetAssemblyManifest(subscriptionStateInternal.CurrentDeployment);
				subscriptionStateInternal.CurrentDeploymentSourceUri = this.GetPropertyUri(subscriptionStateInternal.CurrentBind, "DeploymentSourceUri");
				subscriptionStateInternal.CurrentApplication = subscriptionStateInternal.CurrentBind.ApplicationIdentity;
				subscriptionStateInternal.CurrentApplicationManifest = this.GetAssemblyManifest(subscriptionStateInternal.CurrentBind.ApplicationIdentity);
				subscriptionStateInternal.CurrentApplicationSourceUri = this.GetPropertyUri(subscriptionStateInternal.CurrentBind, "ApplicationSourceUri");
				DefinitionIdentity definitionIdentity = ((subscriptionStateInternal.PreviousBind != null) ? subscriptionStateInternal.PreviousBind.DeploymentIdentity : null);
				subscriptionStateInternal.RollbackDeployment = ((definitionIdentity != null && (subscriptionStateInternal.MinimumRequiredVersion == null || definitionIdentity.Version >= subscriptionStateInternal.MinimumRequiredVersion)) ? definitionIdentity : null);
				if (subscriptionStateInternal.PreviousBind != null)
				{
					subscriptionStateInternal.PreviousApplication = subscriptionStateInternal.PreviousBind.ApplicationIdentity;
					subscriptionStateInternal.PreviousApplicationManifest = this.GetAssemblyManifest(subscriptionStateInternal.PreviousBind.ApplicationIdentity);
				}
			}
			return subscriptionStateInternal;
		}

		// Token: 0x06000150 RID: 336 RVA: 0x000094F4 File Offset: 0x000084F4
		public void ActivateApplication(DefinitionAppId appId, string activationParameter, bool useActivationParameter)
		{
			ComponentStore.HostType hostType = this.GetHostTypeFromMetadata(appId);
			PolicyKeys.HostType hostType2 = PolicyKeys.ClrHostType();
			if (hostType2 == PolicyKeys.HostType.AppLaunch)
			{
				hostType = ComponentStore.HostType.AppLaunch;
			}
			else if (hostType2 == PolicyKeys.HostType.Cor)
			{
				hostType = ComponentStore.HostType.CorFlag;
			}
			string text = appId.ToString();
			AssemblyManifest assemblyManifest = this.GetAssemblyManifest(appId.DeploymentIdentity);
			int num = 0;
			string[] array = null;
			if (activationParameter != null && (assemblyManifest.Deployment.TrustURLParameters || useActivationParameter))
			{
				num = 1;
				array = new string[] { activationParameter };
			}
			uint num2 = (uint)hostType;
			if (!assemblyManifest.Deployment.Install)
			{
				num2 |= 2147483648U;
			}
			try
			{
				NativeMethods.CorLaunchApplication(num2, text, 0, null, num, array, new NativeMethods.PROCESS_INFORMATION());
			}
			catch (COMException ex)
			{
				int num3 = ex.ErrorCode & 65535;
				if (num3 >= 14000 && num3 <= 14999)
				{
					throw new DeploymentException(ExceptionTypes.Activation, Resources.GetString("Ex_ActivationFailureDueToSxSError"), ex);
				}
				if (ex.ErrorCode == -2147024784)
				{
					throw new DeploymentException(ExceptionTypes.DiskIsFull, Resources.GetString("Ex_StoreOperationFailed"), ex);
				}
				throw;
			}
			catch (UnauthorizedAccessException ex2)
			{
				throw new DeploymentException(ExceptionTypes.Activation, Resources.GetString("Ex_GenericActivationFailure"), ex2);
			}
			catch (IOException ex3)
			{
				throw new DeploymentException(ExceptionTypes.Activation, Resources.GetString("Ex_GenericActivationFailure"), ex3);
			}
		}

		// Token: 0x06000151 RID: 337 RVA: 0x00009640 File Offset: 0x00008640
		public bool IsAssemblyInstalled(DefinitionIdentity asmId)
		{
			IDefinitionIdentity definitionIdentity = null;
			bool flag;
			try
			{
				definitionIdentity = this._store.GetAssemblyIdentity(0U, asmId.ComPointer);
				flag = true;
			}
			catch (COMException)
			{
				flag = false;
			}
			finally
			{
				if (definitionIdentity != null)
				{
					Marshal.ReleaseComObject(definitionIdentity);
				}
			}
			return flag;
		}

		// Token: 0x06000152 RID: 338 RVA: 0x00009694 File Offset: 0x00008694
		public Store.IPathLock LockApplicationPath(DefinitionAppId definitionAppId)
		{
			Store.IPathLock pathLock;
			try
			{
				pathLock = this._store.LockApplicationPath(definitionAppId.ComPointer);
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147024784)
				{
					throw new DeploymentException(ExceptionTypes.DiskIsFull, Resources.GetString("Ex_StoreOperationFailed"), ex);
				}
				throw;
			}
			return pathLock;
		}

		// Token: 0x06000153 RID: 339 RVA: 0x000096EC File Offset: 0x000086EC
		public Store.IPathLock LockAssemblyPath(DefinitionIdentity asmId)
		{
			Store.IPathLock pathLock;
			try
			{
				pathLock = this._store.LockAssemblyPath(asmId.ComPointer);
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode == -2147024784)
				{
					throw new DeploymentException(ExceptionTypes.DiskIsFull, Resources.GetString("Ex_StoreOperationFailed"), ex);
				}
				throw;
			}
			return pathLock;
		}

		// Token: 0x06000154 RID: 340 RVA: 0x00009744 File Offset: 0x00008744
		public bool CheckGroupInstalled(DefinitionAppId appId, string groupName)
		{
			AssemblyManifest assemblyManifest = this.GetAssemblyManifest(appId.ApplicationIdentity);
			return this.CheckGroupInstalled(appId, assemblyManifest, groupName);
		}

		// Token: 0x06000155 RID: 341 RVA: 0x00009768 File Offset: 0x00008768
		public bool CheckGroupInstalled(DefinitionAppId appId, AssemblyManifest appManifest, string groupName)
		{
			Store.IPathLock pathLock = null;
			try
			{
				pathLock = this.LockApplicationPath(appId);
				string path = pathLock.Path;
				global::System.Deployment.Application.Manifest.File[] filesInGroup = appManifest.GetFilesInGroup(groupName, true);
				foreach (global::System.Deployment.Application.Manifest.File file in filesInGroup)
				{
					string text = Path.Combine(path, file.NameFS);
					if (!global::System.IO.File.Exists(text))
					{
						return false;
					}
				}
				DependentAssembly[] privateAssembliesInGroup = appManifest.GetPrivateAssembliesInGroup(groupName, true);
				foreach (DependentAssembly dependentAssembly in privateAssembliesInGroup)
				{
					string text2 = Path.Combine(path, dependentAssembly.CodebaseFS);
					if (!global::System.IO.File.Exists(text2))
					{
						return false;
					}
				}
				if (filesInGroup.Length + privateAssembliesInGroup.Length == 0)
				{
					throw new InvalidDeploymentException(string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_NoSuchDownloadGroup"), new object[] { groupName }));
				}
			}
			finally
			{
				if (pathLock != null)
				{
					pathLock.Dispose();
				}
			}
			return true;
		}

		// Token: 0x06000156 RID: 342 RVA: 0x00009864 File Offset: 0x00008864
		private ComponentStore.HostType GetHostTypeFromMetadata(DefinitionAppId defAppId)
		{
			ComponentStore.HostType hostType = ComponentStore.HostType.Default;
			try
			{
				bool propertyBoolean = this.GetPropertyBoolean(defAppId, "IsFullTrust");
				if (propertyBoolean)
				{
					hostType = ComponentStore.HostType.CorFlag;
				}
				else
				{
					hostType = ComponentStore.HostType.AppLaunch;
				}
			}
			catch (DeploymentException)
			{
			}
			return hostType;
		}

		// Token: 0x06000157 RID: 343 RVA: 0x000098A0 File Offset: 0x000088A0
		private AssemblyManifest GetAssemblyManifest(DefinitionIdentity asmId)
		{
			ICMS assemblyManifest = this._store.GetAssemblyManifest(0U, asmId.ComPointer);
			return new AssemblyManifest(assemblyManifest);
		}

		// Token: 0x06000158 RID: 344 RVA: 0x000098C8 File Offset: 0x000088C8
		private bool IsSubscriptionInstalled(DefinitionIdentity subId)
		{
			DefinitionAppId definitionAppId = new DefinitionAppId(new DefinitionIdentity[] { subId });
			bool flag;
			try
			{
				DefinitionAppId propertyDefinitionAppId = this.GetPropertyDefinitionAppId(definitionAppId, "CurrentBind");
				flag = propertyDefinitionAppId != null;
			}
			catch (DeploymentException)
			{
				flag = false;
			}
			return flag;
		}

		// Token: 0x06000159 RID: 345 RVA: 0x00009914 File Offset: 0x00008914
		private string GetPropertyString(DefinitionAppId appId, string propName)
		{
			byte[] deploymentProperty;
			try
			{
				deploymentProperty = this._store.GetDeploymentProperty(Store.GetPackagePropertyFlags.Nothing, appId.ComPointer, this.InstallReference, Constants.DeploymentPropertySet, propName);
			}
			catch (COMException)
			{
				return null;
			}
			int num = deploymentProperty.Length;
			if (num == 0 || deploymentProperty.Length % 2 != 0 || deploymentProperty[num - 2] != 0 || deploymentProperty[num - 1] != 0)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_InvalidStoreMetaData"), new object[] { propName }));
			}
			return Encoding.Unicode.GetString(deploymentProperty, 0, num - 2);
		}

		// Token: 0x0600015A RID: 346 RVA: 0x000099AC File Offset: 0x000089AC
		private DefinitionIdentity GetPropertyDefinitionIdentity(DefinitionAppId appId, string propName)
		{
			DefinitionIdentity definitionIdentity;
			try
			{
				string propertyString = this.GetPropertyString(appId, propName);
				definitionIdentity = ((propertyString != null && propertyString.Length > 0) ? new DefinitionIdentity(propertyString) : null);
			}
			catch (COMException ex)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_InvalidStoreMetaData"), new object[] { propName }), ex);
			}
			catch (SEHException ex2)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_InvalidStoreMetaData"), new object[] { propName }), ex2);
			}
			return definitionIdentity;
		}

		// Token: 0x0600015B RID: 347 RVA: 0x00009A50 File Offset: 0x00008A50
		private DefinitionAppId GetPropertyDefinitionAppId(DefinitionAppId appId, string propName)
		{
			DefinitionAppId definitionAppId;
			try
			{
				string propertyString = this.GetPropertyString(appId, propName);
				definitionAppId = ((propertyString != null && propertyString.Length > 0) ? new DefinitionAppId(propertyString) : null);
			}
			catch (COMException ex)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_InvalidStoreMetaData"), new object[] { propName }), ex);
			}
			catch (SEHException ex2)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_InvalidStoreMetaData"), new object[] { propName }), ex2);
			}
			return definitionAppId;
		}

		// Token: 0x0600015C RID: 348 RVA: 0x00009AF4 File Offset: 0x00008AF4
		private bool GetPropertyBoolean(DefinitionAppId appId, string propName)
		{
			bool flag;
			try
			{
				string propertyString = this.GetPropertyString(appId, propName);
				flag = propertyString != null && propertyString.Length > 0 && Convert.ToBoolean(propertyString, CultureInfo.InvariantCulture);
			}
			catch (FormatException ex)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_InvalidStoreMetaData"), new object[] { propName }), ex);
			}
			return flag;
		}

		// Token: 0x0600015D RID: 349 RVA: 0x00009B64 File Offset: 0x00008B64
		private Uri GetPropertyUri(DefinitionAppId appId, string propName)
		{
			Uri uri;
			try
			{
				string propertyString = this.GetPropertyString(appId, propName);
				uri = ((propertyString != null && propertyString.Length > 0) ? new Uri(propertyString) : null);
			}
			catch (UriFormatException ex)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_InvalidStoreMetaData"), new object[] { propName }), ex);
			}
			return uri;
		}

		// Token: 0x0600015E RID: 350 RVA: 0x00009BCC File Offset: 0x00008BCC
		private Version GetPropertyVersion(DefinitionAppId appId, string propName)
		{
			Version version;
			try
			{
				string propertyString = this.GetPropertyString(appId, propName);
				version = ((propertyString != null && propertyString.Length > 0) ? new Version(propertyString) : null);
			}
			catch (ArgumentException ex)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_InvalidStoreMetaData"), new object[] { propName }), ex);
			}
			catch (FormatException ex2)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_InvalidStoreMetaData"), new object[] { propName }), ex2);
			}
			return version;
		}

		// Token: 0x0600015F RID: 351 RVA: 0x00009C70 File Offset: 0x00008C70
		private DateTime GetPropertyDateTime(DefinitionAppId appId, string propName)
		{
			DateTime dateTime;
			try
			{
				string propertyString = this.GetPropertyString(appId, propName);
				dateTime = ((propertyString != null && propertyString.Length > 0) ? DateTime.ParseExact(propertyString, "yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo) : DateTime.MinValue);
			}
			catch (FormatException ex)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_InvalidStoreMetaData"), new object[] { propName }), ex);
			}
			return dateTime;
		}

		// Token: 0x06000160 RID: 352 RVA: 0x00009CE8 File Offset: 0x00008CE8
		private AppType GetPropertyAppType(DefinitionAppId appId, string propName)
		{
			AppType appType;
			try
			{
				string propertyString = this.GetPropertyString(appId, propName);
				if (propertyString == null)
				{
					appType = AppType.None;
				}
				else
				{
					switch (Convert.ToUInt16(propertyString))
					{
					case 0:
						appType = AppType.None;
						break;
					case 1:
						appType = AppType.Installed;
						break;
					case 2:
						appType = AppType.Online;
						break;
					case 3:
						appType = AppType.CustomHostSpecified;
						break;
					case 4:
						appType = AppType.CustomUX;
						break;
					default:
						appType = AppType.None;
						break;
					}
				}
			}
			catch (DeploymentException)
			{
				appType = AppType.None;
			}
			catch (FormatException ex)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_InvalidStoreMetaData"), new object[] { propName }), ex);
			}
			catch (OverflowException ex2)
			{
				throw new DeploymentException(ExceptionTypes.SubscriptionState, string.Format(CultureInfo.CurrentUICulture, Resources.GetString("Ex_InvalidStoreMetaData"), new object[] { propName }), ex2);
			}
			return appType;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x00009DD8 File Offset: 0x00008DD8
		private void PrepareCommitApplication(ComponentStore.StoreTransactionContext storeTxn, SubscriptionState subState, CommitApplicationParams commitParams)
		{
			DefinitionAppId appId = commitParams.AppId;
			SubscriptionStateInternal subscriptionStateInternal = null;
			if (commitParams.CommitDeploy)
			{
				subscriptionStateInternal = this.PrepareCommitDeploymentState(storeTxn, subState, commitParams);
				if ((commitParams.IsConfirmed && appId.Equals(subscriptionStateInternal.CurrentBind)) || (!commitParams.IsConfirmed && appId.Equals(subscriptionStateInternal.PendingBind)))
				{
					this.PrepareStageDeploymentComponent(storeTxn, subState, commitParams);
				}
			}
			if (commitParams.CommitApp)
			{
				this.PrepareStageAppComponent(storeTxn, commitParams);
				if (!commitParams.DeployManifest.Deployment.Install && commitParams.appType != AppType.CustomHostSpecified)
				{
					storeTxn.ScavengeContext.AddOnlineAppToCommit(appId, subState);
				}
			}
			if (commitParams.CommitDeploy)
			{
				this.PrepareSetSubscriptionState(storeTxn, subState, subscriptionStateInternal);
			}
		}

		// Token: 0x06000162 RID: 354 RVA: 0x00009E80 File Offset: 0x00008E80
		private SubscriptionStateInternal PrepareCommitDeploymentState(ComponentStore.StoreTransactionContext storeTxn, SubscriptionState subState, CommitApplicationParams commitParams)
		{
			DefinitionAppId appId = commitParams.AppId;
			AssemblyManifest deployManifest = commitParams.DeployManifest;
			SubscriptionStateInternal subscriptionStateInternal = new SubscriptionStateInternal(subState);
			if (commitParams.IsConfirmed)
			{
				subscriptionStateInternal.IsInstalled = true;
				subscriptionStateInternal.IsShellVisible = deployManifest.Deployment.Install;
				subscriptionStateInternal.DeploymentProviderUri = ((deployManifest.Deployment.ProviderCodebaseUri != null) ? deployManifest.Deployment.ProviderCodebaseUri : commitParams.DeploySourceUri);
				if (deployManifest.Deployment.MinimumRequiredVersion != null)
				{
					subscriptionStateInternal.MinimumRequiredVersion = deployManifest.Deployment.MinimumRequiredVersion;
				}
				if (!appId.Equals(subState.CurrentBind))
				{
					subscriptionStateInternal.CurrentBind = appId;
					subscriptionStateInternal.PreviousBind = ((subscriptionStateInternal.IsShellVisible && !subState.IsShellVisible) ? null : subState.CurrentBind);
				}
				subscriptionStateInternal.PendingBind = null;
				subscriptionStateInternal.PendingDeployment = null;
				subscriptionStateInternal.ExcludedDeployment = null;
				subscriptionStateInternal.appType = commitParams.appType;
				ComponentStore.ResetUpdateSkippedState(subscriptionStateInternal);
			}
			else
			{
				subscriptionStateInternal.PendingBind = appId;
				subscriptionStateInternal.PendingDeployment = appId.DeploymentIdentity;
				if (!subscriptionStateInternal.PendingDeployment.Equals(subState.UpdateSkippedDeployment))
				{
					ComponentStore.ResetUpdateSkippedState(subscriptionStateInternal);
				}
			}
			subscriptionStateInternal.LastCheckTime = commitParams.TimeStamp;
			ComponentStore.FinalizeSubscriptionState(subscriptionStateInternal);
			return subscriptionStateInternal;
		}

		// Token: 0x06000163 RID: 355 RVA: 0x00009FB4 File Offset: 0x00008FB4
		private void PrepareStageDeploymentComponent(ComponentStore.StoreTransactionContext storeTxn, SubscriptionState subState, CommitApplicationParams commitParams)
		{
			DefinitionAppId definitionAppId = commitParams.AppId.ToDeploymentAppId();
			string deployManifestPath = commitParams.DeployManifestPath;
			storeTxn.Add(new StoreOperationStageComponent(definitionAppId.ComPointer, deployManifestPath));
			this.PrepareSetDeploymentProperties(storeTxn, commitParams.AppId, commitParams);
		}

		// Token: 0x06000164 RID: 356 RVA: 0x00009FF4 File Offset: 0x00008FF4
		private void PrepareSetDeploymentProperties(ComponentStore.StoreTransactionContext storeTxn, DefinitionAppId appId, CommitApplicationParams commitParams)
		{
			string text = null;
			string text2 = null;
			string text3 = null;
			if (commitParams != null)
			{
				text = ComponentStore.ToPropertyString(commitParams.DeploySourceUri);
				text2 = ComponentStore.ToPropertyString(commitParams.AppSourceUri);
				if (commitParams.IsUpdate && commitParams.Trust == null)
				{
					text3 = null;
				}
				else if (commitParams.appType == AppType.CustomHostSpecified)
				{
					text3 = null;
				}
				else
				{
					text3 = ComponentStore.ToPropertyString(commitParams.Trust.DefaultGrantSet.PermissionSet.IsUnrestricted());
				}
			}
			StoreOperationMetadataProperty[] array = new StoreOperationMetadataProperty[]
			{
				new StoreOperationMetadataProperty(Constants.DeploymentPropertySet, "DeploymentSourceUri", text),
				new StoreOperationMetadataProperty(Constants.DeploymentPropertySet, "ApplicationSourceUri", text2),
				new StoreOperationMetadataProperty(Constants.DeploymentPropertySet, "IsFullTrust", text3)
			};
			storeTxn.Add(new StoreOperationSetDeploymentMetadata(appId.ComPointer, this.InstallReference, array));
		}

		// Token: 0x06000165 RID: 357 RVA: 0x0000A0E0 File Offset: 0x000090E0
		private void PrepareStageAppComponent(ComponentStore.StoreTransactionContext storeTxn, CommitApplicationParams commitParams)
		{
			DefinitionAppId appId = commitParams.AppId;
			AssemblyManifest appManifest = commitParams.AppManifest;
			string appManifestPath = commitParams.AppManifestPath;
			string appPayloadPath = commitParams.AppPayloadPath;
			string appGroup = commitParams.AppGroup;
			if (appGroup == null)
			{
				if (appManifestPath == null)
				{
					throw new ArgumentNullException("commitParams");
				}
				storeTxn.Add(new StoreOperationStageComponent(appId.ComPointer, appManifestPath));
			}
			global::System.Deployment.Application.Manifest.File[] filesInGroup = appManifest.GetFilesInGroup(appGroup, true);
			foreach (global::System.Deployment.Application.Manifest.File file in filesInGroup)
			{
				this.PrepareInstallFile(storeTxn, file, appId, null, appPayloadPath);
			}
			DependentAssembly[] privateAssembliesInGroup = appManifest.GetPrivateAssembliesInGroup(appGroup, true);
			foreach (DependentAssembly dependentAssembly in privateAssembliesInGroup)
			{
				this.PrepareInstallPrivateAssembly(storeTxn, dependentAssembly, appId, appPayloadPath);
			}
		}

		// Token: 0x06000166 RID: 358 RVA: 0x0000A1A0 File Offset: 0x000091A0
		private void PrepareInstallFile(ComponentStore.StoreTransactionContext storeTxn, global::System.Deployment.Application.Manifest.File file, DefinitionAppId appId, DefinitionIdentity asmId, string asmPayloadPath)
		{
			string text = Path.Combine(asmPayloadPath, file.NameFS);
			string name = file.Name;
			storeTxn.Add(new StoreOperationStageComponentFile(appId.ComPointer, (asmId != null) ? asmId.ComPointer : null, name, text));
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0000A1E4 File Offset: 0x000091E4
		private void PrepareInstallPrivateAssembly(ComponentStore.StoreTransactionContext storeTxn, DependentAssembly privAsm, DefinitionAppId appId, string appPayloadPath)
		{
			string codebaseFS = privAsm.CodebaseFS;
			string text = Path.Combine(appPayloadPath, codebaseFS);
			string directoryName = Path.GetDirectoryName(text);
			AssemblyManifest assemblyManifest = new AssemblyManifest(text);
			DefinitionIdentity definitionIdentity = assemblyManifest.Identity;
			string text2 = assemblyManifest.RawXmlFilePath;
			if (text2 == null)
			{
				text2 = text + ".genman";
				definitionIdentity = ManifestGenerator.GenerateManifest(privAsm.Identity, assemblyManifest, text2);
			}
			storeTxn.Add(new StoreOperationStageComponent(appId.ComPointer, definitionIdentity.ComPointer, text2));
			foreach (global::System.Deployment.Application.Manifest.File file in assemblyManifest.Files)
			{
				this.PrepareInstallFile(storeTxn, file, appId, definitionIdentity, directoryName);
			}
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0000A28C File Offset: 0x0000928C
		private void PrepareRemoveSubscription(ComponentStore.StoreTransactionContext storeTxn, SubscriptionState subState)
		{
			this.PrepareFinalizeSubscriptionState(storeTxn, subState, new SubscriptionStateInternal(subState)
			{
				IsInstalled = false
			});
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0000A2B0 File Offset: 0x000092B0
		private void PrepareRollbackSubscription(ComponentStore.StoreTransactionContext storeTxn, SubscriptionState subState)
		{
			this.PrepareFinalizeSubscriptionState(storeTxn, subState, new SubscriptionStateInternal(subState)
			{
				ExcludedDeployment = subState.CurrentBind.DeploymentIdentity,
				CurrentBind = subState.PreviousBind,
				PreviousBind = null
			});
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0000A2F4 File Offset: 0x000092F4
		private void PrepareSetPendingDeployment(ComponentStore.StoreTransactionContext storeTxn, SubscriptionState subState, DefinitionIdentity deployId, DateTime checkTime)
		{
			SubscriptionStateInternal subscriptionStateInternal = new SubscriptionStateInternal(subState);
			subscriptionStateInternal.PendingDeployment = deployId;
			subscriptionStateInternal.LastCheckTime = checkTime;
			if (subscriptionStateInternal.PendingDeployment != null && !subscriptionStateInternal.PendingDeployment.Equals(subState.UpdateSkippedDeployment))
			{
				ComponentStore.ResetUpdateSkippedState(subscriptionStateInternal);
			}
			this.PrepareFinalizeSubscriptionState(storeTxn, subState, subscriptionStateInternal);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0000A344 File Offset: 0x00009344
		private void PrepareUpdateSkipTime(ComponentStore.StoreTransactionContext storeTxn, SubscriptionState subState, DefinitionIdentity updateSkippedDeployment, DateTime updateSkipTime)
		{
			this.PrepareFinalizeSubscriptionState(storeTxn, subState, new SubscriptionStateInternal(subState)
			{
				UpdateSkippedDeployment = updateSkippedDeployment,
				UpdateSkipTime = updateSkipTime
			});
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0000A370 File Offset: 0x00009370
		private void PrepareFinalizeSubscriptionState(ComponentStore.StoreTransactionContext storeTxn, SubscriptionState subState, SubscriptionStateInternal newState)
		{
			ComponentStore.FinalizeSubscriptionState(newState);
			this.PrepareSetSubscriptionState(storeTxn, subState, newState);
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0000A381 File Offset: 0x00009381
		private void PrepareSetSubscriptionState(ComponentStore.StoreTransactionContext storeTxn, SubscriptionState subState, SubscriptionStateInternal newState)
		{
			this.PrepareFinalizeSubscription(storeTxn, subState, newState);
			this.PrepareSetSubscriptionProperties(storeTxn, subState, newState);
			this.PrepareRemoveOrphanedDeployments(storeTxn, subState, newState);
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0000A3A0 File Offset: 0x000093A0
		private static void FinalizeSubscriptionState(SubscriptionStateInternal newState)
		{
			if (!newState.IsInstalled)
			{
				newState.Reset();
				return;
			}
			DefinitionAppId currentBind = newState.CurrentBind;
			DefinitionIdentity deploymentIdentity = currentBind.DeploymentIdentity;
			DefinitionAppId definitionAppId = newState.PreviousBind;
			if (definitionAppId != null && definitionAppId.Equals(currentBind))
			{
				definitionAppId = (newState.PreviousBind = null);
			}
			DefinitionIdentity definitionIdentity = ((definitionAppId != null) ? definitionAppId.DeploymentIdentity : null);
			DefinitionIdentity definitionIdentity2 = newState.ExcludedDeployment;
			if (definitionIdentity2 != null && (definitionIdentity2.Equals(deploymentIdentity) || definitionIdentity2.Equals(definitionIdentity)))
			{
				definitionIdentity2 = (newState.ExcludedDeployment = null);
			}
			DefinitionIdentity definitionIdentity3 = newState.PendingDeployment;
			if (definitionIdentity3 != null && (definitionIdentity3.Equals(deploymentIdentity) || definitionIdentity3.Equals(definitionIdentity2)))
			{
				definitionIdentity3 = (newState.PendingDeployment = null);
			}
			DefinitionAppId pendingBind = newState.PendingBind;
			if (pendingBind != null && (!pendingBind.DeploymentIdentity.Equals(definitionIdentity3) || pendingBind.Equals(definitionAppId)))
			{
				newState.PendingBind = null;
			}
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0000A478 File Offset: 0x00009478
		private static void ResetUpdateSkippedState(SubscriptionStateInternal newState)
		{
			newState.UpdateSkippedDeployment = null;
			newState.UpdateSkipTime = DateTime.MinValue;
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0000A48C File Offset: 0x0000948C
		private void PrepareSetSubscriptionProperties(ComponentStore.StoreTransactionContext storeTxn, SubscriptionState subState, SubscriptionStateInternal newState)
		{
			SubscriptionStateVariable[] array = new SubscriptionStateVariable[]
			{
				new SubscriptionStateVariable("IsShellVisible", newState.IsShellVisible, subState.IsShellVisible),
				new SubscriptionStateVariable("PreviousBind", newState.PreviousBind, subState.PreviousBind),
				new SubscriptionStateVariable("PendingBind", newState.PendingBind, subState.PendingBind),
				new SubscriptionStateVariable("ExcludedDeployment", newState.ExcludedDeployment, subState.ExcludedDeployment),
				new SubscriptionStateVariable("PendingDeployment", newState.PendingDeployment, subState.PendingDeployment),
				new SubscriptionStateVariable("DeploymentProviderUri", newState.DeploymentProviderUri, subState.DeploymentProviderUri),
				new SubscriptionStateVariable("MinimumRequiredVersion", newState.MinimumRequiredVersion, subState.MinimumRequiredVersion),
				new SubscriptionStateVariable("LastCheckTime", newState.LastCheckTime, subState.LastCheckTime),
				new SubscriptionStateVariable("UpdateSkippedDeployment", newState.UpdateSkippedDeployment, subState.UpdateSkippedDeployment),
				new SubscriptionStateVariable("UpdateSkipTime", newState.UpdateSkipTime, subState.UpdateSkipTime),
				new SubscriptionStateVariable("AppType", (ushort)newState.appType, (ushort)subState.appType),
				new SubscriptionStateVariable("CurrentBind", newState.CurrentBind, subState.CurrentBind)
			};
			ArrayList arrayList = new ArrayList();
			foreach (SubscriptionStateVariable subscriptionStateVariable in array)
			{
				if (!subState.IsInstalled || !subscriptionStateVariable.IsUnchanged || !newState.IsInstalled)
				{
					arrayList.Add(new StoreOperationMetadataProperty(Constants.DeploymentPropertySet, subscriptionStateVariable.PropertyName, newState.IsInstalled ? ComponentStore.ToPropertyString(subscriptionStateVariable.NewValue) : null));
				}
			}
			if (arrayList.Count > 0)
			{
				StoreOperationMetadataProperty[] array3 = (StoreOperationMetadataProperty[])arrayList.ToArray(typeof(StoreOperationMetadataProperty));
				DefinitionAppId definitionAppId = new DefinitionAppId(new DefinitionIdentity[] { subState.SubscriptionId });
				storeTxn.Add(new StoreOperationSetDeploymentMetadata(definitionAppId.ComPointer, this.InstallReference, array3));
			}
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0000A6CC File Offset: 0x000096CC
		private void PrepareRemoveOrphanedDeployments(ComponentStore.StoreTransactionContext storeTxn, SubscriptionState subState, SubscriptionStateInternal newState)
		{
			ArrayList arrayList = new ArrayList();
			arrayList.Add(subState.CurrentBind);
			arrayList.Add(subState.PreviousBind);
			arrayList.Add(subState.PendingBind);
			arrayList.Remove(newState.CurrentBind);
			arrayList.Remove(newState.PreviousBind);
			arrayList.Remove(newState.PendingBind);
			foreach (object obj in arrayList)
			{
				DefinitionAppId definitionAppId = (DefinitionAppId)obj;
				if (definitionAppId != null)
				{
					this.PrepareRemoveDeployment(storeTxn, subState, definitionAppId);
				}
			}
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0000A778 File Offset: 0x00009778
		private void PrepareRemoveDeployment(ComponentStore.StoreTransactionContext storeTxn, SubscriptionState subState, DefinitionAppId appId)
		{
			DefinitionAppId definitionAppId = appId.ToDeploymentAppId();
			if (subState.IsShellVisible)
			{
				this.PrepareInstallUninstallDeployment(storeTxn, definitionAppId, false);
			}
			else
			{
				this.PreparePinUnpinDeployment(storeTxn, definitionAppId, false);
			}
			this.PrepareSetDeploymentProperties(storeTxn, appId, null);
			storeTxn.ScavengeContext.AddDeploymentToUnpin(definitionAppId, subState);
			ApplicationTrust.RemoveCachedTrust(appId);
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0000A7C4 File Offset: 0x000097C4
		private void PrepareFinalizeSubscription(ComponentStore.StoreTransactionContext storeTxn, SubscriptionState subState, SubscriptionStateInternal newState)
		{
			if (newState.IsInstalled && (!subState.IsInstalled || newState.IsShellVisible != subState.IsShellVisible || !newState.CurrentBind.Equals(subState.CurrentBind)))
			{
				DefinitionAppId definitionAppId = newState.CurrentBind.ToDeploymentAppId();
				if (newState.IsShellVisible)
				{
					this.PrepareInstallUninstallDeployment(storeTxn, definitionAppId, true);
					return;
				}
				this.PreparePinUnpinDeployment(storeTxn, definitionAppId, true);
			}
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0000A829 File Offset: 0x00009829
		private void PreparePinUnpinDeployment(ComponentStore.StoreTransactionContext storeTxn, DefinitionAppId deployAppId, bool isPin)
		{
			if (isPin)
			{
				storeTxn.Add(new StoreOperationPinDeployment(deployAppId.ComPointer, this.InstallReference));
				return;
			}
			storeTxn.Add(new StoreOperationUnpinDeployment(deployAppId.ComPointer, this.InstallReference));
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0000A85D File Offset: 0x0000985D
		private void PrepareInstallUninstallDeployment(ComponentStore.StoreTransactionContext storeTxn, DefinitionAppId deployAppId, bool isInstall)
		{
			if (isInstall)
			{
				storeTxn.Add(new StoreOperationInstallDeployment(deployAppId.ComPointer, this.InstallReference));
				return;
			}
			storeTxn.Add(new StoreOperationUninstallDeployment(deployAppId.ComPointer, this.InstallReference));
		}

		// Token: 0x17000089 RID: 137
		// (get) Token: 0x06000176 RID: 374 RVA: 0x0000A891 File Offset: 0x00009891
		private StoreApplicationReference InstallReference
		{
			get
			{
				if (ComponentStore._installReference == null)
				{
					Interlocked.CompareExchange(ref ComponentStore._installReference, new StoreApplicationReference(IsolationInterop.GUID_SXS_INSTALL_REFERENCE_SCHEME_OPAQUESTRING, "{3f471841-eef2-47d6-89c0-d028f03a4ad5}", null), null);
				}
				return (StoreApplicationReference)ComponentStore._installReference;
			}
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0000A8C8 File Offset: 0x000098C8
		private void SubmitStoreTransaction(ComponentStore.StoreTransactionContext storeTxn, SubscriptionState subState)
		{
			CodeMarker_Singleton.Instance.CodeMarker(CodeMarkerEvent.perfPersisterWriteStart);
			storeTxn.Add(new StoreOperationScavenge(false));
			StoreTransactionOperation[] operations = storeTxn.Operations;
			if (operations.Length > 0)
			{
				uint[] array = new uint[operations.Length];
				int[] array2 = new int[operations.Length];
				try
				{
					this._store.Transact(operations, array, array2);
					uint num;
					this._stateMgr.Scavenge(0U, out num);
				}
				catch (DirectoryNotFoundException ex)
				{
					throw new DeploymentException(ExceptionTypes.ComponentStore, Resources.GetString("Ex_TransactDirectoryNotFoundException"), ex);
				}
				catch (ArgumentException ex2)
				{
					throw new DeploymentException(ExceptionTypes.ComponentStore, Resources.GetString("Ex_StoreOperationFailed"), ex2);
				}
				catch (UnauthorizedAccessException ex3)
				{
					throw new DeploymentException(ExceptionTypes.ComponentStore, Resources.GetString("Ex_StoreOperationFailed"), ex3);
				}
				catch (IOException ex4)
				{
					throw new DeploymentException(ExceptionTypes.ComponentStore, Resources.GetString("Ex_StoreOperationFailed"), ex4);
				}
				finally
				{
					CodeMarker_Singleton.Instance.CodeMarker(CodeMarkerEvent.perfPersisterWriteEnd);
					Logger.AddTransactionInformation(operations, array, array2);
				}
				if (subState != null)
				{
					subState.Invalidate();
				}
			}
		}

		// Token: 0x06000178 RID: 376 RVA: 0x0000A9E8 File Offset: 0x000099E8
		private void SubmitStoreTransactionCheckQuota(ComponentStore.StoreTransactionContext storeTxn, SubscriptionState subState)
		{
			storeTxn.ScavengeContext.CalculateSizesPreTransact();
			this.SubmitStoreTransaction(storeTxn, subState);
			storeTxn.ScavengeContext.CalculateSizesPostTransact();
			storeTxn.ScavengeContext.CheckQuotaAndScavenge();
		}

		// Token: 0x06000179 RID: 377 RVA: 0x0000AA14 File Offset: 0x00009A14
		private static string ToPropertyString(object propValue)
		{
			string text;
			if (propValue == null)
			{
				text = string.Empty;
			}
			else if (propValue is bool)
			{
				text = ((bool)propValue).ToString(CultureInfo.InvariantCulture);
			}
			else if (propValue is DateTime)
			{
				text = ((DateTime)propValue).ToString("yyyy/MM/dd HH:mm:ss", DateTimeFormatInfo.InvariantInfo);
			}
			else if (propValue is Uri)
			{
				text = ((Uri)propValue).AbsoluteUri;
			}
			else
			{
				text = propValue.ToString();
			}
			return text;
		}

		// Token: 0x040000CE RID: 206
		private const string DateTimeFormatString = "yyyy/MM/dd HH:mm:ss";

		// Token: 0x040000CF RID: 207
		private static object _installReference;

		// Token: 0x040000D0 RID: 208
		private ComponentStoreType _storeType;

		// Token: 0x040000D1 RID: 209
		private Store _store;

		// Token: 0x040000D2 RID: 210
		private IStateManager _stateMgr;

		// Token: 0x040000D3 RID: 211
		private SubscriptionStore _subStore;

		// Token: 0x040000D4 RID: 212
		private bool _firstRefresh;

		// Token: 0x02000026 RID: 38
		internal class CrossGroupApplicationData
		{
			// Token: 0x0600017A RID: 378 RVA: 0x0000AA8B File Offset: 0x00009A8B
			public CrossGroupApplicationData(SubscriptionState subState, ComponentStore.CrossGroupApplicationData.GroupType groupType)
			{
				this.SubState = subState;
				this.CrossGroupType = groupType;
			}

			// Token: 0x040000D5 RID: 213
			public SubscriptionState SubState;

			// Token: 0x040000D6 RID: 214
			public ComponentStore.CrossGroupApplicationData.GroupType CrossGroupType;

			// Token: 0x02000027 RID: 39
			public enum GroupType
			{
				// Token: 0x040000D8 RID: 216
				UndefinedGroup,
				// Token: 0x040000D9 RID: 217
				LocationGroup,
				// Token: 0x040000DA RID: 218
				IdentityGroup
			}
		}

		// Token: 0x02000028 RID: 40
		private enum HostType
		{
			// Token: 0x040000DC RID: 220
			Default,
			// Token: 0x040000DD RID: 221
			AppLaunch,
			// Token: 0x040000DE RID: 222
			CorFlag
		}

		// Token: 0x0200002A RID: 42
		private class StoreTransactionContext : StoreTransaction
		{
			// Token: 0x0600018A RID: 394 RVA: 0x0000AF36 File Offset: 0x00009F36
			public StoreTransactionContext(ComponentStore compStore)
			{
				this._compStore = compStore;
			}

			// Token: 0x1700008B RID: 139
			// (get) Token: 0x0600018B RID: 395 RVA: 0x0000AF45 File Offset: 0x00009F45
			public ComponentStore.ScavengeContext ScavengeContext
			{
				get
				{
					if (this._scavengeContext == null)
					{
						Interlocked.CompareExchange(ref this._scavengeContext, new ComponentStore.ScavengeContext(this._compStore), null);
					}
					return (ComponentStore.ScavengeContext)this._scavengeContext;
				}
			}

			// Token: 0x040000E1 RID: 225
			private object _scavengeContext;

			// Token: 0x040000E2 RID: 226
			private ComponentStore _compStore;
		}

		// Token: 0x0200002B RID: 43
		private class ScavengeContext
		{
			// Token: 0x0600018C RID: 396 RVA: 0x0000AF72 File Offset: 0x00009F72
			public ScavengeContext(ComponentStore compStore)
			{
				this._compStore = compStore;
			}

			// Token: 0x0600018D RID: 397 RVA: 0x0000AF84 File Offset: 0x00009F84
			public void CheckQuotaAndScavenge()
			{
				ulong onlineAppQuotaInBytes = this._compStore.GetOnlineAppQuotaInBytes();
				ulong onlineAppQuotaUsageEstimate = this.GetOnlineAppQuotaUsageEstimate();
				long num = (long)(this._onlineToPinPrivateSizePostTransact - this._onlineToPinPrivateSizePreTransact - this._onlineToUnpinPrivateSize + this._shellVisibleToUnpinSharedSize + this._addinToUnpinSharedSize);
				ulong num2;
				if (num >= 0L)
				{
					num2 = onlineAppQuotaUsageEstimate + (ulong)num;
					if (num2 < onlineAppQuotaUsageEstimate)
					{
						num2 = ulong.MaxValue;
					}
				}
				else
				{
					num2 = onlineAppQuotaUsageEstimate - (ulong)(-(ulong)num);
					if (num2 > onlineAppQuotaUsageEstimate)
					{
						num2 = ulong.MaxValue;
					}
				}
				if (num2 > onlineAppQuotaInBytes)
				{
					IDefinitionAppId[] array2;
					ComponentStore.ScavengeContext.SubInstance[] array = this.CollectOnlineAppsMRU(out array2);
					ulong num3 = 0UL;
					ulong num4 = 0UL;
					if (array2.Length > 0)
					{
						this._compStore.CalculateDeploymentsUnderQuota(array2.Length, array2, ulong.MaxValue, ref num3, ref num4);
						if (num3 > onlineAppQuotaInBytes)
						{
							ulong num5 = onlineAppQuotaInBytes / 2UL;
							int num6 = this._compStore.CalculateDeploymentsUnderQuota(array2.Length, array2, num5, ref num3, ref num4);
							bool flag;
							this.ScavengeAppsOverQuota(array, array2.Length - num6, out flag);
							if (flag)
							{
								this.CollectOnlineApps(out array2);
								this._compStore.CalculateDeploymentsUnderQuota(array2.Length, array2, ulong.MaxValue, ref num3, ref num4);
							}
						}
					}
					num2 = num3;
				}
				ComponentStore.ScavengeContext.PersistOnlineAppQuotaUsageEstimate(num2);
			}

			// Token: 0x0600018E RID: 398 RVA: 0x0000B080 File Offset: 0x0000A080
			public void AddOnlineAppToCommit(DefinitionAppId appId, SubscriptionState subState)
			{
				DefinitionAppId definitionAppId = appId.ToDeploymentAppId();
				ComponentStore.ScavengeContext.AddDeploymentToList(ref this._onlineDeploysToPin, definitionAppId);
				if (appId.Equals(subState.CurrentBind) || appId.Equals(subState.PreviousBind))
				{
					ComponentStore.ScavengeContext.AddDeploymentToList(ref this._onlineDeploysToPinAlreadyPinned, definitionAppId);
				}
			}

			// Token: 0x0600018F RID: 399 RVA: 0x0000B0C8 File Offset: 0x0000A0C8
			public void AddDeploymentToUnpin(DefinitionAppId deployAppId, SubscriptionState subState)
			{
				if (subState.IsShellVisible)
				{
					ComponentStore.ScavengeContext.AddDeploymentToList(ref this._shellVisbleDeploysToUnpin, deployAppId);
					return;
				}
				if (subState.appType == AppType.CustomHostSpecified)
				{
					ComponentStore.ScavengeContext.AddDeploymentToList(ref this._addinDeploysToUnpin, deployAppId);
					return;
				}
				ComponentStore.ScavengeContext.AddDeploymentToList(ref this._onlineDeploysToUnpin, deployAppId);
			}

			// Token: 0x06000190 RID: 400 RVA: 0x0000B104 File Offset: 0x0000A104
			public void CalculateSizesPreTransact()
			{
				this._onlineToPinPrivateSizePreTransact = this._compStore.GetPrivateSize(this._onlineDeploysToPinAlreadyPinned);
				this._onlineToUnpinPrivateSize = this._compStore.GetPrivateSize(this._onlineDeploysToUnpin);
				this._shellVisibleToUnpinSharedSize = this._compStore.GetSharedSize(this._shellVisbleDeploysToUnpin);
				this._addinToUnpinSharedSize = this._compStore.GetSharedSize(this._addinDeploysToUnpin);
			}

			// Token: 0x06000191 RID: 401 RVA: 0x0000B16D File Offset: 0x0000A16D
			public void CalculateSizesPostTransact()
			{
				this._onlineToPinPrivateSizePostTransact = this._compStore.GetPrivateSize(this._onlineDeploysToPin);
			}

			// Token: 0x06000192 RID: 402 RVA: 0x0000B188 File Offset: 0x0000A188
			public void CleanOnlineAppCache()
			{
				IDefinitionAppId[] array2;
				ComponentStore.ScavengeContext.SubInstance[] array = this.CollectOnlineApps(out array2);
				using (ComponentStore.StoreTransactionContext storeTransactionContext = new ComponentStore.StoreTransactionContext(this._compStore))
				{
					foreach (ComponentStore.ScavengeContext.SubInstance subInstance in array)
					{
						SubscriptionStateInternal subscriptionStateInternal = new SubscriptionStateInternal(subInstance.SubState);
						subscriptionStateInternal.IsInstalled = false;
						this._compStore.PrepareFinalizeSubscriptionState(storeTransactionContext, subInstance.SubState, subscriptionStateInternal);
					}
					this._compStore.SubmitStoreTransaction(storeTransactionContext, null);
				}
				array = this.CollectOnlineApps(out array2);
				ulong num = 0UL;
				ulong num2 = 0UL;
				if (array2.Length > 0)
				{
					this._compStore.CalculateDeploymentsUnderQuota(array2.Length, array2, ulong.MaxValue, ref num, ref num2);
				}
				ComponentStore.ScavengeContext.PersistOnlineAppQuotaUsageEstimate(num);
			}

			// Token: 0x06000193 RID: 403 RVA: 0x0000B24C File Offset: 0x0000A24C
			private static void AddDeploymentToList(ref ArrayList list, DefinitionAppId deployAppId)
			{
				if (list == null)
				{
					list = new ArrayList();
				}
				if (!list.Contains(deployAppId))
				{
					list.Add(deployAppId);
				}
			}

			// Token: 0x06000194 RID: 404 RVA: 0x0000B26C File Offset: 0x0000A26C
			private ComponentStore.ScavengeContext.SubInstance[] CollectOnlineApps(out IDefinitionAppId[] deployAppIdPtrs)
			{
				Hashtable hashtable = new Hashtable();
				StoreAssemblyEnumeration storeAssemblyEnumeration = this._compStore._store.EnumAssemblies(Store.EnumAssembliesFlags.Nothing);
				foreach (object obj in storeAssemblyEnumeration)
				{
					DefinitionIdentity definitionIdentity = new DefinitionIdentity(((STORE_ASSEMBLY)obj).DefinitionIdentity);
					DefinitionIdentity definitionIdentity2 = definitionIdentity.ToSubscriptionId();
					SubscriptionState subscriptionState = this._compStore._subStore.GetSubscriptionState(definitionIdentity2);
					if (subscriptionState.IsInstalled && !subscriptionState.IsShellVisible && subscriptionState.appType != AppType.CustomHostSpecified && !hashtable.Contains(definitionIdentity2))
					{
						hashtable.Add(definitionIdentity2, new ComponentStore.ScavengeContext.SubInstance
						{
							SubState = subscriptionState,
							LastAccessTime = subscriptionState.LastCheckTime
						});
					}
				}
				ComponentStore.ScavengeContext.SubInstance[] array = new ComponentStore.ScavengeContext.SubInstance[hashtable.Count];
				hashtable.Values.CopyTo(array, 0);
				ArrayList arrayList = new ArrayList();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].SubState.CurrentBind != null)
					{
						arrayList.Add(array[i].SubState.CurrentBind.ToDeploymentAppId().ComPointer);
					}
					if (array[i].SubState.PreviousBind != null)
					{
						arrayList.Add(array[i].SubState.PreviousBind.ToDeploymentAppId().ComPointer);
					}
				}
				deployAppIdPtrs = (IDefinitionAppId[])arrayList.ToArray(typeof(IDefinitionAppId));
				return array;
			}

			// Token: 0x06000195 RID: 405 RVA: 0x0000B408 File Offset: 0x0000A408
			private ComponentStore.ScavengeContext.SubInstance[] CollectOnlineAppsMRU(out IDefinitionAppId[] deployAppIdPtrs)
			{
				Hashtable hashtable = new Hashtable();
				StoreAssemblyEnumeration storeAssemblyEnumeration = this._compStore._store.EnumAssemblies(Store.EnumAssembliesFlags.Nothing);
				foreach (object obj in storeAssemblyEnumeration)
				{
					DefinitionIdentity definitionIdentity = new DefinitionIdentity(((STORE_ASSEMBLY)obj).DefinitionIdentity);
					DefinitionIdentity definitionIdentity2 = definitionIdentity.ToSubscriptionId();
					SubscriptionState subscriptionState = this._compStore._subStore.GetSubscriptionState(definitionIdentity2);
					if (subscriptionState.IsInstalled && !subscriptionState.IsShellVisible && subscriptionState.appType != AppType.CustomHostSpecified && !hashtable.Contains(definitionIdentity2))
					{
						hashtable.Add(definitionIdentity2, new ComponentStore.ScavengeContext.SubInstance
						{
							SubState = subscriptionState,
							LastAccessTime = subscriptionState.LastCheckTime
						});
					}
				}
				ComponentStore.ScavengeContext.SubInstance[] array = new ComponentStore.ScavengeContext.SubInstance[hashtable.Count];
				hashtable.Values.CopyTo(array, 0);
				Array.Sort<ComponentStore.ScavengeContext.SubInstance>(array);
				ArrayList arrayList = new ArrayList();
				for (int i = 0; i < array.Length; i++)
				{
					if (array[i].SubState.CurrentBind != null)
					{
						arrayList.Add(array[i].SubState.CurrentBind.ToDeploymentAppId().ComPointer);
					}
					if (array[i].SubState.PreviousBind != null)
					{
						arrayList.Add(array[i].SubState.PreviousBind.ToDeploymentAppId().ComPointer);
					}
				}
				deployAppIdPtrs = (IDefinitionAppId[])arrayList.ToArray(typeof(IDefinitionAppId));
				return array;
			}

			// Token: 0x06000196 RID: 406 RVA: 0x0000B5AC File Offset: 0x0000A5AC
			private void ScavengeAppsOverQuota(ComponentStore.ScavengeContext.SubInstance[] subs, int deploysToScavenge, out bool appExcluded)
			{
				appExcluded = false;
				DateTime dateTime = DateTime.UtcNow - Constants.OnlineAppScavengingGracePeriod;
				using (ComponentStore.StoreTransactionContext storeTransactionContext = new ComponentStore.StoreTransactionContext(this._compStore))
				{
					int num = subs.Length - 1;
					while (num >= 0 && deploysToScavenge > 0)
					{
						bool flag = false;
						bool flag2 = false;
						if (subs[num].SubState.PreviousBind != null)
						{
							if (subs[num].LastAccessTime >= dateTime)
							{
								appExcluded = true;
							}
							else
							{
								flag = true;
							}
							deploysToScavenge--;
						}
						if (deploysToScavenge > 0)
						{
							if (subs[num].LastAccessTime >= dateTime)
							{
								appExcluded = true;
							}
							else
							{
								flag2 = true;
							}
							deploysToScavenge--;
						}
						if (flag2 || flag)
						{
							SubscriptionStateInternal subscriptionStateInternal = new SubscriptionStateInternal(subs[num].SubState);
							if (flag2)
							{
								subscriptionStateInternal.IsInstalled = false;
							}
							else
							{
								subscriptionStateInternal.PreviousBind = null;
							}
							this._compStore.PrepareFinalizeSubscriptionState(storeTransactionContext, subs[num].SubState, subscriptionStateInternal);
						}
						num--;
					}
					this._compStore.SubmitStoreTransaction(storeTransactionContext, null);
				}
			}

			// Token: 0x06000197 RID: 407 RVA: 0x0000B6B0 File Offset: 0x0000A6B0
			private ulong GetOnlineAppQuotaUsageEstimate()
			{
				ulong num = ulong.MaxValue;
				ulong num3;
				using (RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("SOFTWARE\\Classes\\Software\\Microsoft\\Windows\\CurrentVersion\\Deployment"))
				{
					if (registryKey != null)
					{
						object value = registryKey.GetValue("OnlineAppQuotaUsageEstimate");
						if (value is long)
						{
							long num2 = (long)value;
							num = (ulong)((num2 >= 0L) ? num2 : (-1L - -num2 + 1L));
						}
					}
					num3 = num;
				}
				return num3;
			}

			// Token: 0x06000198 RID: 408 RVA: 0x0000B720 File Offset: 0x0000A720
			private static void PersistOnlineAppQuotaUsageEstimate(ulong usage)
			{
				using (RegistryKey registryKey = Registry.CurrentUser.CreateSubKey("SOFTWARE\\Classes\\Software\\Microsoft\\Windows\\CurrentVersion\\Deployment"))
				{
					if (registryKey != null)
					{
						registryKey.SetValue("OnlineAppQuotaUsageEstimate", usage, RegistryValueKind.QWord);
					}
				}
			}

			// Token: 0x040000E3 RID: 227
			private ComponentStore _compStore;

			// Token: 0x040000E4 RID: 228
			private ArrayList _onlineDeploysToPin;

			// Token: 0x040000E5 RID: 229
			private ArrayList _onlineDeploysToPinAlreadyPinned;

			// Token: 0x040000E6 RID: 230
			private ArrayList _shellVisbleDeploysToUnpin;

			// Token: 0x040000E7 RID: 231
			private ArrayList _addinDeploysToUnpin;

			// Token: 0x040000E8 RID: 232
			private ArrayList _onlineDeploysToUnpin;

			// Token: 0x040000E9 RID: 233
			private ulong _onlineToPinPrivateSizePreTransact;

			// Token: 0x040000EA RID: 234
			private ulong _onlineToPinPrivateSizePostTransact;

			// Token: 0x040000EB RID: 235
			private ulong _shellVisibleToUnpinSharedSize;

			// Token: 0x040000EC RID: 236
			private ulong _onlineToUnpinPrivateSize;

			// Token: 0x040000ED RID: 237
			private ulong _addinToUnpinSharedSize;

			// Token: 0x0200002C RID: 44
			private class SubInstance : IComparable
			{
				// Token: 0x06000199 RID: 409 RVA: 0x0000B770 File Offset: 0x0000A770
				public int CompareTo(object other)
				{
					return ((ComponentStore.ScavengeContext.SubInstance)other).LastAccessTime.CompareTo(this.LastAccessTime);
				}

				// Token: 0x040000EE RID: 238
				public SubscriptionState SubState;

				// Token: 0x040000EF RID: 239
				public DateTime LastAccessTime;
			}
		}
	}
}
