using System;

namespace System.Transactions.Diagnostics
{
	// Token: 0x020000A4 RID: 164
	internal enum EnlistmentCallback
	{
		// Token: 0x04000280 RID: 640
		Done,
		// Token: 0x04000281 RID: 641
		Prepared,
		// Token: 0x04000282 RID: 642
		ForceRollback,
		// Token: 0x04000283 RID: 643
		Committed,
		// Token: 0x04000284 RID: 644
		Aborted,
		// Token: 0x04000285 RID: 645
		InDoubt
	}
}
