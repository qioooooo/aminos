using System;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000020 RID: 32
	internal class TransactionStateCommitted : TransactionStateEnded
	{
		// Token: 0x06000100 RID: 256 RVA: 0x0002B3D0 File Offset: 0x0002A7D0
		internal override void EnterState(InternalTransaction tx)
		{
			base.EnterState(tx);
			base.CommonEnterState(tx);
			for (int i = 0; i < tx.phase0Volatiles.volatileEnlistmentCount; i++)
			{
				tx.phase0Volatiles.volatileEnlistments[i].twoPhaseState.InternalCommitted(tx.phase0Volatiles.volatileEnlistments[i]);
			}
			for (int j = 0; j < tx.phase1Volatiles.volatileEnlistmentCount; j++)
			{
				tx.phase1Volatiles.volatileEnlistments[j].twoPhaseState.InternalCommitted(tx.phase1Volatiles.volatileEnlistments[j]);
			}
			TransactionManager.TransactionTable.Remove(tx);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.TransactionCommittedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), tx.TransactionTraceId);
			}
			tx.FireCompletion();
			if (tx.asyncCommit)
			{
				tx.SignalAsyncCompletion();
			}
		}

		// Token: 0x06000101 RID: 257 RVA: 0x0002B49C File Offset: 0x0002A89C
		internal override TransactionStatus get_Status(InternalTransaction tx)
		{
			return TransactionStatus.Committed;
		}

		// Token: 0x06000102 RID: 258 RVA: 0x0002B4AC File Offset: 0x0002A8AC
		internal override void Rollback(InternalTransaction tx, Exception e)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x06000103 RID: 259 RVA: 0x0002B4D0 File Offset: 0x0002A8D0
		internal override void EndCommit(InternalTransaction tx)
		{
		}
	}
}
