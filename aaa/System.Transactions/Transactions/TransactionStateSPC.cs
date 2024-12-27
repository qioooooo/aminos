using System;

namespace System.Transactions
{
	// Token: 0x0200001D RID: 29
	internal class TransactionStateSPC : ActiveStates
	{
		// Token: 0x060000E5 RID: 229 RVA: 0x0002B080 File Offset: 0x0002A480
		internal override void EnterState(InternalTransaction tx)
		{
			base.CommonEnterState(tx);
			if (tx.durableEnlistment != null)
			{
				tx.durableEnlistment.State.ChangeStateCommitting(tx.durableEnlistment);
				return;
			}
			TransactionState._TransactionStateCommitted.EnterState(tx);
		}

		// Token: 0x060000E6 RID: 230 RVA: 0x0002B0C0 File Offset: 0x0002A4C0
		internal override void ChangeStateTransactionCommitted(InternalTransaction tx)
		{
			TransactionState._TransactionStateCommitted.EnterState(tx);
		}

		// Token: 0x060000E7 RID: 231 RVA: 0x0002B0D8 File Offset: 0x0002A4D8
		internal override void InDoubtFromEnlistment(InternalTransaction tx)
		{
			TransactionState._TransactionStateInDoubt.EnterState(tx);
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x0002B0F0 File Offset: 0x0002A4F0
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
