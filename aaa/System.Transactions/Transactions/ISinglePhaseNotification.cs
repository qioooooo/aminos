using System;

namespace System.Transactions
{
	// Token: 0x0200005F RID: 95
	public interface ISinglePhaseNotification : IEnlistmentNotification
	{
		// Token: 0x060002A8 RID: 680
		void SinglePhaseCommit(SinglePhaseEnlistment singlePhaseEnlistment);
	}
}
