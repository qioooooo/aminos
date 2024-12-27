using System;
using System.Collections;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000E4 RID: 228
	public class ReplicationOperation
	{
		// Token: 0x06000708 RID: 1800 RVA: 0x00025180 File Offset: 0x00024180
		internal ReplicationOperation(IntPtr addr, DirectoryServer server, Hashtable table)
		{
			DS_REPL_OP ds_REPL_OP = new DS_REPL_OP();
			Marshal.PtrToStructure(addr, ds_REPL_OP);
			this.timeEnqueued = DateTime.FromFileTime(ds_REPL_OP.ftimeEnqueued);
			this.serialNumber = ds_REPL_OP.ulSerialNumber;
			this.priority = ds_REPL_OP.ulPriority;
			this.operationType = ds_REPL_OP.OpType;
			this.namingContext = Marshal.PtrToStringUni(ds_REPL_OP.pszNamingContext);
			this.dsaDN = Marshal.PtrToStringUni(ds_REPL_OP.pszDsaDN);
			this.uuidDsaObjGuid = ds_REPL_OP.uuidDsaObjGuid;
			this.server = server;
			this.nameTable = table;
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000709 RID: 1801 RVA: 0x00025211 File Offset: 0x00024211
		public DateTime TimeEnqueued
		{
			get
			{
				return this.timeEnqueued;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x0600070A RID: 1802 RVA: 0x00025219 File Offset: 0x00024219
		public int OperationNumber
		{
			get
			{
				return this.serialNumber;
			}
		}

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x0600070B RID: 1803 RVA: 0x00025221 File Offset: 0x00024221
		public int Priority
		{
			get
			{
				return this.priority;
			}
		}

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x0600070C RID: 1804 RVA: 0x00025229 File Offset: 0x00024229
		public ReplicationOperationType OperationType
		{
			get
			{
				return this.operationType;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x0600070D RID: 1805 RVA: 0x00025231 File Offset: 0x00024231
		public string PartitionName
		{
			get
			{
				return this.namingContext;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x0600070E RID: 1806 RVA: 0x0002523C File Offset: 0x0002423C
		public string SourceServer
		{
			get
			{
				if (this.sourceServer == null)
				{
					if (this.nameTable.Contains(this.SourceServerGuid))
					{
						this.sourceServer = (string)this.nameTable[this.SourceServerGuid];
					}
					else if (this.dsaDN != null)
					{
						this.sourceServer = Utils.GetServerNameFromInvocationID(this.dsaDN, this.SourceServerGuid, this.server);
						this.nameTable.Add(this.SourceServerGuid, this.sourceServer);
					}
				}
				return this.sourceServer;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x0600070F RID: 1807 RVA: 0x000252D3 File Offset: 0x000242D3
		private Guid SourceServerGuid
		{
			get
			{
				return this.uuidDsaObjGuid;
			}
		}

		// Token: 0x040005A4 RID: 1444
		private DateTime timeEnqueued;

		// Token: 0x040005A5 RID: 1445
		private int serialNumber;

		// Token: 0x040005A6 RID: 1446
		private int priority;

		// Token: 0x040005A7 RID: 1447
		private ReplicationOperationType operationType;

		// Token: 0x040005A8 RID: 1448
		private string namingContext;

		// Token: 0x040005A9 RID: 1449
		private string dsaDN;

		// Token: 0x040005AA RID: 1450
		private Guid uuidDsaObjGuid;

		// Token: 0x040005AB RID: 1451
		private DirectoryServer server;

		// Token: 0x040005AC RID: 1452
		private string sourceServer;

		// Token: 0x040005AD RID: 1453
		private Hashtable nameTable;
	}
}
