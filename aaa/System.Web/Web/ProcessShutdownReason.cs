using System;

namespace System.Web
{
	// Token: 0x020000BE RID: 190
	public enum ProcessShutdownReason
	{
		// Token: 0x040011F0 RID: 4592
		None,
		// Token: 0x040011F1 RID: 4593
		Unexpected,
		// Token: 0x040011F2 RID: 4594
		RequestsLimit,
		// Token: 0x040011F3 RID: 4595
		RequestQueueLimit,
		// Token: 0x040011F4 RID: 4596
		Timeout,
		// Token: 0x040011F5 RID: 4597
		IdleTimeout,
		// Token: 0x040011F6 RID: 4598
		MemoryLimitExceeded,
		// Token: 0x040011F7 RID: 4599
		PingFailed,
		// Token: 0x040011F8 RID: 4600
		DeadlockSuspected
	}
}
