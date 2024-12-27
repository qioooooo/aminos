using System;
using System.Collections;
using System.ComponentModel;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000085 RID: 133
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class AdamInstance : DirectoryServer
	{
		// Token: 0x060003EF RID: 1007 RVA: 0x00015308 File Offset: 0x00014308
		internal AdamInstance(DirectoryContext context, string adamInstanceName)
			: this(context, adamInstanceName, new DirectoryEntryManager(context), true)
		{
		}

		// Token: 0x060003F0 RID: 1008 RVA: 0x0001531C File Offset: 0x0001431C
		internal AdamInstance(DirectoryContext context, string adamInstanceName, DirectoryEntryManager directoryEntryMgr, bool nameIncludesPort)
		{
			this.cachedLdapPort = -1;
			this.cachedSslPort = -1;
			this.ADAMHandle = (IntPtr)0;
			this.authIdentity = IntPtr.Zero;
			base..ctor();
			this.context = context;
			this.replicaName = adamInstanceName;
			this.directoryEntryMgr = directoryEntryMgr;
			this.becomeRoleOwnerAttrs = new string[2];
			this.becomeRoleOwnerAttrs[0] = PropertyManager.BecomeSchemaMaster;
			this.becomeRoleOwnerAttrs[1] = PropertyManager.BecomeDomainMaster;
			this.syncAllFunctionPointer = new SyncReplicaFromAllServersCallback(base.SyncAllCallbackRoutine);
		}

		// Token: 0x060003F1 RID: 1009 RVA: 0x000153A4 File Offset: 0x000143A4
		internal AdamInstance(DirectoryContext context, string adamHostName, DirectoryEntryManager directoryEntryMgr)
		{
			this.cachedLdapPort = -1;
			this.cachedSslPort = -1;
			this.ADAMHandle = (IntPtr)0;
			this.authIdentity = IntPtr.Zero;
			base..ctor();
			this.context = context;
			this.replicaName = adamHostName;
			string text;
			Utils.SplitServerNameAndPortNumber(context.Name, out text);
			if (text != null)
			{
				this.replicaName = this.replicaName + ":" + text;
			}
			this.directoryEntryMgr = directoryEntryMgr;
			this.becomeRoleOwnerAttrs = new string[2];
			this.becomeRoleOwnerAttrs[0] = PropertyManager.BecomeSchemaMaster;
			this.becomeRoleOwnerAttrs[1] = PropertyManager.BecomeDomainMaster;
			this.syncAllFunctionPointer = new SyncReplicaFromAllServersCallback(base.SyncAllCallbackRoutine);
		}

		// Token: 0x060003F2 RID: 1010 RVA: 0x00015454 File Offset: 0x00014454
		~AdamInstance()
		{
			this.Dispose(false);
		}

		// Token: 0x060003F3 RID: 1011 RVA: 0x00015484 File Offset: 0x00014484
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				try
				{
					this.FreeADAMHandle();
					this.disposed = true;
				}
				finally
				{
					base.Dispose();
				}
			}
		}

		// Token: 0x060003F4 RID: 1012 RVA: 0x000154C0 File Offset: 0x000144C0
		public static AdamInstance GetAdamInstance(DirectoryContext context)
		{
			DirectoryEntryManager directoryEntryManager = null;
			string text = null;
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.DirectoryServer)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeADAMServer"), "context");
			}
			if (!context.isServer())
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("AINotFound", new object[] { context.Name }), typeof(AdamInstance), context.Name);
			}
			context = new DirectoryContext(context);
			try
			{
				directoryEntryManager = new DirectoryEntryManager(context);
				DirectoryEntry cachedDirectoryEntry = directoryEntryManager.GetCachedDirectoryEntry(WellKnownDN.RootDSE);
				if (!Utils.CheckCapability(cachedDirectoryEntry, Capability.ActiveDirectoryApplicationMode))
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("AINotFound", new object[] { context.Name }), typeof(AdamInstance), context.Name);
				}
				text = (string)PropertyManager.GetPropertyValue(context, cachedDirectoryEntry, PropertyManager.DnsHostName);
			}
			catch (COMException ex)
			{
				int errorCode = ex.ErrorCode;
				if (errorCode == -2147016646)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("AINotFound", new object[] { context.Name }), typeof(AdamInstance), context.Name);
				}
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
			}
			return new AdamInstance(context, text, directoryEntryManager);
		}

		// Token: 0x060003F5 RID: 1013 RVA: 0x00015608 File Offset: 0x00014608
		public static AdamInstance FindOne(DirectoryContext context, string partitionName)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.ConfigurationSet)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeConfigSet"), "context");
			}
			if (partitionName == null)
			{
				throw new ArgumentNullException("partitionName");
			}
			if (partitionName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "partitionName");
			}
			context = new DirectoryContext(context);
			return ConfigurationSet.FindOneAdamInstance(context, partitionName, null);
		}

		// Token: 0x060003F6 RID: 1014 RVA: 0x0001567C File Offset: 0x0001467C
		public static AdamInstanceCollection FindAll(DirectoryContext context, string partitionName)
		{
			AdamInstanceCollection adamInstanceCollection = null;
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.ConfigurationSet)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeConfigSet"), "context");
			}
			if (partitionName == null)
			{
				throw new ArgumentNullException("partitionName");
			}
			if (partitionName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "partitionName");
			}
			context = new DirectoryContext(context);
			try
			{
				adamInstanceCollection = ConfigurationSet.FindAdamInstances(context, partitionName, null);
			}
			catch (ActiveDirectoryObjectNotFoundException)
			{
				adamInstanceCollection = new AdamInstanceCollection(new ArrayList());
			}
			return adamInstanceCollection;
		}

		// Token: 0x060003F7 RID: 1015 RVA: 0x00015714 File Offset: 0x00014714
		public void TransferRoleOwnership(AdamRole role)
		{
			base.CheckIfDisposed();
			if (role < AdamRole.SchemaRole || role > AdamRole.NamingRole)
			{
				throw new InvalidEnumArgumentException("role", (int)role, typeof(AdamRole));
			}
			try
			{
				DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(WellKnownDN.RootDSE);
				cachedDirectoryEntry.Properties[this.becomeRoleOwnerAttrs[(int)role]].Value = 1;
				cachedDirectoryEntry.CommitChanges();
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			this.cachedRoles = null;
		}

		// Token: 0x060003F8 RID: 1016 RVA: 0x000157A0 File Offset: 0x000147A0
		public void SeizeRoleOwnership(AdamRole role)
		{
			base.CheckIfDisposed();
			string text;
			switch (role)
			{
			case AdamRole.SchemaRole:
				text = this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.SchemaNamingContext);
				break;
			case AdamRole.NamingRole:
				text = this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.PartitionsContainer);
				break;
			default:
				throw new InvalidEnumArgumentException("role", (int)role, typeof(AdamRole));
			}
			DirectoryEntry directoryEntry = null;
			try
			{
				directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, text);
				directoryEntry.Properties[PropertyManager.FsmoRoleOwner].Value = this.NtdsaObjectName;
				directoryEntry.CommitChanges();
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
			this.cachedRoles = null;
		}

		// Token: 0x060003F9 RID: 1017 RVA: 0x0001586C File Offset: 0x0001486C
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public override void CheckReplicationConsistency()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			this.GetADAMHandle();
			base.CheckConsistencyHelper(this.ADAMHandle, DirectoryContext.ADAMHandle);
		}

		// Token: 0x060003FA RID: 1018 RVA: 0x000158A0 File Offset: 0x000148A0
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override ReplicationCursorCollection GetReplicationCursors(string partition)
		{
			IntPtr intPtr = (IntPtr)0;
			int num = 0;
			bool flag = true;
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (partition == null)
			{
				throw new ArgumentNullException("partition");
			}
			if (partition.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "partition");
			}
			this.GetADAMHandle();
			intPtr = base.GetReplicationInfoHelper(this.ADAMHandle, 8, 1, partition, ref flag, num, DirectoryContext.ADAMHandle);
			return base.ConstructReplicationCursors(this.ADAMHandle, flag, intPtr, partition, this, DirectoryContext.ADAMHandle);
		}

		// Token: 0x060003FB RID: 1019 RVA: 0x00015930 File Offset: 0x00014930
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override ReplicationOperationInformation GetReplicationOperationInformation()
		{
			IntPtr intPtr = (IntPtr)0;
			bool flag = true;
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			this.GetADAMHandle();
			intPtr = base.GetReplicationInfoHelper(this.ADAMHandle, 5, 5, null, ref flag, 0, DirectoryContext.ADAMHandle);
			return base.ConstructPendingOperations(intPtr, this, DirectoryContext.ADAMHandle);
		}

		// Token: 0x060003FC RID: 1020 RVA: 0x0001598C File Offset: 0x0001498C
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public override ReplicationNeighborCollection GetReplicationNeighbors(string partition)
		{
			IntPtr intPtr = (IntPtr)0;
			bool flag = true;
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (partition == null)
			{
				throw new ArgumentNullException("partition");
			}
			if (partition.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "partition");
			}
			this.GetADAMHandle();
			intPtr = base.GetReplicationInfoHelper(this.ADAMHandle, 0, 0, partition, ref flag, 0, DirectoryContext.ADAMHandle);
			return base.ConstructNeighbors(intPtr, this, DirectoryContext.ADAMHandle);
		}

		// Token: 0x060003FD RID: 1021 RVA: 0x00015A14 File Offset: 0x00014A14
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override ReplicationNeighborCollection GetAllReplicationNeighbors()
		{
			IntPtr intPtr = (IntPtr)0;
			bool flag = true;
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			this.GetADAMHandle();
			intPtr = base.GetReplicationInfoHelper(this.ADAMHandle, 0, 0, null, ref flag, 0, DirectoryContext.ADAMHandle);
			return base.ConstructNeighbors(intPtr, this, DirectoryContext.ADAMHandle);
		}

		// Token: 0x060003FE RID: 1022 RVA: 0x00015A6E File Offset: 0x00014A6E
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public override ReplicationFailureCollection GetReplicationConnectionFailures()
		{
			return this.GetReplicationFailures(DS_REPL_INFO_TYPE.DS_REPL_INFO_KCC_DSA_CONNECT_FAILURES);
		}

		// Token: 0x060003FF RID: 1023 RVA: 0x00015A78 File Offset: 0x00014A78
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override ActiveDirectoryReplicationMetadata GetReplicationMetadata(string objectPath)
		{
			IntPtr intPtr = (IntPtr)0;
			bool flag = true;
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (objectPath == null)
			{
				throw new ArgumentNullException("objectPath");
			}
			if (objectPath.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "objectPath");
			}
			this.GetADAMHandle();
			intPtr = base.GetReplicationInfoHelper(this.ADAMHandle, 9, 2, objectPath, ref flag, 0, DirectoryContext.ADAMHandle);
			return base.ConstructMetaData(flag, intPtr, this, DirectoryContext.ADAMHandle);
		}

		// Token: 0x06000400 RID: 1024 RVA: 0x00015B00 File Offset: 0x00014B00
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public override void SyncReplicaFromServer(string partition, string sourceServer)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (partition == null)
			{
				throw new ArgumentNullException("partition");
			}
			if (partition.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "partition");
			}
			if (sourceServer == null)
			{
				throw new ArgumentNullException("sourceServer");
			}
			if (sourceServer.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "sourceServer");
			}
			this.GetADAMHandle();
			base.SyncReplicaHelper(this.ADAMHandle, true, partition, sourceServer, 0, DirectoryContext.ADAMHandle);
		}

		// Token: 0x06000401 RID: 1025 RVA: 0x00015B98 File Offset: 0x00014B98
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override void TriggerSyncReplicaFromNeighbors(string partition)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (partition == null)
			{
				throw new ArgumentNullException("partition");
			}
			if (partition.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "partition");
			}
			this.GetADAMHandle();
			base.SyncReplicaHelper(this.ADAMHandle, true, partition, null, 17, DirectoryContext.ADAMHandle);
		}

		// Token: 0x06000402 RID: 1026 RVA: 0x00015C08 File Offset: 0x00014C08
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public override void SyncReplicaFromAllServers(string partition, SyncFromAllServersOptions options)
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			if (partition == null)
			{
				throw new ArgumentNullException("partition");
			}
			if (partition.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "partition");
			}
			this.GetADAMHandle();
			base.SyncReplicaAllHelper(this.ADAMHandle, this.syncAllFunctionPointer, partition, options, this.SyncFromAllServersCallback, DirectoryContext.ADAMHandle);
		}

		// Token: 0x06000403 RID: 1027 RVA: 0x00015C80 File Offset: 0x00014C80
		public void Save()
		{
			base.CheckIfDisposed();
			if (this.defaultPartitionModified)
			{
				DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(this.NtdsaObjectName);
				try
				{
					cachedDirectoryEntry.CommitChanges();
				}
				catch (COMException ex)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
				}
			}
			this.defaultPartitionInitialized = false;
			this.defaultPartitionModified = false;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x06000404 RID: 1028 RVA: 0x00015CE4 File Offset: 0x00014CE4
		public ConfigurationSet ConfigurationSet
		{
			get
			{
				base.CheckIfDisposed();
				if (this.currentConfigSet == null)
				{
					DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(base.Name, DirectoryContextType.DirectoryServer, this.context);
					this.currentConfigSet = ConfigurationSet.GetConfigurationSet(newDirectoryContext);
				}
				return this.currentConfigSet;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x06000405 RID: 1029 RVA: 0x00015D24 File Offset: 0x00014D24
		public string HostName
		{
			get
			{
				base.CheckIfDisposed();
				if (this.cachedHostName == null)
				{
					DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(this.ServerObjectName);
					this.cachedHostName = (string)PropertyManager.GetPropertyValue(this.context, cachedDirectoryEntry, PropertyManager.DnsHostName);
				}
				return this.cachedHostName;
			}
		}

		// Token: 0x170000F6 RID: 246
		// (get) Token: 0x06000406 RID: 1030 RVA: 0x00015D74 File Offset: 0x00014D74
		public int LdapPort
		{
			get
			{
				base.CheckIfDisposed();
				if (this.cachedLdapPort == -1)
				{
					DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(this.NtdsaObjectName);
					this.cachedLdapPort = (int)PropertyManager.GetPropertyValue(this.context, cachedDirectoryEntry, PropertyManager.MsDSPortLDAP);
				}
				return this.cachedLdapPort;
			}
		}

		// Token: 0x170000F7 RID: 247
		// (get) Token: 0x06000407 RID: 1031 RVA: 0x00015DC4 File Offset: 0x00014DC4
		public int SslPort
		{
			get
			{
				base.CheckIfDisposed();
				if (this.cachedSslPort == -1)
				{
					DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(this.NtdsaObjectName);
					this.cachedSslPort = (int)PropertyManager.GetPropertyValue(this.context, cachedDirectoryEntry, PropertyManager.MsDSPortSSL);
				}
				return this.cachedSslPort;
			}
		}

		// Token: 0x170000F8 RID: 248
		// (get) Token: 0x06000408 RID: 1032 RVA: 0x00015E14 File Offset: 0x00014E14
		public AdamRoleCollection Roles
		{
			get
			{
				base.CheckIfDisposed();
				DirectoryEntry directoryEntry = null;
				DirectoryEntry directoryEntry2 = null;
				try
				{
					if (this.cachedRoles == null)
					{
						ArrayList arrayList = new ArrayList();
						directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.SchemaNamingContext));
						if (this.NtdsaObjectName.Equals((string)PropertyManager.GetPropertyValue(this.context, directoryEntry, PropertyManager.FsmoRoleOwner)))
						{
							arrayList.Add(AdamRole.SchemaRole);
						}
						directoryEntry2 = DirectoryEntryManager.GetDirectoryEntry(this.context, this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.PartitionsContainer));
						if (this.NtdsaObjectName.Equals((string)PropertyManager.GetPropertyValue(this.context, directoryEntry2, PropertyManager.FsmoRoleOwner)))
						{
							arrayList.Add(AdamRole.NamingRole);
						}
						this.cachedRoles = new AdamRoleCollection(arrayList);
					}
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
					if (directoryEntry2 != null)
					{
						directoryEntry2.Dispose();
					}
				}
				return this.cachedRoles;
			}
		}

		// Token: 0x170000F9 RID: 249
		// (get) Token: 0x06000409 RID: 1033 RVA: 0x00015F20 File Offset: 0x00014F20
		// (set) Token: 0x0600040A RID: 1034 RVA: 0x00015FC4 File Offset: 0x00014FC4
		public string DefaultPartition
		{
			get
			{
				base.CheckIfDisposed();
				if (!this.defaultPartitionInitialized || this.defaultPartitionModified)
				{
					DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(this.NtdsaObjectName);
					try
					{
						cachedDirectoryEntry.RefreshCache();
						if (cachedDirectoryEntry.Properties[PropertyManager.MsDSDefaultNamingContext].Value == null)
						{
							this.cachedDefaultPartition = null;
						}
						else
						{
							this.cachedDefaultPartition = (string)PropertyManager.GetPropertyValue(this.context, cachedDirectoryEntry, PropertyManager.MsDSDefaultNamingContext);
						}
					}
					catch (COMException ex)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
					}
					this.defaultPartitionInitialized = true;
				}
				return this.cachedDefaultPartition;
			}
			set
			{
				base.CheckIfDisposed();
				DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(this.NtdsaObjectName);
				if (value == null)
				{
					if (cachedDirectoryEntry.Properties.Contains(PropertyManager.MsDSDefaultNamingContext))
					{
						cachedDirectoryEntry.Properties[PropertyManager.MsDSDefaultNamingContext].Clear();
					}
				}
				else
				{
					if (!Utils.IsValidDNFormat(value))
					{
						throw new ArgumentException(Res.GetString("InvalidDNFormat"), "value");
					}
					if (!base.Partitions.Contains(value))
					{
						throw new ArgumentException(Res.GetString("ServerNotAReplica", new object[] { value }), "value");
					}
					cachedDirectoryEntry.Properties[PropertyManager.MsDSDefaultNamingContext].Value = value;
				}
				this.defaultPartitionModified = true;
			}
		}

		// Token: 0x170000FA RID: 250
		// (get) Token: 0x0600040B RID: 1035 RVA: 0x00016080 File Offset: 0x00015080
		public override string IPAddress
		{
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			[DnsPermission(SecurityAction.Assert, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
			get
			{
				base.CheckIfDisposed();
				IPHostEntry hostEntry = Dns.GetHostEntry(this.HostName);
				if (hostEntry.AddressList.GetLength(0) > 0)
				{
					return hostEntry.AddressList[0].ToString();
				}
				return null;
			}
		}

		// Token: 0x170000FB RID: 251
		// (get) Token: 0x0600040C RID: 1036 RVA: 0x000160C0 File Offset: 0x000150C0
		public override string SiteName
		{
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
			get
			{
				base.CheckIfDisposed();
				if (this.cachedSiteName == null)
				{
					DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, this.SiteObjectName);
					try
					{
						this.cachedSiteName = (string)PropertyManager.GetPropertyValue(this.context, directoryEntry, PropertyManager.Cn);
					}
					finally
					{
						directoryEntry.Dispose();
					}
				}
				return this.cachedSiteName;
			}
		}

		// Token: 0x170000FC RID: 252
		// (get) Token: 0x0600040D RID: 1037 RVA: 0x00016128 File Offset: 0x00015128
		internal string SiteObjectName
		{
			get
			{
				base.CheckIfDisposed();
				if (this.cachedSiteObjectName == null)
				{
					string[] array = this.ServerObjectName.Split(new char[] { ',' });
					if (array.GetLength(0) < 3)
					{
						throw new ActiveDirectoryOperationException(Res.GetString("InvalidServerNameFormat"));
					}
					this.cachedSiteObjectName = array[2];
					for (int i = 3; i < array.GetLength(0); i++)
					{
						this.cachedSiteObjectName = this.cachedSiteObjectName + "," + array[i];
					}
				}
				return this.cachedSiteObjectName;
			}
		}

		// Token: 0x170000FD RID: 253
		// (get) Token: 0x0600040E RID: 1038 RVA: 0x000161B0 File Offset: 0x000151B0
		internal string ServerObjectName
		{
			get
			{
				base.CheckIfDisposed();
				if (this.cachedServerObjectName == null)
				{
					DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.RootDSE);
					try
					{
						this.cachedServerObjectName = (string)PropertyManager.GetPropertyValue(this.context, directoryEntry, PropertyManager.ServerName);
					}
					catch (COMException ex)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
					}
					finally
					{
						directoryEntry.Dispose();
					}
				}
				return this.cachedServerObjectName;
			}
		}

		// Token: 0x170000FE RID: 254
		// (get) Token: 0x0600040F RID: 1039 RVA: 0x00016230 File Offset: 0x00015230
		internal string NtdsaObjectName
		{
			get
			{
				base.CheckIfDisposed();
				if (this.cachedNtdsaObjectName == null)
				{
					DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.RootDSE);
					try
					{
						this.cachedNtdsaObjectName = (string)PropertyManager.GetPropertyValue(this.context, directoryEntry, PropertyManager.DsServiceName);
					}
					catch (COMException ex)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
					}
					finally
					{
						directoryEntry.Dispose();
					}
				}
				return this.cachedNtdsaObjectName;
			}
		}

		// Token: 0x170000FF RID: 255
		// (get) Token: 0x06000410 RID: 1040 RVA: 0x000162B0 File Offset: 0x000152B0
		internal Guid NtdsaObjectGuid
		{
			get
			{
				base.CheckIfDisposed();
				if (this.cachedNtdsaObjectGuid == Guid.Empty)
				{
					DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(this.NtdsaObjectName);
					byte[] array = (byte[])PropertyManager.GetPropertyValue(this.context, cachedDirectoryEntry, PropertyManager.ObjectGuid);
					this.cachedNtdsaObjectGuid = new Guid(array);
				}
				return this.cachedNtdsaObjectGuid;
			}
		}

		// Token: 0x17000100 RID: 256
		// (get) Token: 0x06000411 RID: 1041 RVA: 0x00016310 File Offset: 0x00015310
		// (set) Token: 0x06000412 RID: 1042 RVA: 0x00016331 File Offset: 0x00015331
		public override SyncUpdateCallback SyncFromAllServersCallback
		{
			[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			get
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				return this.userDelegate;
			}
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				this.userDelegate = value;
			}
		}

		// Token: 0x17000101 RID: 257
		// (get) Token: 0x06000413 RID: 1043 RVA: 0x00016353 File Offset: 0x00015353
		public override ReplicationConnectionCollection InboundConnections
		{
			[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			get
			{
				return base.GetInboundConnectionsHelper();
			}
		}

		// Token: 0x17000102 RID: 258
		// (get) Token: 0x06000414 RID: 1044 RVA: 0x0001635B File Offset: 0x0001535B
		public override ReplicationConnectionCollection OutboundConnections
		{
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
			get
			{
				return base.GetOutboundConnectionsHelper();
			}
		}

		// Token: 0x06000415 RID: 1045 RVA: 0x00016364 File Offset: 0x00015364
		private ReplicationFailureCollection GetReplicationFailures(DS_REPL_INFO_TYPE type)
		{
			IntPtr intPtr = (IntPtr)0;
			bool flag = true;
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			this.GetADAMHandle();
			intPtr = base.GetReplicationInfoHelper(this.ADAMHandle, (int)type, (int)type, null, ref flag, 0, DirectoryContext.ADAMHandle);
			return base.ConstructFailures(intPtr, this, DirectoryContext.ADAMHandle);
		}

		// Token: 0x06000416 RID: 1046 RVA: 0x000163C0 File Offset: 0x000153C0
		private void GetADAMHandle()
		{
			try
			{
				Monitor.Enter(this);
				if (this.ADAMHandle == IntPtr.Zero)
				{
					if (this.authIdentity == IntPtr.Zero)
					{
						this.authIdentity = Utils.GetAuthIdentity(this.context, DirectoryContext.ADAMHandle);
					}
					string text = this.HostName + ":" + this.LdapPort;
					this.ADAMHandle = Utils.GetDSHandle(text, null, this.authIdentity, DirectoryContext.ADAMHandle);
				}
			}
			finally
			{
				Monitor.Exit(this);
			}
		}

		// Token: 0x06000417 RID: 1047 RVA: 0x0001645C File Offset: 0x0001545C
		private void FreeADAMHandle()
		{
			Monitor.Enter(this);
			Utils.FreeDSHandle(this.ADAMHandle, DirectoryContext.ADAMHandle);
			Utils.FreeAuthIdentity(this.authIdentity, DirectoryContext.ADAMHandle);
			Monitor.Exit(this);
		}

		// Token: 0x040003A8 RID: 936
		private string[] becomeRoleOwnerAttrs;

		// Token: 0x040003A9 RID: 937
		private bool disposed;

		// Token: 0x040003AA RID: 938
		private string cachedHostName;

		// Token: 0x040003AB RID: 939
		private int cachedLdapPort;

		// Token: 0x040003AC RID: 940
		private int cachedSslPort;

		// Token: 0x040003AD RID: 941
		private bool defaultPartitionInitialized;

		// Token: 0x040003AE RID: 942
		private bool defaultPartitionModified;

		// Token: 0x040003AF RID: 943
		private ConfigurationSet currentConfigSet;

		// Token: 0x040003B0 RID: 944
		private string cachedDefaultPartition;

		// Token: 0x040003B1 RID: 945
		private AdamRoleCollection cachedRoles;

		// Token: 0x040003B2 RID: 946
		private IntPtr ADAMHandle;

		// Token: 0x040003B3 RID: 947
		private IntPtr authIdentity;

		// Token: 0x040003B4 RID: 948
		private SyncUpdateCallback userDelegate;

		// Token: 0x040003B5 RID: 949
		private SyncReplicaFromAllServersCallback syncAllFunctionPointer;
	}
}
