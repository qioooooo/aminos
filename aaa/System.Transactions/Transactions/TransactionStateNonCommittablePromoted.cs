using System;

namespace System.Transactions
{
	// Token: 0x02000023 RID: 35
	internal class TransactionStateNonCommittablePromoted : TransactionStatePromotedBase
	{
		// Token: 0x06000127 RID: 295 RVA: 0x0002BCE4 File Offset: 0x0002B0E4
		internal override void EnterState(InternalTransaction tx)
		{
			base.CommonEnterState(tx);
			tx.PromotedTransaction.realOletxTransaction.InternalTransaction = tx;
		}
	}
}
