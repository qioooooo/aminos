using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000E1 RID: 225
	public class ReplicationNeighbor
	{
		// Token: 0x060006F4 RID: 1780 RVA: 0x00024E84 File Offset: 0x00023E84
		internal ReplicationNeighbor(IntPtr addr, DirectoryServer server, Hashtable table)
		{
			DS_REPL_NEIGHBOR ds_REPL_NEIGHBOR = new DS_REPL_NEIGHBOR();
			Marshal.PtrToStructure(addr, ds_REPL_NEIGHBOR);
			this.namingContext = Marshal.PtrToStringUni(ds_REPL_NEIGHBOR.pszNamingContext);
			this.sourceServerDN = Marshal.PtrToStringUni(ds_REPL_NEIGHBOR.pszSourceDsaDN);
			string text = Marshal.PtrToStringUni(ds_REPL_NEIGHBOR.pszAsyncIntersiteTransportDN);
			if (text != null)
			{
				string rdnFromDN = Utils.GetRdnFromDN(text);
				string value = Utils.GetDNComponents(rdnFromDN)[0].Value;
				if (string.Compare(value, "SMTP", StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.transportType = ActiveDirectoryTransportType.Smtp;
				}
				else
				{
					this.transportType = ActiveDirectoryTransportType.Rpc;
				}
			}
			this.replicaFlags = (ReplicationNeighbor.ReplicationNeighborOptions)ds_REPL_NEIGHBOR.dwReplicaFlags;
			this.uuidSourceDsaInvocationID = ds_REPL_NEIGHBOR.uuidSourceDsaInvocationID;
			this.usnLastObjChangeSynced = ds_REPL_NEIGHBOR.usnLastObjChangeSynced;
			this.usnAttributeFilter = ds_REPL_NEIGHBOR.usnAttributeFilter;
			this.timeLastSyncSuccess = DateTime.FromFileTime(ds_REPL_NEIGHBOR.ftimeLastSyncSuccess);
			this.timeLastSyncAttempt = DateTime.FromFileTime(ds_REPL_NEIGHBOR.ftimeLastSyncAttempt);
			this.lastSyncResult = ds_REPL_NEIGHBOR.dwLastSyncResult;
			this.consecutiveSyncFailures = ds_REPL_NEIGHBOR.cNumConsecutiveSyncFailures;
			this.server = server;
			this.nameTable = table;
		}

		// Token: 0x170001A9 RID: 425
		// (get) Token: 0x060006F5 RID: 1781 RVA: 0x00024F85 File Offset: 0x00023F85
		public string PartitionName
		{
			get
			{
				return this.namingContext;
			}
		}

		// Token: 0x170001AA RID: 426
		// (get) Token: 0x060006F6 RID: 1782 RVA: 0x00024F90 File Offset: 0x00023F90
		public string SourceServer
		{
			get
			{
				if (this.sourceServer == null)
				{
					if (this.nameTable.Contains(this.SourceInvocationId))
					{
						this.sourceServer = (string)this.nameTable[this.SourceInvocationId];
					}
					else if (this.sourceServerDN != null)
					{
						this.sourceServer = Utils.GetServerNameFromInvocationID(this.sourceServerDN, this.SourceInvocationId, this.server);
						this.nameTable.Add(this.SourceInvocationId, this.sourceServer);
					}
				}
				return this.sourceServer;
			}
		}

		// Token: 0x170001AB RID: 427
		// (get) Token: 0x060006F7 RID: 1783 RVA: 0x00025027 File Offset: 0x00024027
		public ActiveDirectoryTransportType TransportType
		{
			get
			{
				return this.transportType;
			}
		}

		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060006F8 RID: 1784 RVA: 0x0002502F File Offset: 0x0002402F
		public ReplicationNeighbor.ReplicationNeighborOptions ReplicationNeighborOption
		{
			get
			{
				return this.replicaFlags;
			}
		}

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060006F9 RID: 1785 RVA: 0x00025037 File Offset: 0x00024037
		public Guid SourceInvocationId
		{
			get
			{
				return this.uuidSourceDsaInvocationID;
			}
		}

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060006FA RID: 1786 RVA: 0x0002503F File Offset: 0x0002403F
		public long UsnLastObjectChangeSynced
		{
			get
			{
				return this.usnLastObjChangeSynced;
			}
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060006FB RID: 1787 RVA: 0x00025047 File Offset: 0x00024047
		public long UsnAttributeFilter
		{
			get
			{
				return this.usnAttributeFilter;
			}
		}

		// Token: 0x170001B0 RID: 432
		// (get) Token: 0x060006FC RID: 1788 RVA: 0x0002504F File Offset: 0x0002404F
		public DateTime LastSuccessfulSync
		{
			get
			{
				return this.timeLastSyncSuccess;
			}
		}

		// Token: 0x170001B1 RID: 433
		// (get) Token: 0x060006FD RID: 1789 RVA: 0x00025057 File Offset: 0x00024057
		public DateTime LastAttemptedSync
		{
			get
			{
				return this.timeLastSyncAttempt;
			}
		}

		// Token: 0x170001B2 RID: 434
		// (get) Token: 0x060006FE RID: 1790 RVA: 0x0002505F File Offset: 0x0002405F
		public int LastSyncResult
		{
			get
			{
				return this.lastSyncResult;
			}
		}

		// Token: 0x170001B3 RID: 435
		// (get) Token: 0x060006FF RID: 1791 RVA: 0x00025067 File Offset: 0x00024067
		public string LastSyncMessage
		{
			get
			{
				return ExceptionHelper.GetErrorMessage(this.lastSyncResult, false);
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000700 RID: 1792 RVA: 0x00025075 File Offset: 0x00024075
		public int ConsecutiveFailureCount
		{
			get
			{
				return this.consecutiveSyncFailures;
			}
		}

		// Token: 0x04000584 RID: 1412
		private string namingContext;

		// Token: 0x04000585 RID: 1413
		private string sourceServerDN;

		// Token: 0x04000586 RID: 1414
		private ActiveDirectoryTransportType transportType;

		// Token: 0x04000587 RID: 1415
		private ReplicationNeighbor.ReplicationNeighborOptions replicaFlags;

		// Token: 0x04000588 RID: 1416
		private Guid uuidSourceDsaInvocationID;

		// Token: 0x04000589 RID: 1417
		private long usnLastObjChangeSynced;

		// Token: 0x0400058A RID: 1418
		private long usnAttributeFilter;

		// Token: 0x0400058B RID: 1419
		private DateTime timeLastSyncSuccess;

		// Token: 0x0400058C RID: 1420
		private DateTime timeLastSyncAttempt;

		// Token: 0x0400058D RID: 1421
		private int lastSyncResult;

		// Token: 0x0400058E RID: 1422
		private int consecutiveSyncFailures;

		// Token: 0x0400058F RID: 1423
		private DirectoryServer server;

		// Token: 0x04000590 RID: 1424
		private string sourceServer;

		// Token: 0x04000591 RID: 1425
		private Hashtable nameTable;

		// Token: 0x020000E2 RID: 226
		[Flags]
		public enum ReplicationNeighborOptions : long
		{
			// Token: 0x04000593 RID: 1427
			Writeable = 16L,
			// Token: 0x04000594 RID: 1428
			SyncOnStartup = 32L,
			// Token: 0x04000595 RID: 1429
			ScheduledSync = 64L,
			// Token: 0x04000596 RID: 1430
			UseInterSiteTransport = 128L,
			// Token: 0x04000597 RID: 1431
			TwoWaySync = 512L,
			// Token: 0x04000598 RID: 1432
			ReturnObjectParent = 2048L,
			// Token: 0x04000599 RID: 1433
			FullSyncInProgress = 65536L,
			// Token: 0x0400059A RID: 1434
			FullSyncNextPacket = 131072L,
			// Token: 0x0400059B RID: 1435
			NeverSynced = 2097152L,
			// Token: 0x0400059C RID: 1436
			Preempted = 16777216L,
			// Token: 0x0400059D RID: 1437
			IgnoreChangeNotifications = 67108864L,
			// Token: 0x0400059E RID: 1438
			DisableScheduledSync = 134217728L,
			// Token: 0x0400059F RID: 1439
			CompressChanges = 268435456L,
			// Token: 0x040005A0 RID: 1440
			NoChangeNotifications = 536870912L,
			// Token: 0x040005A1 RID: 1441
			PartialAttributeSet = 1073741824L
		}
	}
}
