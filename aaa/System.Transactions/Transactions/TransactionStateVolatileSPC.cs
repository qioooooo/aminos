using System;

namespace System.Transactions
{
	// Token: 0x0200001C RID: 28
	internal class TransactionStateVolatileSPC : ActiveStates
	{
		// Token: 0x060000E0 RID: 224 RVA: 0x0002AFDC File Offset: 0x0002A3DC
		internal override void EnterState(InternalTransaction tx)
		{
			base.CommonEnterState(tx);
			tx.phase1Volatiles.volatileEnlistments[0].twoPhaseState.ChangeStateSinglePhaseCommit(tx.phase1Volatiles.volatileEnlistments[0]);
		}

		// Token: 0x060000E1 RID: 225 RVA: 0x0002B014 File Offset: 0x0002A414
		internal override void ChangeStateTransactionCommitted(InternalTransaction tx)
		{
			TransactionState._TransactionStateCommitted.EnterState(tx);
		}

		// Token: 0x060000E2 RID: 226 RVA: 0x0002B02C File Offset: 0x0002A42C
		internal override void InDoubtFromEnlistment(InternalTransaction tx)
		{
			TransactionState._TransactionStateInDoubt.EnterState(tx);
		}

		// Token: 0x060000E3 RID: 227 RVA: 0x0002B044 File Offset: 0x0002A444
		internal override void ChangeStateTransactionAborted(InternalTransaction tx, Exception e)
		{
			if (tx.innerException == null)
			{
				tx.innerException = e;
			}
			TransactionState._TransactionStateAborted.EnterState(tx);
		}
	}
}
