using System;

namespace System.Transactions
{
	// Token: 0x0200005C RID: 92
	public interface IPromotableSinglePhaseNotification : ITransactionPromoter
	{
		// Token: 0x060002A3 RID: 675
		void Initialize();

		// Token: 0x060002A4 RID: 676
		void SinglePhaseCommit(SinglePhaseEnlistment singlePhaseEnlistment);

		// Token: 0x060002A5 RID: 677
		void Rollback(SinglePhaseEnlistment singlePhaseEnlistment);
	}
}
