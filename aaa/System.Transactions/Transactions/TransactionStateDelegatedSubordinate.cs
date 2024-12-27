using System;

namespace System.Transactions
{
	// Token: 0x02000032 RID: 50
	internal class TransactionStateDelegatedSubordinate : TransactionStateDelegatedBase
	{
		// Token: 0x06000194 RID: 404 RVA: 0x0002D110 File Offset: 0x0002C510
		internal override bool PromoteDurable(InternalTransaction tx)
		{
			return true;
		}

		// Token: 0x06000195 RID: 405 RVA: 0x0002D120 File Offset: 0x0002C520
		internal override void Rollback(InternalTransaction tx, Exception e)
		{
			if (tx.innerException == null)
			{
				tx.innerException = e;
			}
			tx.PromotedTransaction.Rollback();
			TransactionState._TransactionStatePromotedAborted.EnterState(tx);
		}

		// Token: 0x06000196 RID: 406 RVA: 0x0002D154 File Offset: 0x0002C554
		internal override void ChangeStatePromotedPhase0(InternalTransaction tx)
		{
			TransactionState._TransactionStatePromotedPhase0.EnterState(tx);
		}

		// Token: 0x06000197 RID: 407 RVA: 0x0002D16C File Offset: 0x0002C56C
		internal override void ChangeStatePromotedPhase1(InternalTransaction tx)
		{
			TransactionState._TransactionStatePromotedPhase1.EnterState(tx);
		}
	}
}
