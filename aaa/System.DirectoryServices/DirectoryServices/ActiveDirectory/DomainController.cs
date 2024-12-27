using System;
using System.Collections;
using System.ComponentModel;
using System.Globalization;
using System.Net;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Threading;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200009C RID: 156
	[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
	public class DomainController : DirectoryServer
	{
		// Token: 0x060004F8 RID: 1272 RVA: 0x0001CB2B File Offset: 0x0001BB2B
		protected DomainController()
		{
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x0001CB49 File Offset: 0x0001BB49
		internal DomainController(DirectoryContext context, string domainControllerName)
			: this(context, domainControllerName, new DirectoryEntryManager(context))
		{
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x0001CB5C File Offset: 0x0001BB5C
		internal DomainController(DirectoryContext context, string domainControllerName, DirectoryEntryManager directoryEntryMgr)
		{
			this.context = context;
			this.replicaName = domainControllerName;
			this.directoryEntryMgr = directoryEntryMgr;
			this.becomeRoleOwnerAttrs = new string[5];
			this.becomeRoleOwnerAttrs[0] = PropertyManager.BecomeSchemaMaster;
			this.becomeRoleOwnerAttrs[1] = PropertyManager.BecomeDomainMaster;
			this.becomeRoleOwnerAttrs[2] = PropertyManager.BecomePdc;
			this.becomeRoleOwnerAttrs[3] = PropertyManager.BecomeRidMaster;
			this.becomeRoleOwnerAttrs[4] = PropertyManager.BecomeInfrastructureMaster;
			this.syncAllFunctionPointer = new SyncReplicaFromAllServersCallback(base.SyncAllCallbackRoutine);
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x0001CBFC File Offset: 0x0001BBFC
		~DomainController()
		{
			this.Dispose(false);
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001CC2C File Offset: 0x0001BC2C
		protected override void Dispose(bool disposing)
		{
			if (!this.disposed)
			{
				try
				{
					this.FreeDSHandle();
					this.disposed = true;
				}
				finally
				{
					base.Dispose();
				}
			}
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x0001CC68 File Offset: 0x0001BC68
		public static DomainController GetDomainController(DirectoryContext context)
		{
			string text = null;
			DirectoryEntryManager directoryEntryManager = null;
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.DirectoryServer)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeDC"), "context");
			}
			if (!context.isServer())
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DCNotFound", new object[] { context.Name }), typeof(DomainController), context.Name);
			}
			context = new DirectoryContext(context);
			try
			{
				directoryEntryManager = new DirectoryEntryManager(context);
				DirectoryEntry cachedDirectoryEntry = directoryEntryManager.GetCachedDirectoryEntry(WellKnownDN.RootDSE);
				if (!Utils.CheckCapability(cachedDirectoryEntry, Capability.ActiveDirectory))
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DCNotFound", new object[] { context.Name }), typeof(DomainController), context.Name);
				}
				text = (string)PropertyManager.GetPropertyValue(context, cachedDirectoryEntry, PropertyManager.DnsHostName);
			}
			catch (COMException ex)
			{
				int errorCode = ex.ErrorCode;
				if (errorCode == -2147016646)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DCNotFound", new object[] { context.Name }), typeof(DomainController), context.Name);
				}
				throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
			}
			return new DomainController(context, text, directoryEntryManager);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x0001CDB0 File Offset: 0x0001BDB0
		public static DomainController FindOne(DirectoryContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.Domain)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeDomain"), "context");
			}
			return DomainController.FindOneWithCredentialValidation(context, null, (LocatorOptions)0L);
		}

		// Token: 0x060004FF RID: 1279 RVA: 0x0001CDE8 File Offset: 0x0001BDE8
		public static DomainController FindOne(DirectoryContext context, string siteName)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.Domain)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeDomain"), "context");
			}
			if (siteName == null)
			{
				throw new ArgumentNullException("siteName");
			}
			return DomainController.FindOneWithCredentialValidation(context, siteName, (LocatorOptions)0L);
		}

		// Token: 0x06000500 RID: 1280 RVA: 0x0001CE37 File Offset: 0x0001BE37
		public static DomainController FindOne(DirectoryContext context, LocatorOptions flag)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.Domain)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeDomain"), "context");
			}
			return DomainController.FindOneWithCredentialValidation(context, null, flag);
		}

		// Token: 0x06000501 RID: 1281 RVA: 0x0001CE6C File Offset: 0x0001BE6C
		public static DomainController FindOne(DirectoryContext context, string siteName, LocatorOptions flag)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.Domain)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeDomain"), "context");
			}
			if (siteName == null)
			{
				throw new ArgumentNullException("siteName");
			}
			return DomainController.FindOneWithCredentialValidation(context, siteName, flag);
		}

		// Token: 0x06000502 RID: 1282 RVA: 0x0001CEBC File Offset: 0x0001BEBC
		public static DomainControllerCollection FindAll(DirectoryContext context)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.Domain)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeDomain"), "context");
			}
			context = new DirectoryContext(context);
			return DomainController.FindAllInternal(context, context.Name, false, null);
		}

		// Token: 0x06000503 RID: 1283 RVA: 0x0001CF0C File Offset: 0x0001BF0C
		public static DomainControllerCollection FindAll(DirectoryContext context, string siteName)
		{
			if (context == null)
			{
				throw new ArgumentNullException("context");
			}
			if (context.ContextType != DirectoryContextType.Domain)
			{
				throw new ArgumentException(Res.GetString("TargetShouldBeDomain"), "context");
			}
			if (siteName == null)
			{
				throw new ArgumentNullException("siteName");
			}
			context = new DirectoryContext(context);
			return DomainController.FindAllInternal(context, context.Name, false, siteName);
		}

		// Token: 0x06000504 RID: 1284 RVA: 0x0001CF68 File Offset: 0x0001BF68
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public virtual GlobalCatalog EnableGlobalCatalog()
		{
			base.CheckIfDisposed();
			try
			{
				DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(this.NtdsaObjectName);
				int num = 0;
				if (cachedDirectoryEntry.Properties[PropertyManager.Options].Value != null)
				{
					num = (int)cachedDirectoryEntry.Properties[PropertyManager.Options].Value;
				}
				cachedDirectoryEntry.Properties[PropertyManager.Options].Value = num | 1;
				cachedDirectoryEntry.CommitChanges();
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			return new GlobalCatalog(this.context, base.Name);
		}

		// Token: 0x06000505 RID: 1285 RVA: 0x0001D018 File Offset: 0x0001C018
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public virtual bool IsGlobalCatalog()
		{
			base.CheckIfDisposed();
			bool flag;
			try
			{
				DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(this.NtdsaObjectName);
				cachedDirectoryEntry.RefreshCache();
				int num = 0;
				if (cachedDirectoryEntry.Properties[PropertyManager.Options].Value != null)
				{
					num = (int)cachedDirectoryEntry.Properties[PropertyManager.Options].Value;
				}
				if ((num & 1) == 1)
				{
					flag = true;
				}
				else
				{
					flag = false;
				}
			}
			catch (COMException ex)
			{
				throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
			}
			return flag;
		}

		// Token: 0x06000506 RID: 1286 RVA: 0x0001D0A4 File Offset: 0x0001C0A4
		public void TransferRoleOwnership(ActiveDirectoryRole role)
		{
			base.CheckIfDisposed();
			if (role < ActiveDirectoryRole.SchemaRole || role > ActiveDirectoryRole.InfrastructureRole)
			{
				throw new InvalidEnumArgumentException("role", (int)role, typeof(ActiveDirectoryRole));
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

		// Token: 0x06000507 RID: 1287 RVA: 0x0001D130 File Offset: 0x0001C130
		public void SeizeRoleOwnership(ActiveDirectoryRole role)
		{
			base.CheckIfDisposed();
			string text;
			switch (role)
			{
			case ActiveDirectoryRole.SchemaRole:
				text = this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.SchemaNamingContext);
				break;
			case ActiveDirectoryRole.NamingRole:
				text = this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.PartitionsContainer);
				break;
			case ActiveDirectoryRole.PdcRole:
				text = this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.DefaultNamingContext);
				break;
			case ActiveDirectoryRole.RidRole:
				text = this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.RidManager);
				break;
			case ActiveDirectoryRole.InfrastructureRole:
				text = this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.Infrastructure);
				break;
			default:
				throw new InvalidEnumArgumentException("role", (int)role, typeof(ActiveDirectoryRole));
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

		// Token: 0x06000508 RID: 1288 RVA: 0x0001D234 File Offset: 0x0001C234
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public virtual DirectorySearcher GetDirectorySearcher()
		{
			base.CheckIfDisposed();
			return this.InternalGetDirectorySearcher();
		}

		// Token: 0x06000509 RID: 1289 RVA: 0x0001D242 File Offset: 0x0001C242
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		public override void CheckReplicationConsistency()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			this.GetDSHandle();
			base.CheckConsistencyHelper(this.dsHandle, DirectoryContext.ADHandle);
		}

		// Token: 0x0600050A RID: 1290 RVA: 0x0001D274 File Offset: 0x0001C274
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
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
			this.GetDSHandle();
			intPtr = base.GetReplicationInfoHelper(this.dsHandle, 8, 1, partition, ref flag, num, DirectoryContext.ADHandle);
			return base.ConstructReplicationCursors(this.dsHandle, flag, intPtr, partition, this, DirectoryContext.ADHandle);
		}

		// Token: 0x0600050B RID: 1291 RVA: 0x0001D304 File Offset: 0x0001C304
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
			this.GetDSHandle();
			intPtr = base.GetReplicationInfoHelper(this.dsHandle, 5, 5, null, ref flag, 0, DirectoryContext.ADHandle);
			return base.ConstructPendingOperations(intPtr, this, DirectoryContext.ADHandle);
		}

		// Token: 0x0600050C RID: 1292 RVA: 0x0001D360 File Offset: 0x0001C360
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
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
			this.GetDSHandle();
			intPtr = base.GetReplicationInfoHelper(this.dsHandle, 0, 0, partition, ref flag, 0, DirectoryContext.ADHandle);
			return base.ConstructNeighbors(intPtr, this, DirectoryContext.ADHandle);
		}

		// Token: 0x0600050D RID: 1293 RVA: 0x0001D3E8 File Offset: 0x0001C3E8
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
			this.GetDSHandle();
			intPtr = base.GetReplicationInfoHelper(this.dsHandle, 0, 0, null, ref flag, 0, DirectoryContext.ADHandle);
			return base.ConstructNeighbors(intPtr, this, DirectoryContext.ADHandle);
		}

		// Token: 0x0600050E RID: 1294 RVA: 0x0001D442 File Offset: 0x0001C442
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		public override ReplicationFailureCollection GetReplicationConnectionFailures()
		{
			return this.GetReplicationFailures(DS_REPL_INFO_TYPE.DS_REPL_INFO_KCC_DSA_CONNECT_FAILURES);
		}

		// Token: 0x0600050F RID: 1295 RVA: 0x0001D44C File Offset: 0x0001C44C
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
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
			this.GetDSHandle();
			intPtr = base.GetReplicationInfoHelper(this.dsHandle, 9, 2, objectPath, ref flag, 0, DirectoryContext.ADHandle);
			return base.ConstructMetaData(flag, intPtr, this, DirectoryContext.ADHandle);
		}

		// Token: 0x06000510 RID: 1296 RVA: 0x0001D4D4 File Offset: 0x0001C4D4
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
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
			this.GetDSHandle();
			base.SyncReplicaHelper(this.dsHandle, false, partition, sourceServer, 0, DirectoryContext.ADHandle);
		}

		// Token: 0x06000511 RID: 1297 RVA: 0x0001D56C File Offset: 0x0001C56C
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
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
			this.GetDSHandle();
			base.SyncReplicaHelper(this.dsHandle, false, partition, null, 17, DirectoryContext.ADHandle);
		}

		// Token: 0x06000512 RID: 1298 RVA: 0x0001D5DC File Offset: 0x0001C5DC
		[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
		[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
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
			this.GetDSHandle();
			base.SyncReplicaAllHelper(this.dsHandle, this.syncAllFunctionPointer, partition, options, this.SyncFromAllServersCallback, DirectoryContext.ADHandle);
		}

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000513 RID: 1299 RVA: 0x0001D654 File Offset: 0x0001C654
		public Forest Forest
		{
			get
			{
				base.CheckIfDisposed();
				if (this.currentForest == null)
				{
					DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(base.Name, DirectoryContextType.DirectoryServer, this.context);
					this.currentForest = Forest.GetForest(newDirectoryContext);
				}
				return this.currentForest;
			}
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000514 RID: 1300 RVA: 0x0001D694 File Offset: 0x0001C694
		public DateTime CurrentTime
		{
			get
			{
				base.CheckIfDisposed();
				DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.RootDSE);
				string text = null;
				try
				{
					text = (string)PropertyManager.GetPropertyValue(this.context, directoryEntry, PropertyManager.CurrentTime);
				}
				finally
				{
					directoryEntry.Dispose();
				}
				return this.ParseDateTime(text);
			}
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x06000515 RID: 1301 RVA: 0x0001D6F0 File Offset: 0x0001C6F0
		public long HighestCommittedUsn
		{
			get
			{
				base.CheckIfDisposed();
				DirectoryEntry directoryEntry = DirectoryEntryManager.GetDirectoryEntry(this.context, WellKnownDN.RootDSE);
				string text = null;
				try
				{
					text = (string)PropertyManager.GetPropertyValue(this.context, directoryEntry, PropertyManager.HighestCommittedUSN);
				}
				finally
				{
					directoryEntry.Dispose();
				}
				return long.Parse(text, NumberFormatInfo.InvariantInfo);
			}
		}

		// Token: 0x1700012D RID: 301
		// (get) Token: 0x06000516 RID: 1302 RVA: 0x0001D750 File Offset: 0x0001C750
		public string OSVersion
		{
			get
			{
				base.CheckIfDisposed();
				if (this.cachedOSVersion == null)
				{
					DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(this.ComputerObjectName);
					this.cachedOSVersion = (string)PropertyManager.GetPropertyValue(this.context, cachedDirectoryEntry, PropertyManager.OperatingSystem);
				}
				return this.cachedOSVersion;
			}
		}

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x06000517 RID: 1303 RVA: 0x0001D7A0 File Offset: 0x0001C7A0
		internal double NumericOSVersion
		{
			get
			{
				base.CheckIfDisposed();
				if (this.cachedNumericOSVersion == 0.0)
				{
					DirectoryEntry cachedDirectoryEntry = this.directoryEntryMgr.GetCachedDirectoryEntry(this.ComputerObjectName);
					string text = (string)PropertyManager.GetPropertyValue(this.context, cachedDirectoryEntry, PropertyManager.OperatingSystemVersion);
					int num = text.IndexOf('(');
					if (num != -1)
					{
						text = text.Substring(0, num);
					}
					this.cachedNumericOSVersion = double.Parse(text, NumberFormatInfo.InvariantInfo);
				}
				return this.cachedNumericOSVersion;
			}
		}

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x06000518 RID: 1304 RVA: 0x0001D81B File Offset: 0x0001C81B
		public ActiveDirectoryRoleCollection Roles
		{
			get
			{
				base.CheckIfDisposed();
				if (this.cachedRoles == null)
				{
					this.cachedRoles = new ActiveDirectoryRoleCollection(this.GetRoles());
				}
				return this.cachedRoles;
			}
		}

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x06000519 RID: 1305 RVA: 0x0001D844 File Offset: 0x0001C844
		public Domain Domain
		{
			get
			{
				base.CheckIfDisposed();
				if (this.cachedDomain == null)
				{
					string text = null;
					try
					{
						string text2 = this.directoryEntryMgr.ExpandWellKnownDN(WellKnownDN.DefaultNamingContext);
						text = Utils.GetDnsNameFromDN(text2);
					}
					catch (COMException ex)
					{
						throw ExceptionHelper.GetExceptionFromCOMException(this.context, ex);
					}
					DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(base.Name, DirectoryContextType.DirectoryServer, this.context);
					this.cachedDomain = new Domain(newDirectoryContext, text);
				}
				return this.cachedDomain;
			}
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x0600051A RID: 1306 RVA: 0x0001D8BC File Offset: 0x0001C8BC
		public override string IPAddress
		{
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			[DnsPermission(SecurityAction.Assert, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
			get
			{
				base.CheckIfDisposed();
				IPHostEntry hostEntry = Dns.GetHostEntry(base.Name);
				if (hostEntry.AddressList.GetLength(0) > 0)
				{
					return hostEntry.AddressList[0].ToString();
				}
				return null;
			}
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x0600051B RID: 1307 RVA: 0x0001D8FC File Offset: 0x0001C8FC
		public override string SiteName
		{
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
			get
			{
				base.CheckIfDisposed();
				if (!this.dcInfoInitialized || this.siteInfoModified)
				{
					this.GetDomainControllerInfo();
				}
				if (this.cachedSiteName == null)
				{
					throw new ActiveDirectoryOperationException(Res.GetString("SiteNameNotFound", new object[] { base.Name }));
				}
				return this.cachedSiteName;
			}
		}

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x0600051C RID: 1308 RVA: 0x0001D954 File Offset: 0x0001C954
		internal string SiteObjectName
		{
			get
			{
				base.CheckIfDisposed();
				if (!this.dcInfoInitialized || this.siteInfoModified)
				{
					this.GetDomainControllerInfo();
				}
				if (this.cachedSiteObjectName == null)
				{
					throw new ActiveDirectoryOperationException(Res.GetString("SiteObjectNameNotFound", new object[] { base.Name }));
				}
				return this.cachedSiteObjectName;
			}
		}

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x0600051D RID: 1309 RVA: 0x0001D9AC File Offset: 0x0001C9AC
		internal string ComputerObjectName
		{
			get
			{
				base.CheckIfDisposed();
				if (!this.dcInfoInitialized)
				{
					this.GetDomainControllerInfo();
				}
				if (this.cachedComputerObjectName == null)
				{
					throw new ActiveDirectoryOperationException(Res.GetString("ComputerObjectNameNotFound", new object[] { base.Name }));
				}
				return this.cachedComputerObjectName;
			}
		}

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x0600051E RID: 1310 RVA: 0x0001D9FC File Offset: 0x0001C9FC
		internal string ServerObjectName
		{
			get
			{
				base.CheckIfDisposed();
				if (!this.dcInfoInitialized || this.siteInfoModified)
				{
					this.GetDomainControllerInfo();
				}
				if (this.cachedServerObjectName == null)
				{
					throw new ActiveDirectoryOperationException(Res.GetString("ServerObjectNameNotFound", new object[] { base.Name }));
				}
				return this.cachedServerObjectName;
			}
		}

		// Token: 0x17000136 RID: 310
		// (get) Token: 0x0600051F RID: 1311 RVA: 0x0001DA54 File Offset: 0x0001CA54
		internal string NtdsaObjectName
		{
			get
			{
				base.CheckIfDisposed();
				if (!this.dcInfoInitialized || this.siteInfoModified)
				{
					this.GetDomainControllerInfo();
				}
				if (this.cachedNtdsaObjectName == null)
				{
					throw new ActiveDirectoryOperationException(Res.GetString("NtdsaObjectNameNotFound", new object[] { base.Name }));
				}
				return this.cachedNtdsaObjectName;
			}
		}

		// Token: 0x17000137 RID: 311
		// (get) Token: 0x06000520 RID: 1312 RVA: 0x0001DAAC File Offset: 0x0001CAAC
		internal Guid NtdsaObjectGuid
		{
			get
			{
				base.CheckIfDisposed();
				if (!this.dcInfoInitialized || this.siteInfoModified)
				{
					this.GetDomainControllerInfo();
				}
				if (this.cachedNtdsaObjectGuid.Equals(Guid.Empty))
				{
					throw new ActiveDirectoryOperationException(Res.GetString("NtdsaObjectGuidNotFound", new object[] { base.Name }));
				}
				return this.cachedNtdsaObjectGuid;
			}
		}

		// Token: 0x17000138 RID: 312
		// (get) Token: 0x06000521 RID: 1313 RVA: 0x0001DB0E File Offset: 0x0001CB0E
		// (set) Token: 0x06000522 RID: 1314 RVA: 0x0001DB2F File Offset: 0x0001CB2F
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
			[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			set
			{
				if (this.disposed)
				{
					throw new ObjectDisposedException(base.GetType().Name);
				}
				this.userDelegate = value;
			}
		}

		// Token: 0x17000139 RID: 313
		// (get) Token: 0x06000523 RID: 1315 RVA: 0x0001DB51 File Offset: 0x0001CB51
		public override ReplicationConnectionCollection InboundConnections
		{
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
			get
			{
				return base.GetInboundConnectionsHelper();
			}
		}

		// Token: 0x1700013A RID: 314
		// (get) Token: 0x06000524 RID: 1316 RVA: 0x0001DB59 File Offset: 0x0001CB59
		public override ReplicationConnectionCollection OutboundConnections
		{
			[DirectoryServicesPermission(SecurityAction.InheritanceDemand, Unrestricted = true)]
			[DirectoryServicesPermission(SecurityAction.LinkDemand, Unrestricted = true)]
			get
			{
				return base.GetOutboundConnectionsHelper();
			}
		}

		// Token: 0x1700013B RID: 315
		// (get) Token: 0x06000525 RID: 1317 RVA: 0x0001DB61 File Offset: 0x0001CB61
		internal IntPtr Handle
		{
			get
			{
				this.GetDSHandle();
				return this.dsHandle;
			}
		}

		// Token: 0x06000526 RID: 1318 RVA: 0x0001DB70 File Offset: 0x0001CB70
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		internal static void ValidateCredential(DomainController dc, DirectoryContext context)
		{
			DirectoryEntry directoryEntry;
			if (DirectoryContext.ServerBindSupported)
			{
				directoryEntry = new DirectoryEntry("LDAP://" + dc.Name + "/RootDSE", context.UserName, context.Password, Utils.DefaultAuthType | AuthenticationTypes.ServerBind);
			}
			else
			{
				directoryEntry = new DirectoryEntry("LDAP://" + dc.Name + "/RootDSE", context.UserName, context.Password, Utils.DefaultAuthType);
			}
			directoryEntry.Bind(true);
		}

		// Token: 0x06000527 RID: 1319 RVA: 0x0001DBEC File Offset: 0x0001CBEC
		internal static DomainController FindOneWithCredentialValidation(DirectoryContext context, string siteName, LocatorOptions flag)
		{
			bool flag2 = false;
			bool flag3 = false;
			context = new DirectoryContext(context);
			DomainController domainController = DomainController.FindOneInternal(context, context.Name, siteName, flag);
			try
			{
				DomainController.ValidateCredential(domainController, context);
				flag3 = true;
			}
			catch (COMException ex)
			{
				if (ex.ErrorCode != -2147016646)
				{
					throw ExceptionHelper.GetExceptionFromCOMException(context, ex);
				}
				if ((flag & LocatorOptions.ForceRediscovery) != (LocatorOptions)0L)
				{
					throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DCNotFoundInDomain", new object[] { context.Name }), typeof(DomainController), null);
				}
				flag2 = true;
			}
			finally
			{
				if (!flag3)
				{
					domainController.Dispose();
				}
			}
			if (flag2)
			{
				flag3 = false;
				domainController = DomainController.FindOneInternal(context, context.Name, siteName, flag | LocatorOptions.ForceRediscovery);
				try
				{
					DomainController.ValidateCredential(domainController, context);
					flag3 = true;
				}
				catch (COMException ex2)
				{
					if (ex2.ErrorCode == -2147016646)
					{
						throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DCNotFoundInDomain", new object[] { context.Name }), typeof(DomainController), null);
					}
					throw ExceptionHelper.GetExceptionFromCOMException(context, ex2);
				}
				finally
				{
					if (!flag3)
					{
						domainController.Dispose();
					}
				}
			}
			return domainController;
		}

		// Token: 0x06000528 RID: 1320 RVA: 0x0001DD2C File Offset: 0x0001CD2C
		internal static DomainController FindOneInternal(DirectoryContext context, string domainName, string siteName, LocatorOptions flag)
		{
			if (siteName != null && siteName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "siteName");
			}
			if ((flag & ~(LocatorOptions.ForceRediscovery | LocatorOptions.KdcRequired | LocatorOptions.TimeServerRequired | LocatorOptions.WriteableRequired | LocatorOptions.AvoidSelf)) != (LocatorOptions)0L)
			{
				throw new ArgumentException(Res.GetString("InvalidFlags"), "flag");
			}
			if (domainName == null)
			{
				domainName = DirectoryContext.GetLoggedOnDomain();
			}
			DomainControllerInfo domainControllerInfo;
			int num = Locator.DsGetDcNameWrapper(null, domainName, siteName, (long)(flag | (LocatorOptions)16L), out domainControllerInfo);
			if (num == 1355)
			{
				throw new ActiveDirectoryObjectNotFoundException(Res.GetString("DCNotFoundInDomain", new object[] { domainName }), typeof(DomainController), null);
			}
			if (num == 1004)
			{
				throw new ArgumentException(Res.GetString("InvalidFlags"), "flag");
			}
			if (num != 0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(num);
			}
			string text = domainControllerInfo.DomainControllerName.Substring(2);
			DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(text, DirectoryContextType.DirectoryServer, context);
			return new DomainController(newDirectoryContext, text);
		}

		// Token: 0x06000529 RID: 1321 RVA: 0x0001DE0C File Offset: 0x0001CE0C
		internal static DomainControllerCollection FindAllInternal(DirectoryContext context, string domainName, bool isDnsDomainName, string siteName)
		{
			ArrayList arrayList = new ArrayList();
			if (siteName != null && siteName.Length == 0)
			{
				throw new ArgumentException(Res.GetString("EmptyStringParameter"), "siteName");
			}
			if (domainName == null || !isDnsDomainName)
			{
				DomainControllerInfo domainControllerInfo;
				int num = Locator.DsGetDcNameWrapper(null, (domainName != null) ? domainName : DirectoryContext.GetLoggedOnDomain(), null, 16L, out domainControllerInfo);
				if (num == 1355)
				{
					return new DomainControllerCollection(arrayList);
				}
				if (num != 0)
				{
					throw ExceptionHelper.GetExceptionFromErrorCode(num);
				}
				domainName = domainControllerInfo.DomainName;
			}
			foreach (object obj in Utils.GetReplicaList(context, Utils.GetDNFromDnsName(domainName), siteName, true, false, false))
			{
				string text = (string)obj;
				DirectoryContext newDirectoryContext = Utils.GetNewDirectoryContext(text, DirectoryContextType.DirectoryServer, context);
				arrayList.Add(new DomainController(newDirectoryContext, text));
			}
			return new DomainControllerCollection(arrayList);
		}

		// Token: 0x0600052A RID: 1322 RVA: 0x0001DEF4 File Offset: 0x0001CEF4
		private void GetDomainControllerInfo()
		{
			int num = 0;
			IntPtr zero = IntPtr.Zero;
			int num2 = 0;
			bool flag = false;
			this.GetDSHandle();
			IntPtr intPtr = UnsafeNativeMethods.GetProcAddress(DirectoryContext.ADHandle, "DsGetDomainControllerInfoW");
			if (intPtr == (IntPtr)0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
			}
			NativeMethods.DsGetDomainControllerInfo dsGetDomainControllerInfo = (NativeMethods.DsGetDomainControllerInfo)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(NativeMethods.DsGetDomainControllerInfo));
			num2 = 3;
			int num3 = dsGetDomainControllerInfo(this.dsHandle, this.Domain.Name, num2, out num, out zero);
			if (num3 != 0)
			{
				num2 = 2;
				num3 = dsGetDomainControllerInfo(this.dsHandle, this.Domain.Name, num2, out num, out zero);
			}
			if (num3 != 0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(num3, base.Name);
			}
			try
			{
				IntPtr intPtr2 = zero;
				for (int i = 0; i < num; i++)
				{
					if (num2 == 3)
					{
						DsDomainControllerInfo3 dsDomainControllerInfo = new DsDomainControllerInfo3();
						Marshal.PtrToStructure(intPtr2, dsDomainControllerInfo);
						if (dsDomainControllerInfo != null && Utils.Compare(dsDomainControllerInfo.dnsHostName, this.replicaName) == 0)
						{
							flag = true;
							this.cachedSiteName = dsDomainControllerInfo.siteName;
							this.cachedSiteObjectName = dsDomainControllerInfo.siteObjectName;
							this.cachedComputerObjectName = dsDomainControllerInfo.computerObjectName;
							this.cachedServerObjectName = dsDomainControllerInfo.serverObjectName;
							this.cachedNtdsaObjectName = dsDomainControllerInfo.ntdsaObjectName;
							this.cachedNtdsaObjectGuid = dsDomainControllerInfo.ntdsDsaObjectGuid;
						}
						intPtr2 = Utils.AddToIntPtr(intPtr2, Marshal.SizeOf(dsDomainControllerInfo));
					}
					else
					{
						DsDomainControllerInfo2 dsDomainControllerInfo2 = new DsDomainControllerInfo2();
						Marshal.PtrToStructure(intPtr2, dsDomainControllerInfo2);
						if (dsDomainControllerInfo2 != null && Utils.Compare(dsDomainControllerInfo2.dnsHostName, this.replicaName) == 0)
						{
							flag = true;
							this.cachedSiteName = dsDomainControllerInfo2.siteName;
							this.cachedSiteObjectName = dsDomainControllerInfo2.siteObjectName;
							this.cachedComputerObjectName = dsDomainControllerInfo2.computerObjectName;
							this.cachedServerObjectName = dsDomainControllerInfo2.serverObjectName;
							this.cachedNtdsaObjectName = dsDomainControllerInfo2.ntdsaObjectName;
							this.cachedNtdsaObjectGuid = dsDomainControllerInfo2.ntdsDsaObjectGuid;
						}
						intPtr2 = Utils.AddToIntPtr(intPtr2, Marshal.SizeOf(dsDomainControllerInfo2));
					}
				}
			}
			finally
			{
				if (zero != IntPtr.Zero)
				{
					intPtr = UnsafeNativeMethods.GetProcAddress(DirectoryContext.ADHandle, "DsFreeDomainControllerInfoW");
					if (intPtr == (IntPtr)0)
					{
						throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
					}
					NativeMethods.DsFreeDomainControllerInfo dsFreeDomainControllerInfo = (NativeMethods.DsFreeDomainControllerInfo)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(NativeMethods.DsFreeDomainControllerInfo));
					dsFreeDomainControllerInfo(num2, num, zero);
				}
			}
			if (!flag)
			{
				throw new ActiveDirectoryOperationException(Res.GetString("DCInfoNotFound"));
			}
			this.dcInfoInitialized = true;
			this.siteInfoModified = false;
		}

		// Token: 0x0600052B RID: 1323 RVA: 0x0001E184 File Offset: 0x0001D184
		internal void GetDSHandle()
		{
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			try
			{
				Monitor.Enter(this);
				if (this.dsHandle == IntPtr.Zero)
				{
					if (this.authIdentity == IntPtr.Zero)
					{
						this.authIdentity = Utils.GetAuthIdentity(this.context, DirectoryContext.ADHandle);
					}
					this.dsHandle = Utils.GetDSHandle(this.replicaName, null, this.authIdentity, DirectoryContext.ADHandle);
				}
			}
			finally
			{
				Monitor.Exit(this);
			}
		}

		// Token: 0x0600052C RID: 1324 RVA: 0x0001E220 File Offset: 0x0001D220
		internal void FreeDSHandle()
		{
			Monitor.Enter(this);
			Utils.FreeDSHandle(this.dsHandle, DirectoryContext.ADHandle);
			Utils.FreeAuthIdentity(this.authIdentity, DirectoryContext.ADHandle);
			Monitor.Exit(this);
		}

		// Token: 0x0600052D RID: 1325 RVA: 0x0001E250 File Offset: 0x0001D250
		internal ReplicationFailureCollection GetReplicationFailures(DS_REPL_INFO_TYPE type)
		{
			IntPtr intPtr = (IntPtr)0;
			bool flag = true;
			if (this.disposed)
			{
				throw new ObjectDisposedException(base.GetType().Name);
			}
			this.GetDSHandle();
			intPtr = base.GetReplicationInfoHelper(this.dsHandle, (int)type, (int)type, null, ref flag, 0, DirectoryContext.ADHandle);
			return base.ConstructFailures(intPtr, this, DirectoryContext.ADHandle);
		}

		// Token: 0x0600052E RID: 1326 RVA: 0x0001E2AC File Offset: 0x0001D2AC
		private ArrayList GetRoles()
		{
			ArrayList arrayList = new ArrayList();
			IntPtr zero = IntPtr.Zero;
			this.GetDSHandle();
			IntPtr intPtr = UnsafeNativeMethods.GetProcAddress(DirectoryContext.ADHandle, "DsListRolesW");
			if (intPtr == (IntPtr)0)
			{
				throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
			}
			NativeMethods.DsListRoles dsListRoles = (NativeMethods.DsListRoles)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(NativeMethods.DsListRoles));
			int num = dsListRoles(this.dsHandle, out zero);
			if (num == 0)
			{
				try
				{
					DsNameResult dsNameResult = new DsNameResult();
					Marshal.PtrToStructure(zero, dsNameResult);
					IntPtr intPtr2 = dsNameResult.items;
					for (int i = 0; i < dsNameResult.itemCount; i++)
					{
						DsNameResultItem dsNameResultItem = new DsNameResultItem();
						Marshal.PtrToStructure(intPtr2, dsNameResultItem);
						if (dsNameResultItem.status == 0 && dsNameResultItem.name.Equals(this.NtdsaObjectName))
						{
							arrayList.Add((ActiveDirectoryRole)i);
						}
						intPtr2 = Utils.AddToIntPtr(intPtr2, Marshal.SizeOf(dsNameResultItem));
					}
					return arrayList;
				}
				finally
				{
					if (zero != IntPtr.Zero)
					{
						intPtr = UnsafeNativeMethods.GetProcAddress(DirectoryContext.ADHandle, "DsFreeNameResultW");
						if (intPtr == (IntPtr)0)
						{
							throw ExceptionHelper.GetExceptionFromErrorCode(Marshal.GetLastWin32Error());
						}
						UnsafeNativeMethods.DsFreeNameResultW dsFreeNameResultW = (UnsafeNativeMethods.DsFreeNameResultW)Marshal.GetDelegateForFunctionPointer(intPtr, typeof(UnsafeNativeMethods.DsFreeNameResultW));
						dsFreeNameResultW(zero);
					}
				}
			}
			throw ExceptionHelper.GetExceptionFromErrorCode(num, base.Name);
		}

		// Token: 0x0600052F RID: 1327 RVA: 0x0001E414 File Offset: 0x0001D414
		private DateTime ParseDateTime(string dateTime)
		{
			int num = int.Parse(dateTime.Substring(0, 4), NumberFormatInfo.InvariantInfo);
			int num2 = int.Parse(dateTime.Substring(4, 2), NumberFormatInfo.InvariantInfo);
			int num3 = int.Parse(dateTime.Substring(6, 2), NumberFormatInfo.InvariantInfo);
			int num4 = int.Parse(dateTime.Substring(8, 2), NumberFormatInfo.InvariantInfo);
			int num5 = int.Parse(dateTime.Substring(10, 2), NumberFormatInfo.InvariantInfo);
			int num6 = int.Parse(dateTime.Substring(12, 2), NumberFormatInfo.InvariantInfo);
			return new DateTime(num, num2, num3, num4, num5, num6, 0);
		}

		// Token: 0x06000530 RID: 1328 RVA: 0x0001E4A8 File Offset: 0x0001D4A8
		[DirectoryServicesPermission(SecurityAction.Assert, Unrestricted = true)]
		private DirectorySearcher InternalGetDirectorySearcher()
		{
			DirectoryEntry directoryEntry = new DirectoryEntry("LDAP://" + base.Name);
			if (DirectoryContext.ServerBindSupported)
			{
				directoryEntry.AuthenticationType = Utils.DefaultAuthType | AuthenticationTypes.ServerBind;
			}
			else
			{
				directoryEntry.AuthenticationType = Utils.DefaultAuthType;
			}
			directoryEntry.Username = this.context.UserName;
			directoryEntry.Password = this.context.Password;
			return new DirectorySearcher(directoryEntry);
		}

		// Token: 0x04000424 RID: 1060
		private IntPtr dsHandle = IntPtr.Zero;

		// Token: 0x04000425 RID: 1061
		private IntPtr authIdentity = IntPtr.Zero;

		// Token: 0x04000426 RID: 1062
		private string[] becomeRoleOwnerAttrs;

		// Token: 0x04000427 RID: 1063
		private bool disposed;

		// Token: 0x04000428 RID: 1064
		private string cachedComputerObjectName;

		// Token: 0x04000429 RID: 1065
		private string cachedOSVersion;

		// Token: 0x0400042A RID: 1066
		private double cachedNumericOSVersion;

		// Token: 0x0400042B RID: 1067
		private Forest currentForest;

		// Token: 0x0400042C RID: 1068
		private Domain cachedDomain;

		// Token: 0x0400042D RID: 1069
		private ActiveDirectoryRoleCollection cachedRoles;

		// Token: 0x0400042E RID: 1070
		private bool dcInfoInitialized;

		// Token: 0x0400042F RID: 1071
		internal SyncUpdateCallback userDelegate;

		// Token: 0x04000430 RID: 1072
		internal SyncReplicaFromAllServersCallback syncAllFunctionPointer;
	}
}
