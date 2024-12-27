using System;

namespace System.Transactions
{
	// Token: 0x02000038 RID: 56
	internal interface IEnlistmentNotificationInternal
	{
		// Token: 0x060001B3 RID: 435
		void Prepare(IPromotedEnlistment preparingEnlistment);

		// Token: 0x060001B4 RID: 436
		void Commit(IPromotedEnlistment enlistment);

		// Token: 0x060001B5 RID: 437
		void Rollback(IPromotedEnlistment enlistment);

		// Token: 0x060001B6 RID: 438
		void InDoubt(IPromotedEnlistment enlistment);
	}
}
