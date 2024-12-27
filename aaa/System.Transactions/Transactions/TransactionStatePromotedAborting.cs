using System;

namespace System.Transactions
{
	// Token: 0x02000029 RID: 41
	internal abstract class TransactionStatePromotedAborting : TransactionStatePromotedBase
	{
		// Token: 0x0600014A RID: 330 RVA: 0x0002C608 File Offset: 0x0002BA08
		internal override void EnterState(InternalTransaction tx)
		{
			base.CommonEnterState(tx);
		}

		// Token: 0x0600014B RID: 331 RVA: 0x0002C61C File Offset: 0x0002BA1C
		internal override TransactionStatus get_Status(InternalTransaction tx)
		{
			return TransactionStatus.Aborted;
		}

		// Token: 0x0600014C RID: 332 RVA: 0x0002C62C File Offset: 0x0002BA2C
		internal override void BeginCommit(InternalTransaction tx, bool asyncCommit, AsyncCallback asyncCallback, object asyncState)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x0600014D RID: 333 RVA: 0x0002C650 File Offset: 0x0002BA50
		internal override void CreateBlockingClone(InternalTransaction tx)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x0600014E RID: 334 RVA: 0x0002C674 File Offset: 0x0002BA74
		internal override void CreateAbortingClone(InternalTransaction tx)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x0600014F RID: 335 RVA: 0x0002C698 File Offset: 0x0002BA98
		internal override void ChangeStatePromotedAborted(InternalTransaction tx)
		{
			TransactionState._TransactionStatePromotedAborted.EnterState(tx);
		}

		// Token: 0x06000150 RID: 336 RVA: 0x0002C6B0 File Offset: 0x0002BAB0
		internal override void ChangeStateTransactionAborted(InternalTransaction tx, Exception e)
		{
		}

		// Token: 0x06000151 RID: 337 RVA: 0x0002C6C0 File Offset: 0x0002BAC0
		internal override void RestartCommitIfNeeded(InternalTransaction tx)
		{
		}
	}
}
