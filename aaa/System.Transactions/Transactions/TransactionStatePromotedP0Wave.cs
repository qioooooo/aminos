using System;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000025 RID: 37
	internal class TransactionStatePromotedP0Wave : TransactionStatePromotedBase
	{
		// Token: 0x0600012F RID: 303 RVA: 0x0002C16C File Offset: 0x0002B56C
		internal override void EnterState(InternalTransaction tx)
		{
			base.CommonEnterState(tx);
		}

		// Token: 0x06000130 RID: 304 RVA: 0x0002C180 File Offset: 0x0002B580
		internal override void BeginCommit(InternalTransaction tx, bool asyncCommit, AsyncCallback asyncCallback, object asyncState)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x06000131 RID: 305 RVA: 0x0002C1A4 File Offset: 0x0002B5A4
		internal override void Phase0VolatilePrepareDone(InternalTransaction tx)
		{
			try
			{
				TransactionState._TransactionStatePromotedCommitting.EnterState(tx);
			}
			catch (TransactionException ex)
			{
				if (tx.innerException == null)
				{
					tx.innerException = ex;
				}
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), ex);
				}
			}
		}

		// Token: 0x06000132 RID: 306 RVA: 0x0002C204 File Offset: 0x0002B604
		internal override bool ContinuePhase0Prepares()
		{
			return true;
		}

		// Token: 0x06000133 RID: 307 RVA: 0x0002C214 File Offset: 0x0002B614
		internal override void ChangeStateTransactionAborted(InternalTransaction tx, Exception e)
		{
			if (tx.innerException == null)
			{
				tx.innerException = e;
			}
			TransactionState._TransactionStatePromotedP0Aborting.EnterState(tx);
		}
	}
}
