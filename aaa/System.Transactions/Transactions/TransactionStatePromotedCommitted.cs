using System;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x0200002E RID: 46
	internal class TransactionStatePromotedCommitted : TransactionStatePromotedEnded
	{
		// Token: 0x06000179 RID: 377 RVA: 0x0002CC24 File Offset: 0x0002C024
		internal override void EnterState(InternalTransaction tx)
		{
			base.EnterState(tx);
			if (tx.phase1Volatiles.VolatileDemux != null)
			{
				tx.phase1Volatiles.VolatileDemux.BroadcastCommitted(ref tx.phase1Volatiles);
			}
			if (tx.phase0Volatiles.VolatileDemux != null)
			{
				tx.phase0Volatiles.VolatileDemux.BroadcastCommitted(ref tx.phase0Volatiles);
			}
			tx.FireCompletion();
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.TransactionCommittedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), tx.TransactionTraceId);
			}
		}

		// Token: 0x0600017A RID: 378 RVA: 0x0002CCA0 File Offset: 0x0002C0A0
		internal override TransactionStatus get_Status(InternalTransaction tx)
		{
			return TransactionStatus.Committed;
		}

		// Token: 0x0600017B RID: 379 RVA: 0x0002CCB0 File Offset: 0x0002C0B0
		internal override void ChangeStatePromotedCommitted(InternalTransaction tx)
		{
		}

		// Token: 0x0600017C RID: 380 RVA: 0x0002CCC0 File Offset: 0x0002C0C0
		protected override void PromotedTransactionOutcome(InternalTransaction tx)
		{
		}

		// Token: 0x0600017D RID: 381 RVA: 0x0002CCD0 File Offset: 0x0002C0D0
		internal override void InDoubtFromDtc(InternalTransaction tx)
		{
		}

		// Token: 0x0600017E RID: 382 RVA: 0x0002CCE0 File Offset: 0x0002C0E0
		internal override void InDoubtFromEnlistment(InternalTransaction tx)
		{
		}
	}
}
