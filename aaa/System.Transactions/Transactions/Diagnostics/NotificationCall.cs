using System;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000A3 RID: 163
	internal enum NotificationCall
	{
		// Token: 0x04000279 RID: 633
		Prepare,
		// Token: 0x0400027A RID: 634
		Commit,
		// Token: 0x0400027B RID: 635
		Rollback,
		// Token: 0x0400027C RID: 636
		InDoubt,
		// Token: 0x0400027D RID: 637
		SinglePhaseCommit,
		// Token: 0x0400027E RID: 638
		Promote
	}
}
