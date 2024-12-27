using System;
using System.Threading;

namespace System.Transactions
{
	// Token: 0x0200002C RID: 44
	internal abstract class TransactionStatePromotedEnded : TransactionStateEnded
	{
		// Token: 0x06000159 RID: 345 RVA: 0x0002C800 File Offset: 0x0002BC00
		internal override void EnterState(InternalTransaction tx)
		{
			base.EnterState(tx);
			base.CommonEnterState(tx);
			if (!ThreadPool.QueueUserWorkItem(TransactionStatePromotedEnded.SignalMethod, tx))
			{
				throw TransactionException.CreateInvalidOperationException(SR.GetString("TraceSourceLtm"), SR.GetString("UnexpectedFailureOfThreadPool"), null);
			}
		}

		// Token: 0x0600015A RID: 346 RVA: 0x0002C844 File Offset: 0x0002BC44
		internal override void AddOutcomeRegistrant(InternalTransaction tx, TransactionCompletedEventHandler transactionCompletedDelegate)
		{
			if (transactionCompletedDelegate != null)
			{
				TransactionEventArgs transactionEventArgs = new TransactionEventArgs();
				transactionEventArgs.transaction = tx.outcomeSource.InternalClone();
				transactionCompletedDelegate(transactionEventArgs.transaction, transactionEventArgs);
			}
		}

		// Token: 0x0600015B RID: 347 RVA: 0x0002C878 File Offset: 0x0002BC78
		internal override void EndCommit(InternalTransaction tx)
		{
			this.PromotedTransactionOutcome(tx);
		}

		// Token: 0x0600015C RID: 348 RVA: 0x0002C88C File Offset: 0x0002BC8C
		internal override void CompleteBlockingClone(InternalTransaction tx)
		{
		}

		// Token: 0x0600015D RID: 349 RVA: 0x0002C89C File Offset: 0x0002BC9C
		internal override void CompleteAbortingClone(InternalTransaction tx)
		{
		}

		// Token: 0x0600015E RID: 350 RVA: 0x0002C8AC File Offset: 0x0002BCAC
		internal override void CreateBlockingClone(InternalTransaction tx)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x0600015F RID: 351 RVA: 0x0002C8D0 File Offset: 0x0002BCD0
		internal override void CreateAbortingClone(InternalTransaction tx)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x06000160 RID: 352 RVA: 0x0002C8F4 File Offset: 0x0002BCF4
		internal override Guid get_Identifier(InternalTransaction tx)
		{
			return tx.PromotedTransaction.Identifier;
		}

		// Token: 0x06000161 RID: 353 RVA: 0x0002C90C File Offset: 0x0002BD0C
		internal override void Promote(InternalTransaction tx)
		{
		}

		// Token: 0x06000162 RID: 354
		protected abstract void PromotedTransactionOutcome(InternalTransaction tx);

		// Token: 0x17000038 RID: 56
		// (get) Token: 0x06000163 RID: 355 RVA: 0x0002C91C File Offset: 0x0002BD1C
		private static WaitCallback SignalMethod
		{
			get
			{
				if (TransactionStatePromotedEnded.signalMethod == null)
				{
					lock (TransactionState.ClassSyncObject)
					{
						if (TransactionStatePromotedEnded.signalMethod == null)
						{
							TransactionStatePromotedEnded.signalMethod = new WaitCallback(TransactionStatePromotedEnded.SignalCallback);
						}
					}
				}
				return TransactionStatePromotedEnded.signalMethod;
			}
		}

		// Token: 0x06000164 RID: 356 RVA: 0x0002C980 File Offset: 0x0002BD80
		private static void SignalCallback(object state)
		{
			InternalTransaction internalTransaction = (InternalTransaction)state;
			lock (internalTransaction)
			{
				internalTransaction.SignalAsyncCompletion();
				TransactionManager.TransactionTable.Remove(internalTransaction);
			}
		}

		// Token: 0x040000E2 RID: 226
		private static WaitCallback signalMethod;
	}
}
