using System;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000011 RID: 17
	[Serializable]
	public sealed class DependentTransaction : Transaction
	{
		// Token: 0x06000043 RID: 67 RVA: 0x000288FC File Offset: 0x00027CFC
		internal DependentTransaction(IsolationLevel isoLevel, InternalTransaction internalTransaction, bool blocking)
			: base(isoLevel, internalTransaction)
		{
			this.blocking = blocking;
			lock (this.internalTransaction)
			{
				if (blocking)
				{
					this.internalTransaction.State.CreateBlockingClone(this.internalTransaction);
				}
				else
				{
					this.internalTransaction.State.CreateAbortingClone(this.internalTransaction);
				}
			}
		}

		// Token: 0x06000044 RID: 68 RVA: 0x0002897C File Offset: 0x00027D7C
		public void Complete()
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "DependentTransaction.Complete");
			}
			lock (this.internalTransaction)
			{
				if (base.Disposed)
				{
					throw new ObjectDisposedException("Transaction");
				}
				if (this.complete)
				{
					throw TransactionException.CreateTransactionCompletedException(SR.GetString("TraceSourceLtm"));
				}
				this.complete = true;
				if (this.blocking)
				{
					this.internalTransaction.State.CompleteBlockingClone(this.internalTransaction);
				}
				else
				{
					this.internalTransaction.State.CompleteAbortingClone(this.internalTransaction);
				}
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Information)
			{
				global::System.Transactions.Diagnostics.DependentCloneCompleteTraceRecord.Trace(SR.GetString("TraceSourceLtm"), base.TransactionTraceId);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "DependentTransaction.Complete");
			}
		}

		// Token: 0x040000A0 RID: 160
		private bool blocking;
	}
}
