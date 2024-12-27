using System;
using System.Runtime.Serialization;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x0200002F RID: 47
	internal class TransactionStatePromotedIndoubt : TransactionStatePromotedEnded
	{
		// Token: 0x06000180 RID: 384 RVA: 0x0002CD04 File Offset: 0x0002C104
		internal override void EnterState(InternalTransaction tx)
		{
			base.EnterState(tx);
			if (tx.phase1Volatiles.VolatileDemux != null)
			{
				tx.phase1Volatiles.VolatileDemux.BroadcastInDoubt(ref tx.phase1Volatiles);
			}
			if (tx.phase0Volatiles.VolatileDemux != null)
			{
				tx.phase0Volatiles.VolatileDemux.BroadcastInDoubt(ref tx.phase0Volatiles);
			}
			tx.FireCompletion();
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
			{
				global::System.Transactions.Diagnostics.TransactionInDoubtTraceRecord.Trace(SR.GetString("TraceSourceLtm"), tx.TransactionTraceId);
			}
		}

		// Token: 0x06000181 RID: 385 RVA: 0x0002CD80 File Offset: 0x0002C180
		internal override TransactionStatus get_Status(InternalTransaction tx)
		{
			return TransactionStatus.InDoubt;
		}

		// Token: 0x06000182 RID: 386 RVA: 0x0002CD90 File Offset: 0x0002C190
		internal override void RestartCommitIfNeeded(InternalTransaction tx)
		{
		}

		// Token: 0x06000183 RID: 387 RVA: 0x0002CDA0 File Offset: 0x0002C1A0
		internal override void ChangeStatePromotedPhase0(InternalTransaction tx)
		{
			throw TransactionInDoubtException.Create(SR.GetString("TraceSourceBase"), tx.innerException);
		}

		// Token: 0x06000184 RID: 388 RVA: 0x0002CDC4 File Offset: 0x0002C1C4
		internal override void ChangeStatePromotedPhase1(InternalTransaction tx)
		{
			throw TransactionInDoubtException.Create(SR.GetString("TraceSourceBase"), tx.innerException);
		}

		// Token: 0x06000185 RID: 389 RVA: 0x0002CDE8 File Offset: 0x0002C1E8
		internal override void InDoubtFromDtc(InternalTransaction tx)
		{
		}

		// Token: 0x06000186 RID: 390 RVA: 0x0002CDF8 File Offset: 0x0002C1F8
		internal override void InDoubtFromEnlistment(InternalTransaction tx)
		{
		}

		// Token: 0x06000187 RID: 391 RVA: 0x0002CE08 File Offset: 0x0002C208
		protected override void PromotedTransactionOutcome(InternalTransaction tx)
		{
			if (tx.innerException == null && tx.PromotedTransaction != null)
			{
				tx.innerException = tx.PromotedTransaction.InnerException;
			}
			throw TransactionInDoubtException.Create(SR.GetString("TraceSourceBase"), tx.innerException);
		}

		// Token: 0x06000188 RID: 392 RVA: 0x0002CE4C File Offset: 0x0002C24C
		internal override void CheckForFinishedTransaction(InternalTransaction tx)
		{
			throw TransactionInDoubtException.Create(SR.GetString("TraceSourceBase"), tx.innerException);
		}

		// Token: 0x06000189 RID: 393 RVA: 0x0002CE70 File Offset: 0x0002C270
		internal override void GetObjectData(InternalTransaction tx, SerializationInfo serializationInfo, StreamingContext context)
		{
			throw TransactionInDoubtException.Create(SR.GetString("TraceSourceBase"), tx.innerException);
		}

		// Token: 0x0600018A RID: 394 RVA: 0x0002CE94 File Offset: 0x0002C294
		internal override void ChangeStatePromotedAborted(InternalTransaction tx)
		{
		}

		// Token: 0x0600018B RID: 395 RVA: 0x0002CEA4 File Offset: 0x0002C2A4
		internal override void ChangeStatePromotedCommitted(InternalTransaction tx)
		{
		}
	}
}
