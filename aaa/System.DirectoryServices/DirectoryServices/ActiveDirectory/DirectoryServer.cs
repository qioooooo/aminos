using System;
using System.Collections;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000084 RID: 132
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public abstract class DirectoryServer : IDisposable
	{
		// Token: 0x060003C7 RID: 967 RVA: 0x000142CC File Offset: 0x000132CC
		~DirectoryServer()
		{
			this.Dispose(false);
		}

		// Token: 0x060003C8 RID: 968 RVA: 0x000142FC File Offset: 0x000132FC
		public void Dispose()
		{
			this.Dispose(true);
			GC.SuppressFinalize(this);
		}

		// Token: 0x060003C9 RID: 969 RVA: 0x0001430C File Offset: 0x0001330C
		protected virtual void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				if (disposing)
				{
					foreach (object obj in this.directoryEntryMgr.GetCachedDirectoryEntries())
					{
						DirectoryEntry directoryEntry = (DirectoryEntry)obj;
						directoryEntry.Dispose();
					}
				}
				this.disposed = true;
			}
		}

		// Token: 0x060003CA RID: 970 RVA: 0x0001437C File Offset: 0x0001337C
		public override string ToString()
		{
			return this.Name;
		}

		// Token: 0x060003CB RID: 971 RVA: 0x00014384 File Offset: 0x00013384
		public void MoveToAnotherSite(string siteName)
		{
			this.CheckIfDisposed();
			if (siteName == null)
			{
				throw new ArgumentNullException("siteName");
			}
			if (siteName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "siteName");
			}
			if (Utils.Compare(this.SiteName, siteName) != 0)
			{
				DirectoryEntry directoryEntry = null;
				try
				{
					string text = "CN=Servers,CN=" + siteName + "," + this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.SitesContainer);
					directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, text);
					string text2 = ((this is DomainController) ? ((DomainController)this).ServerObjectName : ((AdamInstance)this).ServerObjectName);
					DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(text2);
					string text3 = (string)PropertyManager.GetPropertyValue(this.context, cachedDirectoryEntry, PropertyManager.DistinguishedName);
					cachedDirectoryEntry.MoveTo(directoryEntry);
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				finally
				{
					if (directoryEntry != null)
					{
						directoryEntry.Dispose();
					}
				}
				this.siteInfoModified = true;
				this.cachedSiteName = null;
				if (this.cachedSiteObjectName != null)
				{
					this.directoryEntryMgr.RemoveIfExists(this.cachedSiteObjectName);
					this.cachedSiteObjectName = null;
				}
				if (this.cachedServerObjectName != null)
				{
					this.directoryEntryMgr.RemoveIfExists(this.cachedServerObjectName);
					this.cachedServerObjectName = null;
				}
				if (this.cachedNtdsaObjectName != null)
				{
					this.directoryEntryMgr.RemoveIfExists(this.cachedNtdsaObjectName);
					this.cachedNtdsaObjectName = null;
				}
			}
		}

		// Token: 0x060003CC RID: 972 RVA: 0x000144F4 File Offset: 0x000134F4
		public DirectoryEntry GetDirectoryEntry()
		{
			this.CheckIfDisposed();
			string text = ((this is DomainController) ? ((DomainController)this).ServerObjectName : ((AdamInstance)this).ServerObjectName);
			return DirectoryEntryManager.GetDirectoryEntry(this.context, text);
		}

		// Token: 0x060003CD RID: 973
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public abstract void CheckReplicationConsistency();

		// Token: 0x060003CE RID: 974
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public abstract ReplicationCursorCollection GetReplicationCursors(string partition);

		// Token: 0x060003CF RID: 975
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public abstract ReplicationOperationInformation GetReplicationOperationInformation();

		// Token: 0x060003D0 RID: 976
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public abstract ReplicationNeighborCollection GetReplicationNeighbors(string partition);

		// Token: 0x060003D1 RID: 977
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public abstract ReplicationNeighborCollection GetAllReplicationNeighbors();

		// Token: 0x060003D2 RID: 978
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public abstract ReplicationFailureCollection GetReplicationConnectionFailures();

		// Token: 0x060003D3 RID: 979
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public abstract ActiveDirectoryReplicationMetadata GetReplicationMetadata(string objectPath);

		// Token: 0x060003D4 RID: 980
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public abstract void SyncReplicaFromServer(string partition, string sourceServer);

		// Token: 0x060003D5 RID: 981
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public abstract void TriggerSyncReplicaFromNeighbors(string partition);

		// Token: 0x060003D6 RID: 982
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public abstract void SyncReplicaFromAllServers(string partition, SyncFromAllServersOptions options);

		// Token: 0x170000EC RID: 236
		// (get) Token: 0x060003D7 RID: 983 RVA: 0x00014534 File Offset: 0x00013534
		public string Name
		{
			get
			{
				this.CheckIfDisposed();
				return this.replicaName;
			}
		}

		// Token: 0x170000ED RID: 237
		// (get) Token: 0x060003D8 RID: 984 RVA: 0x00014542 File Offset: 0x00013542
		public ReadOnlyStringCollection Partitions
		{
			get
			{
				this.CheckIfDisposed();
				if (this.cachedPartitions == null)
				{
					this.cachedPartitions = new ReadOnlyStringCollection(this.GetPartitions());
				}
				return this.cachedPartitions;
			}
		}

		// Token: 0x170000EE RID: 238
		// (get) Token: 0x060003D9 RID: 985
		public abstract string IPAddress
		{
			[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			get;
		}

		// Token: 0x170000EF RID: 239
		// (get) Token: 0x060003DA RID: 986
		public abstract string SiteName
		{
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
			get;
		}

		// Token: 0x170000F0 RID: 240
		// (get) Token: 0x060003DB RID: 987
		// (set) Token: 0x060003DC RID: 988
		public abstract SyncUpdateCallback SyncFromAllServersCallback
		{
			[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			get;
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
			set;
		}

		// Token: 0x170000F1 RID: 241
		// (get) Token: 0x060003DD RID: 989
		public abstract ReplicationConnectionCollection InboundConnections
		{
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
			get;
		}

		// Token: 0x170000F2 RID: 242
		// (get) Token: 0x060003DE RID: 990
		public abstract ReplicationConnectionCollection OutboundConnections
		{
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
			get;
		}

		// Token: 0x060003DF RID: 991 RVA: 0x0001456C File Offset: 0x0001356C
		internal ArrayList GetPartitions()
		{
			ArrayList arrayList = new ArrayList();
			DirectoryEntry directoryEntry = null;
			DirectoryEntry directoryEntry2 = null;
			try
			{
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.RootDSE);
				foreach (object obj in directoryEntry.Properties[PropertyManager.NamingContexts])
				{
					string text = (string)obj;
					arrayList.Add(text);
				}
				string text2 = ((this is DomainController) ? ((DomainController)this).NtdsaObjectName : ((AdamInstance)this).NtdsaObjectName);
				directoryEntry2 = DirectoryEntryManager.GetDirectoryEntry(this.context, text2);
				ArrayList arrayList2 = new ArrayList();
				arrayList2.Add(PropertyManager.HasPartialReplicaNCs);
				Hashtable hashtable = null;
				try
				{
					hashtable = Utils.GetValuesWithRangeRetrieval(directoryEntry2, null, arrayList2, SearchScope.Base);
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
				ArrayList arrayList3 = (ArrayList)hashtable[PropertyManager.HasPartialReplicaNCs.ToLower(CultureInfo.InvariantCulture)];
				foreach (object obj2 in arrayList3)
				{
					string text3 = (string)obj2;
					arrayList.Add(text3);
				}
			}
			catch (COMException ex2)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex2);
			}
			finally
			{
				if (directoryEntry != null)
				{
					directoryEntry.Dispose();
				}
				if (directoryEntry2 != null)
				{
					directoryEntry2.Dispose();
				}
			}
			return arrayList;
		}

		// Token: 0x060003E0 RID: 992 RVA: 0x00014748 File Offset: 0x00013748
		internal void CheckIfDisposed()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
		}

		// Token: 0x170000F3 RID: 243
		// (get) Token: 0x060003E1 RID: 993 RVA: 0x00014763 File Offset: 0x00013763
		internal DirectoryContext Context
		{
			get
			{
				return this.context;
			}
		}

		// Token: 0x060003E2 RID: 994 RVA: 0x0001476C File Offset: 0x0001376C
		internal void CheckConsistencyHelper(IntPtr dsHandle, LoadLibrarySafeHandle libHandle)
		{
			IntPtr procAddress = UnsafeNativeMethods.GetProcAddress(libHandle, "DsReplicaConsistencyCheck");
			if (procAddress == (IntPtr)0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
			}
			UnsafeNativeMethods.DsReplicaConsistencyCheck dsReplicaConsistencyCheck = (UnsafeNativeMethods.DsReplicaConsistencyCheck)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(UnsafeNativeMethods.DsReplicaConsistencyCheck));
			int num = dsReplicaConsistencyCheck(dsHandle, 0, 0);
			if (num != 0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(num, this.Name);
			}
		}

		// Token: 0x060003E3 RID: 995 RVA: 0x000147D0 File Offset: 0x000137D0
		internal IntPtr GetReplicationInfoHelper(IntPtr dsHandle, int type, int secondaryType, string partition, ref bool advanced, int context, LoadLibrarySafeHandle libHandle)
		{
			IntPtr intPtr = (IntPtr)0;
			int num = 0;
			bool flag = true;
			IntPtr intPtr2 = UnsafeNativeMethods.GetProcAddress(libHandle, "DsReplicaGetInfo2W");
			if (intPtr2 == (IntPtr)0)
			{
				intPtr2 = UnsafeNativeMethods.GetProcAddress(libHandle, "DsReplicaGetInfoW");
				if (intPtr2 == (IntPtr)0)
				{
					throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
				}
				UnsafeNativeMethods.DsReplicaGetInfoW dsReplicaGetInfoW = (UnsafeNativeMethods.DsReplicaGetInfoW)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(UnsafeNativeMethods.DsReplicaGetInfoW));
				num = dsReplicaGetInfoW(dsHandle, secondaryType, partition, (IntPtr)0, ref intPtr);
				advanced = false;
				flag = false;
			}
			else
			{
				UnsafeNativeMethods.DsReplicaGetInfo2W dsReplicaGetInfo2W = (UnsafeNativeMethods.DsReplicaGetInfo2W)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(UnsafeNativeMethods.DsReplicaGetInfo2W));
				num = dsReplicaGetInfo2W(dsHandle, type, partition, (IntPtr)0, null, null, 0, context, ref intPtr);
			}
			if (flag && num == 50)
			{
				intPtr2 = UnsafeNativeMethods.GetProcAddress(libHandle, "DsReplicaGetInfoW");
				if (intPtr2 == (IntPtr)0)
				{
					throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
				}
				UnsafeNativeMethods.DsReplicaGetInfoW dsReplicaGetInfoW2 = (UnsafeNativeMethods.DsReplicaGetInfoW)Marshal.GetDelegateForFunctionPointer(intPtr2, typeof(UnsafeNativeMethods.DsReplicaGetInfoW));
				num = dsReplicaGetInfoW2(dsHandle, secondaryType, partition, (IntPtr)0, ref intPtr);
				advanced = false;
			}
			if (num != 0)
			{
				if (partition != null)
				{
					if (type == 9)
					{
						if (num == ExceptionHelper.ERROR_DS_DRA_BAD_DN || num == ExceptionHelper.ERROR_DS_NAME_UNPARSEABLE)
						{
							throw new ArgumentException(ExceptionHelper.GetErrorMessage(num, false), "objectPath");
						}
						DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, partition);
						try
						{
							directoryEntry.RefreshCache(new string[] { "name" });
							goto IL_0201;
						}
						catch (COMException ex)
						{
							if ((ex.ErrorCode == -2147016672) | (ex.ErrorCode == -2147016656))
							{
								throw new ArgumentException(Res.GetString("DSNoObject"), "objectPath");
							}
							if ((ex.ErrorCode == -2147463168) | (ex.ErrorCode == -2147016654))
							{
								throw new ArgumentException(Res.GetString("DSInvalidPath"), "objectPath");
							}
							goto IL_0201;
						}
					}
					if (!this.Partitions.Contains(partition))
					{
						throw new ArgumentException(Res.GetString("ServerNotAReplica"), "partition");
					}
				}
				IL_0201:
				throw ExceptionHelper.GetExceptionFromErrorCode(num, this.Name);
			}
			return intPtr;
		}

		// Token: 0x060003E4 RID: 996 RVA: 0x000149FC File Offset: 0x000139FC
		internal ReplicationCursorCollection ConstructReplicationCursors(IntPtr dsHandle, bool advanced, IntPtr info, string partition, DirectoryServer server, LoadLibrarySafeHandle libHandle)
		{
			int num = 0;
			ReplicationCursorCollection replicationCursorCollection = new ReplicationCursorCollection(server);
			if (advanced)
			{
				for (;;)
				{
					try
					{
						if (!(info != (IntPtr)0))
						{
							break;
						}
						DS_REPL_CURSORS_3 ds_REPL_CURSORS_ = new DS_REPL_CURSORS_3();
						Marshal.PtrToStructure(info, ds_REPL_CURSORS_);
						int cNumCursors = ds_REPL_CURSORS_.cNumCursors;
						if (cNumCursors > 0)
						{
							replicationCursorCollection.AddHelper(partition, ds_REPL_CURSORS_, advanced, info);
						}
						num = ds_REPL_CURSORS_.dwEnumerationContext;
						if (num == -1 || cNumCursors == 0)
						{
							break;
						}
					}
					finally
					{
						this.FreeReplicaInfo(DS_REPL_INFO_TYPE.DS_REPL_INFO_CURSORS_3_FOR_NC, info, libHandle);
					}
					info = this.GetReplicationInfoHelper(dsHandle, 8, 1, partition, ref advanced, num, libHandle);
				}
			}
			else
			{
				try
				{
					if (info != (IntPtr)0)
					{
						DS_REPL_CURSORS ds_REPL_CURSORS = new DS_REPL_CURSORS();
						Marshal.PtrToStructure(info, ds_REPL_CURSORS);
						replicationCursorCollection.AddHelper(partition, ds_REPL_CURSORS, advanced, info);
					}
				}
				finally
				{
					this.FreeReplicaInfo(DS_REPL_INFO_TYPE.DS_REPL_INFO_CURSORS_FOR_NC, info, libHandle);
				}
			}
			return replicationCursorCollection;
		}

		// Token: 0x060003E5 RID: 997 RVA: 0x00014AD0 File Offset: 0x00013AD0
		internal ReplicationOperationInformation ConstructPendingOperations(IntPtr info, DirectoryServer server, LoadLibrarySafeHandle libHandle)
		{
			ReplicationOperationInformation replicationOperationInformation = new ReplicationOperationInformation();
			ReplicationOperationCollection replicationOperationCollection = new ReplicationOperationCollection(server);
			replicationOperationInformation.collection = replicationOperationCollection;
			try
			{
				if (info != (IntPtr)0)
				{
					DS_REPL_PENDING_OPS ds_REPL_PENDING_OPS = new DS_REPL_PENDING_OPS();
					Marshal.PtrToStructure(info, ds_REPL_PENDING_OPS);
					int cNumPendingOps = ds_REPL_PENDING_OPS.cNumPendingOps;
					if (cNumPendingOps > 0)
					{
						replicationOperationCollection.AddHelper(ds_REPL_PENDING_OPS, info);
						replicationOperationInformation.startTime = DateTime.FromFileTime(ds_REPL_PENDING_OPS.ftimeCurrentOpStarted);
						replicationOperationInformation.currentOp = replicationOperationCollection.GetFirstOperation();
					}
				}
			}
			finally
			{
				this.FreeReplicaInfo(DS_REPL_INFO_TYPE.DS_REPL_INFO_PENDING_OPS, info, libHandle);
			}
			return replicationOperationInformation;
		}

		// Token: 0x060003E6 RID: 998 RVA: 0x00014B5C File Offset: 0x00013B5C
		internal ReplicationNeighborCollection ConstructNeighbors(IntPtr info, DirectoryServer server, LoadLibrarySafeHandle libHandle)
		{
			ReplicationNeighborCollection replicationNeighborCollection = new ReplicationNeighborCollection(server);
			try
			{
				if (info != (IntPtr)0)
				{
					DS_REPL_NEIGHBORS ds_REPL_NEIGHBORS = new DS_REPL_NEIGHBORS();
					Marshal.PtrToStructure(info, ds_REPL_NEIGHBORS);
					int cNumNeighbors = ds_REPL_NEIGHBORS.cNumNeighbors;
					if (cNumNeighbors > 0)
					{
						replicationNeighborCollection.AddHelper(ds_REPL_NEIGHBORS, info);
					}
				}
			}
			finally
			{
				this.FreeReplicaInfo(DS_REPL_INFO_TYPE.DS_REPL_INFO_NEIGHBORS, info, libHandle);
			}
			return replicationNeighborCollection;
		}

		// Token: 0x060003E7 RID: 999 RVA: 0x00014BC0 File Offset: 0x00013BC0
		internal ReplicationFailureCollection ConstructFailures(IntPtr info, DirectoryServer server, LoadLibrarySafeHandle libHandle)
		{
			ReplicationFailureCollection replicationFailureCollection = new ReplicationFailureCollection(server);
			try
			{
				if (info != (IntPtr)0)
				{
					DS_REPL_KCC_DSA_FAILURES ds_REPL_KCC_DSA_FAILURES = new DS_REPL_KCC_DSA_FAILURES();
					Marshal.PtrToStructure(info, ds_REPL_KCC_DSA_FAILURES);
					int cNumEntries = ds_REPL_KCC_DSA_FAILURES.cNumEntries;
					if (cNumEntries > 0)
					{
						replicationFailureCollection.AddHelper(ds_REPL_KCC_DSA_FAILURES, info);
					}
				}
			}
			finally
			{
				this.FreeReplicaInfo(DS_REPL_INFO_TYPE.DS_REPL_INFO_KCC_DSA_CONNECT_FAILURES, info, libHandle);
			}
			return replicationFailureCollection;
		}

		// Token: 0x060003E8 RID: 1000 RVA: 0x00014C24 File Offset: 0x00013C24
		internal ActiveDirectoryReplicationMetadata ConstructMetaData(bool advanced, IntPtr info, DirectoryServer server, LoadLibrarySafeHandle libHandle)
		{
			ActiveDirectoryReplicationMetadata activeDirectoryReplicationMetadata = new ActiveDirectoryReplicationMetadata(server);
			if (advanced)
			{
				try
				{
					if (info != (IntPtr)0)
					{
						DS_REPL_OBJ_META_DATA_2 ds_REPL_OBJ_META_DATA_ = new DS_REPL_OBJ_META_DATA_2();
						Marshal.PtrToStructure(info, ds_REPL_OBJ_META_DATA_);
						int num = ds_REPL_OBJ_META_DATA_.cNumEntries;
						if (num > 0)
						{
							activeDirectoryReplicationMetadata.AddHelper(num, info, true);
						}
					}
					return activeDirectoryReplicationMetadata;
				}
				finally
				{
					this.FreeReplicaInfo(DS_REPL_INFO_TYPE.DS_REPL_INFO_METADATA_2_FOR_OBJ, info, libHandle);
				}
			}
			try
			{
				DS_REPL_OBJ_META_DATA ds_REPL_OBJ_META_DATA = new DS_REPL_OBJ_META_DATA();
				Marshal.PtrToStructure(info, ds_REPL_OBJ_META_DATA);
				int num = ds_REPL_OBJ_META_DATA.cNumEntries;
				if (num > 0)
				{
					activeDirectoryReplicationMetadata.AddHelper(num, info, false);
				}
			}
			finally
			{
				this.FreeReplicaInfo(DS_REPL_INFO_TYPE.DS_REPL_INFO_METADATA_FOR_OBJ, info, libHandle);
			}
			return activeDirectoryReplicationMetadata;
		}

		// Token: 0x060003E9 RID: 1001 RVA: 0x00014CC8 File Offset: 0x00013CC8
		internal bool SyncAllCallbackRoutine(IntPtr data, IntPtr update)
		{
			if (this.SyncFromAllServersCallback == null)
			{
				return true;
			}
			DS_REPSYNCALL_UPDATE ds_REPSYNCALL_UPDATE = new DS_REPSYNCALL_UPDATE();
			Marshal.PtrToStructure(update, ds_REPSYNCALL_UPDATE);
			SyncFromAllServersEvent eventType = ds_REPSYNCALL_UPDATE.eventType;
			IntPtr intPtr = ds_REPSYNCALL_UPDATE.pErrInfo;
			SyncFromAllServersOperationException ex = null;
			if (intPtr != (IntPtr)0)
			{
				ex = ExceptionHelper.CreateSyncAllException(intPtr, true);
				if (ex == null)
				{
					return true;
				}
			}
			string text = null;
			string text2 = null;
			intPtr = ds_REPSYNCALL_UPDATE.pSync;
			if (intPtr != (IntPtr)0)
			{
				DS_REPSYNCALL_SYNC ds_REPSYNCALL_SYNC = new DS_REPSYNCALL_SYNC();
				Marshal.PtrToStructure(intPtr, ds_REPSYNCALL_SYNC);
				text = Marshal.PtrToStringUni(ds_REPSYNCALL_SYNC.pszDstId);
				text2 = Marshal.PtrToStringUni(ds_REPSYNCALL_SYNC.pszSrcId);
			}
			SyncUpdateCallback syncFromAllServersCallback = this.SyncFromAllServersCallback;
			return syncFromAllServersCallback(eventType, text, text2, ex);
		}

		// Token: 0x060003EA RID: 1002 RVA: 0x00014D74 File Offset: 0x00013D74
		internal void SyncReplicaAllHelper(IntPtr handle, SyncReplicaFromAllServersCallback syncAllFunctionPointer, string partition, SyncFromAllServersOptions option, SyncUpdateCallback callback, LoadLibrarySafeHandle libHandle)
		{
			IntPtr intPtr = (IntPtr)0;
			if (!this.Partitions.Contains(partition))
			{
				throw new ArgumentException(Res.GetString("ServerNotAReplica"), "partition");
			}
			IntPtr procAddress = UnsafeNativeMethods.GetProcAddress(libHandle, "DsReplicaSyncAllW");
			if (procAddress == (IntPtr)0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
			}
			UnsafeNativeMethods.DsReplicaSyncAllW dsReplicaSyncAllW = (UnsafeNativeMethods.DsReplicaSyncAllW)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(UnsafeNativeMethods.DsReplicaSyncAllW));
			int num = dsReplicaSyncAllW(handle, partition, (int)(option | (SyncFromAllServersOptions)4), syncAllFunctionPointer, (IntPtr)0, ref intPtr);
			try
			{
				if (intPtr != (IntPtr)0)
				{
					SyncFromAllServersOperationException ex = ExceptionHelper.CreateSyncAllException(intPtr, false);
					if (ex != null)
					{
						throw ex;
					}
				}
				else if (num != 0)
				{
					throw new SyncFromAllServersOperationException(ExceptionHelper.GetErrorMessage(num, false));
				}
			}
			finally
			{
				if (intPtr != (IntPtr)0)
				{
					UnsafeNativeMethods.LocalFree(intPtr);
				}
			}
		}

		// Token: 0x060003EB RID: 1003 RVA: 0x00014E58 File Offset: 0x00013E58
		private void FreeReplicaInfo(DS_REPL_INFO_TYPE type, IntPtr value, LoadLibrarySafeHandle libHandle)
		{
			if (value != (IntPtr)0)
			{
				IntPtr procAddress = UnsafeNativeMethods.GetProcAddress(libHandle, "DsReplicaFreeInfo");
				if (procAddress == (IntPtr)0)
				{
					throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
				}
				UnsafeNativeMethods.DsReplicaFreeInfo dsReplicaFreeInfo = (UnsafeNativeMethods.DsReplicaFreeInfo)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(UnsafeNativeMethods.DsReplicaFreeInfo));
				dsReplicaFreeInfo((int)type, value);
			}
		}

		// Token: 0x060003EC RID: 1004 RVA: 0x00014EB8 File Offset: 0x00013EB8
		internal void SyncReplicaHelper(IntPtr dsHandle, bool isADAM, string partition, string sourceServer, int option, LoadLibrarySafeHandle libHandle)
		{
			int num = Marshal.SizeOf(typeof(Guid));
			IntPtr intPtr = (IntPtr)0;
			Guid guid = Guid.Empty;
			AdamInstance adamInstance = null;
			DomainController domainController = null;
			intPtr = Marshal.AllocHGlobal(num);
			try
			{
				if (sourceServer != null)
				{
					DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(sourceServer, DirectoryContextType.DirectoryServer, this.context);
					if (isADAM)
					{
						adamInstance = AdamInstance.GetAdamInstance(newDirectoryContext);
						guid = adamInstance.NtdsaObjectGuid;
					}
					else
					{
						domainController = DomainController.GetDomainController(newDirectoryContext);
						guid = domainController.NtdsaObjectGuid;
					}
					Marshal.StructureToPtr(guid, intPtr, false);
				}
				IntPtr procAddress = UnsafeNativeMethods.GetProcAddress(libHandle, "DsReplicaSyncW");
				if (procAddress == (IntPtr)0)
				{
					throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
				}
				UnsafeNativeMethods.DsReplicaSyncW dsReplicaSyncW = (UnsafeNativeMethods.DsReplicaSyncW)Marshal.GetDelegateForFunctionPointer(procAddress, typeof(UnsafeNativeMethods.DsReplicaSyncW));
				int num2 = dsReplicaSyncW(dsHandle, partition, intPtr, option);
				if (num2 != 0)
				{
					if (!this.Partitions.Contains(partition))
					{
						throw new ArgumentException(Res.GetString("ServerNotAReplica"), "partition");
					}
					string text = null;
					if (num2 == ExceptionHelper.RPC_S_SERVER_UNAVAILABLE)
					{
						text = sourceServer;
					}
					else if (num2 == ExceptionHelper.RPC_S_CALL_FAILED)
					{
						text = this.Name;
					}
					throw ExceptionHelper.GetExceptionFromErrorCode(num2, text);
				}
			}
			finally
			{
				if (intPtr != (IntPtr)0)
				{
					Marshal.FreeHGlobal(intPtr);
				}
				if (adamInstance != null)
				{
					adamInstance.Dispose();
				}
				if (domainController != null)
				{
					domainController.Dispose();
				}
			}
		}

		// Token: 0x060003ED RID: 1005 RVA: 0x00015014 File Offset: 0x00014014
		internal ReplicationConnectionCollection GetInboundConnectionsHelper()
		{
			if (this.inbound == null)
			{
				this.inbound = new ReplicationConnectionCollection();
				DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(this.Name, DirectoryContextType.DirectoryServer, this.context);
				string text = ((this is DomainController) ? ((DomainController)this).ServerObjectName : ((AdamInstance)this).ServerObjectName);
				string text2 = "CN=NTDS Settings," + text;
				DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(Utils.GetNewDirectoryContext(this.Name, DirectoryContextType.DirectoryServer, this.context), text2);
				ADSearcher adsearcher = new ADSearcher(directoryEntry, "(&(objectClass=nTDSConnection)(objectCategory=nTDSConnection))", new string[] { "cn" }, SearchScope.OneLevel);
				SearchResultCollection searchResultCollection = null;
				try
				{
					searchResultCollection = adsearcher.FindAll();
					foreach (object obj in searchResultCollection)
					{
						SearchResult searchResult = (SearchResult)obj;
						ReplicationConnection replicationConnection = new ReplicationConnection(newDirectoryContext, searchResult.GetDirectoryEntry(), (string)PropertyManager.GetSearchResultPropertyValue(searchResult, PropertyManager.Cn));
						this.inbound.Add(replicationConnection);
					}
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(newDirectoryContext, ex);
				}
				finally
				{
					if (searchResultCollection != null)
					{
						searchResultCollection.Dispose();
					}
					directoryEntry.Dispose();
				}
			}
			return this.inbound;
		}

		// Token: 0x060003EE RID: 1006 RVA: 0x00015174 File Offset: 0x00014174
		internal ReplicationConnectionCollection GetOutboundConnectionsHelper()
		{
			if (this.outbound == null)
			{
				string text = ((this is DomainController) ? ((DomainController)this).SiteObjectName : ((AdamInstance)this).SiteObjectName);
				DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(Utils.GetNewDirectoryContext(this.Name, DirectoryContextType.DirectoryServer, this.context), text);
				string text2 = ((this is DomainController) ? ((DomainController)this).ServerObjectName : ((AdamInstance)this).ServerObjectName);
				ADSearcher adsearcher = new ADSearcher(directoryEntry, "(&(objectClass=nTDSConnection)(objectCategory=nTDSConnection)(fromServer=CN=NTDS Settings," + text2 + "))", new string[] { "objectClass", "cn" }, SearchScope.Subtree);
				SearchResultCollection searchResultCollection = null;
				DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(this.Name, DirectoryContextType.DirectoryServer, this.context);
				try
				{
					searchResultCollection = adsearcher.FindAll();
					this.outbound = new ReplicationConnectionCollection();
					foreach (object obj in searchResultCollection)
					{
						SearchResult searchResult = (SearchResult)obj;
						ReplicationConnection replicationConnection = new ReplicationConnection(newDirectoryContext, searchResult.GetDirectoryEntry(), (string)searchResult.Properties["cn"][0]);
						this.outbound.Add(replicationConnection);
					}
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(newDirectoryContext, ex);
				}
				finally
				{
					if (searchResultCollection != null)
					{
						searchResultCollection.Dispose();
					}
					directoryEntry.Dispose();
				}
			}
			return this.outbound;
		}

		// Token: 0x04000395 RID: 917
		internal const int DS_REPSYNC_ASYNCHRONOUS_OPERATION = 1;

		// Token: 0x04000396 RID: 918
		internal const int DS_REPSYNC_ALL_SOURCES = 16;

		// Token: 0x04000397 RID: 919
		internal const int DS_REPSYNCALL_ID_SERVERS_BY_DN = 4;

		// Token: 0x04000398 RID: 920
		internal const int DS_REPL_NOTSUPPORTED = 50;

		// Token: 0x04000399 RID: 921
		private const int DS_REPL_INFO_FLAG_IMPROVE_LINKED_ATTRS = 1;

		// Token: 0x0400039A RID: 922
		private bool disposed;

		// Token: 0x0400039B RID: 923
		internal DirectoryContext context;

		// Token: 0x0400039C RID: 924
		internal string replicaName;

		// Token: 0x0400039D RID: 925
		internal DirectoryEntryManager directoryEntryMgr;

		// Token: 0x0400039E RID: 926
		internal bool siteInfoModified;

		// Token: 0x0400039F RID: 927
		internal string cachedSiteName;

		// Token: 0x040003A0 RID: 928
		internal string cachedSiteObjectName;

		// Token: 0x040003A1 RID: 929
		internal string cachedServerObjectName;

		// Token: 0x040003A2 RID: 930
		internal string cachedNtdsaObjectName;

		// Token: 0x040003A3 RID: 931
		internal Guid cachedNtdsaObjectGuid = Guid.Empty;

		// Token: 0x040003A4 RID: 932
		internal string cachedIPAddress;

		// Token: 0x040003A5 RID: 933
		internal ReadOnlyStringCollection cachedPartitions;

		// Token: 0x040003A6 RID: 934
		private ReplicationConnectionCollection inbound;

		// Token: 0x040003A7 RID: 935
		private ReplicationConnectionCollection outbound;
	}
}
