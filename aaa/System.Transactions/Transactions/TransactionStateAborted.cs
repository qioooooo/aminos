using System;
using System.Runtime.Serialization;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x0200001F RID: 31
	internal class TransactionStateAborted : TransactionStateEnded
	{
		// Token: 0x060000EE RID: 238 RVA: 0x0002B1A0 File Offset: 0x0002A5A0
		internal override void EnterState(InternalTransaction tx)
		{
			base.EnterState(tx);
			base.CommonEnterState(tx);
			for (int i = 0; i < tx.phase0Volatiles.volatileEnlistmentCount; i++)
			{
				tx.phase0Volatiles.volatileEnlistments[i].twoPhaseState.InternalAborted(tx.phase0Volatiles.volatileEnlistments[i]);
			}
			for (int j = 0; j < tx.phase1Volatiles.volatileEnlistmentCount; j++)
			{
				tx.phase1Volatiles.volatileEnlistments[j].twoPhaseState.InternalAborted(tx.phase1Volatiles.volatileEnlistments[j]);
			}
			if (tx.durableEnlistment != null)
			{
				tx.durableEnlistment.State.InternalAborted(tx.durableEnlistment);
			}
			TransactionManager.TransactionTable.Remove(tx);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
			{
				global::System.Transactions.Diagnostics.TransactionAbortedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), tx.TransactionTraceId);
			}
			tx.FireCompletion();
			if (tx.asyncCommit)
			{
				tx.SignalAsyncCompletion();
			}
		}

		// Token: 0x060000EF RID: 239 RVA: 0x0002B288 File Offset: 0x0002A688
		internal override TransactionStatus get_Status(InternalTransaction tx)
		{
			return TransactionStatus.Aborted;
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x0002B298 File Offset: 0x0002A698
		internal override void Rollback(InternalTransaction tx, Exception e)
		{
		}

		// Token: 0x060000F1 RID: 241 RVA: 0x0002B2A8 File Offset: 0x0002A6A8
		internal override void BeginCommit(InternalTransaction tx, bool asyncCommit, AsyncCallback asyncCallback, object asyncState)
		{
			throw this.CreateTransactionAbortedException(tx);
		}

		// Token: 0x060000F2 RID: 242 RVA: 0x0002B2BC File Offset: 0x0002A6BC
		internal override void EndCommit(InternalTransaction tx)
		{
			throw this.CreateTransactionAbortedException(tx);
		}

		// Token: 0x060000F3 RID: 243 RVA: 0x0002B2D0 File Offset: 0x0002A6D0
		internal override void RestartCommitIfNeeded(InternalTransaction tx)
		{
		}

		// Token: 0x060000F4 RID: 244 RVA: 0x0002B2E0 File Offset: 0x0002A6E0
		internal override void Timeout(InternalTransaction tx)
		{
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x0002B2F0 File Offset: 0x0002A6F0
		internal override void Phase0VolatilePrepareDone(InternalTransaction tx)
		{
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x0002B300 File Offset: 0x0002A700
		internal override void Phase1VolatilePrepareDone(InternalTransaction tx)
		{
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0002B310 File Offset: 0x0002A710
		internal override void ChangeStateTransactionAborted(InternalTransaction tx, Exception e)
		{
		}

		// Token: 0x060000F8 RID: 248 RVA: 0x0002B320 File Offset: 0x0002A720
		internal override void ChangeStatePromotedAborted(InternalTransaction tx)
		{
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x0002B330 File Offset: 0x0002A730
		internal override void ChangeStateAbortedDuringPromotion(InternalTransaction tx)
		{
		}

		// Token: 0x060000FA RID: 250 RVA: 0x0002B340 File Offset: 0x0002A740
		internal override void CreateBlockingClone(InternalTransaction tx)
		{
			throw this.CreateTransactionAbortedException(tx);
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0002B354 File Offset: 0x0002A754
		internal override void CreateAbortingClone(InternalTransaction tx)
		{
			throw this.CreateTransactionAbortedException(tx);
		}

		// Token: 0x060000FC RID: 252 RVA: 0x0002B368 File Offset: 0x0002A768
		internal override void GetObjectData(InternalTransaction tx, SerializationInfo serializationInfo, StreamingContext context)
		{
			throw this.CreateTransactionAbortedException(tx);
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0002B37C File Offset: 0x0002A77C
		internal override void CheckForFinishedTransaction(InternalTransaction tx)
		{
			throw this.CreateTransactionAbortedException(tx);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x0002B390 File Offset: 0x0002A790
		private TransactionException CreateTransactionAbortedException(InternalTransaction tx)
		{
			return TransactionAbortedException.Create(SR.GetString("TraceSourceLtm"), SR.GetString("TransactionAborted"), tx.innerException);
		}
	}
}
