using System;
using System.Runtime.Serialization;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x0200002D RID: 45
	internal class TransactionStatePromotedAborted : TransactionStatePromotedEnded
	{
		// Token: 0x06000166 RID: 358 RVA: 0x0002C9E8 File Offset: 0x0002BDE8
		internal override void EnterState(InternalTransaction tx)
		{
			base.EnterState(tx);
			if (tx.phase1Volatiles.VolatileDemux != null)
			{
				tx.phase1Volatiles.VolatileDemux.BroadcastRollback(ref tx.phase1Volatiles);
			}
			if (tx.phase0Volatiles.VolatileDemux != null)
			{
				tx.phase0Volatiles.VolatileDemux.BroadcastRollback(ref tx.phase0Volatiles);
			}
			tx.FireCompletion();
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
			{
				global::System.Transactions.Diagnostics.TransactionAbortedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), tx.TransactionTraceId);
			}
		}

		// Token: 0x06000167 RID: 359 RVA: 0x0002CA64 File Offset: 0x0002BE64
		internal override TransactionStatus get_Status(InternalTransaction tx)
		{
			return TransactionStatus.Aborted;
		}

		// Token: 0x06000168 RID: 360 RVA: 0x0002CA74 File Offset: 0x0002BE74
		internal override void Rollback(InternalTransaction tx, Exception e)
		{
		}

		// Token: 0x06000169 RID: 361 RVA: 0x0002CA84 File Offset: 0x0002BE84
		internal override void BeginCommit(InternalTransaction tx, bool asyncCommit, AsyncCallback asyncCallback, object asyncState)
		{
			throw TransactionAbortedException.Create(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x0600016A RID: 362 RVA: 0x0002CAA8 File Offset: 0x0002BEA8
		internal override void CreateBlockingClone(InternalTransaction tx)
		{
			throw TransactionAbortedException.Create(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x0600016B RID: 363 RVA: 0x0002CACC File Offset: 0x0002BECC
		internal override void CreateAbortingClone(InternalTransaction tx)
		{
			throw TransactionAbortedException.Create(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x0600016C RID: 364 RVA: 0x0002CAF0 File Offset: 0x0002BEF0
		internal override void RestartCommitIfNeeded(InternalTransaction tx)
		{
		}

		// Token: 0x0600016D RID: 365 RVA: 0x0002CB00 File Offset: 0x0002BF00
		internal override void Phase0VolatilePrepareDone(InternalTransaction tx)
		{
		}

		// Token: 0x0600016E RID: 366 RVA: 0x0002CB10 File Offset: 0x0002BF10
		internal override void Phase1VolatilePrepareDone(InternalTransaction tx)
		{
		}

		// Token: 0x0600016F RID: 367 RVA: 0x0002CB20 File Offset: 0x0002BF20
		internal override void ChangeStatePromotedPhase0(InternalTransaction tx)
		{
			throw new TransactionAbortedException(tx.innerException);
		}

		// Token: 0x06000170 RID: 368 RVA: 0x0002CB38 File Offset: 0x0002BF38
		internal override void ChangeStatePromotedPhase1(InternalTransaction tx)
		{
			throw new TransactionAbortedException(tx.innerException);
		}

		// Token: 0x06000171 RID: 369 RVA: 0x0002CB50 File Offset: 0x0002BF50
		internal override void ChangeStatePromotedAborted(InternalTransaction tx)
		{
		}

		// Token: 0x06000172 RID: 370 RVA: 0x0002CB60 File Offset: 0x0002BF60
		internal override void ChangeStateTransactionAborted(InternalTransaction tx, Exception e)
		{
		}

		// Token: 0x06000173 RID: 371 RVA: 0x0002CB70 File Offset: 0x0002BF70
		protected override void PromotedTransactionOutcome(InternalTransaction tx)
		{
			if (tx.innerException == null && tx.PromotedTransaction != null)
			{
				tx.innerException = tx.PromotedTransaction.InnerException;
			}
			throw TransactionAbortedException.Create(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x06000174 RID: 372 RVA: 0x0002CBB4 File Offset: 0x0002BFB4
		internal override void CheckForFinishedTransaction(InternalTransaction tx)
		{
			throw new TransactionAbortedException(tx.innerException);
		}

		// Token: 0x06000175 RID: 373 RVA: 0x0002CBCC File Offset: 0x0002BFCC
		internal override void GetObjectData(InternalTransaction tx, SerializationInfo serializationInfo, StreamingContext context)
		{
			throw TransactionAbortedException.Create(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x06000176 RID: 374 RVA: 0x0002CBF0 File Offset: 0x0002BFF0
		internal override void InDoubtFromDtc(InternalTransaction tx)
		{
		}

		// Token: 0x06000177 RID: 375 RVA: 0x0002CC00 File Offset: 0x0002C000
		internal override void InDoubtFromEnlistment(InternalTransaction tx)
		{
		}
	}
}
