using System;

namespace System.Transactions
{
	// Token: 0x02000039 RID: 57
	internal interface ISinglePhaseNotificationInternal : IEnlistmentNotificationInternal
	{
		// Token: 0x060001B7 RID: 439
		void SinglePhaseCommit(IPromotedEnlistment singlePhaseEnlistment);
	}
}
