using System;

namespace System.Transactions
{
	// Token: 0x02000031 RID: 49
	internal class TransactionStateDelegated : TransactionStateDelegatedBase
	{
		// Token: 0x0600018F RID: 399 RVA: 0x0002D068 File Offset: 0x0002C468
		internal override void BeginCommit(InternalTransaction tx, bool asyncCommit, AsyncCallback asyncCallback, object asyncState)
		{
			tx.asyncCommit = asyncCommit;
			tx.asyncCallback = asyncCallback;
			tx.asyncState = asyncState;
			TransactionState._TransactionStateDelegatedCommitting.EnterState(tx);
		}

		// Token: 0x06000190 RID: 400 RVA: 0x0002D098 File Offset: 0x0002C498
		internal override bool PromoteDurable(InternalTransaction tx)
		{
			tx.durableEnlistment.State.ChangeStateDelegated(tx.durableEnlistment);
			return true;
		}

		// Token: 0x06000191 RID: 401 RVA: 0x0002D0BC File Offset: 0x0002C4BC
		internal override void RestartCommitIfNeeded(InternalTransaction tx)
		{
			TransactionState._TransactionStateDelegatedP0Wave.EnterState(tx);
		}

		// Token: 0x06000192 RID: 402 RVA: 0x0002D0D4 File Offset: 0x0002C4D4
		internal override void Rollback(InternalTransaction tx, Exception e)
		{
			if (tx.innerException == null)
			{
				tx.innerException = e;
			}
			TransactionState._TransactionStateDelegatedAborting.EnterState(tx);
		}
	}
}
