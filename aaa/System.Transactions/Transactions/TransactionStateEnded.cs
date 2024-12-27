using System;
using System.Threading;

namespace System.Transactions
{
	// Token: 0x0200001E RID: 30
	internal abstract class TransactionStateEnded : TransactionState
	{
		// Token: 0x060000EA RID: 234 RVA: 0x0002B12C File Offset: 0x0002A52C
		internal override void EnterState(InternalTransaction tx)
		{
			if (tx.needPulse)
			{
				Monitor.Pulse(tx);
			}
		}

		// Token: 0x060000EB RID: 235 RVA: 0x0002B148 File Offset: 0x0002A548
		internal override void AddOutcomeRegistrant(InternalTransaction tx, TransactionCompletedEventHandler transactionCompletedDelegate)
		{
			if (transactionCompletedDelegate != null)
			{
				TransactionEventArgs transactionEventArgs = new TransactionEventArgs();
				transactionEventArgs.transaction = tx.outcomeSource.InternalClone();
				transactionCompletedDelegate(transactionEventArgs.transaction, transactionEventArgs);
			}
		}

		// Token: 0x060000EC RID: 236 RVA: 0x0002B17C File Offset: 0x0002A57C
		internal override bool IsCompleted(InternalTransaction tx)
		{
			return true;
		}
	}
}
