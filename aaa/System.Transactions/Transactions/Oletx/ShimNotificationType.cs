using System;

namespace System.Transactions.Oletx
{
	// Token: 0x02000078 RID: 120
	internal enum ShimNotificationType
	{
		// Token: 0x04000174 RID: 372
		None,
		// Token: 0x04000175 RID: 373
		Phase0RequestNotify,
		// Token: 0x04000176 RID: 374
		VoteRequestNotify,
		// Token: 0x04000177 RID: 375
		PrepareRequestNotify,
		// Token: 0x04000178 RID: 376
		CommitRequestNotify,
		// Token: 0x04000179 RID: 377
		AbortRequestNotify,
		// Token: 0x0400017A RID: 378
		CommittedNotify,
		// Token: 0x0400017B RID: 379
		AbortedNotify,
		// Token: 0x0400017C RID: 380
		InDoubtNotify,
		// Token: 0x0400017D RID: 381
		EnlistmentTmDownNotify,
		// Token: 0x0400017E RID: 382
		ResourceManagerTmDownNotify
	}
}
