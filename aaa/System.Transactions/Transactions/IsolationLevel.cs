using System;

namespace System.Transactions
{
	// Token: 0x02000008 RID: 8
	public enum IsolationLevel
	{
		// Token: 0x0400007B RID: 123
		Serializable,
		// Token: 0x0400007C RID: 124
		RepeatableRead,
		// Token: 0x0400007D RID: 125
		ReadCommitted,
		// Token: 0x0400007E RID: 126
		ReadUncommitted,
		// Token: 0x0400007F RID: 127
		Snapshot,
		// Token: 0x04000080 RID: 128
		Chaos,
		// Token: 0x04000081 RID: 129
		Unspecified
	}
}
