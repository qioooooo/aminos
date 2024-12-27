using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000E6 RID: 230
	public class ReplicationOperationInformation
	{
		// Token: 0x170001BE RID: 446
		// (get) Token: 0x06000719 RID: 1817 RVA: 0x00025410 File Offset: 0x00024410
		public DateTime OperationStartTime
		{
			get
			{
				return this.startTime;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600071A RID: 1818 RVA: 0x00025418 File Offset: 0x00024418
		public ReplicationOperation CurrentOperation
		{
			get
			{
				return this.currentOp;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x0600071B RID: 1819 RVA: 0x00025420 File Offset: 0x00024420
		public ReplicationOperationCollection PendingOperations
		{
			get
			{
				return this.collection;
			}
		}

		// Token: 0x040005B0 RID: 1456
		internal DateTime startTime;

		// Token: 0x040005B1 RID: 1457
		internal ReplicationOperation currentOp;

		// Token: 0x040005B2 RID: 1458
		internal ReplicationOperationCollection collection;
	}
}
