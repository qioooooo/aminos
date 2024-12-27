using System;
using System.Threading;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000036 RID: 54
	internal class TransactionStateDelegatedAborting : TransactionStatePromotedAborted
	{
		// Token: 0x060001A3 RID: 419 RVA: 0x0002D410 File Offset: 0x0002C810
		internal override void EnterState(InternalTransaction tx)
		{
			base.CommonEnterState(tx);
			Monitor.Exit(tx);
			try
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.EnlistmentNotificationCallTraceRecord.Trace(SR.GetString("TraceSourceLtm"), tx.durableEnlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.NotificationCall.Rollback);
				}
				tx.durableEnlistment.PromotableSinglePhaseNotification.Rollback(tx.durableEnlistment.SinglePhaseEnlistment);
			}
			finally
			{
				Monitor.Enter(tx);
			}
		}

		// Token: 0x060001A4 RID: 420 RVA: 0x0002D48C File Offset: 0x0002C88C
		internal override void BeginCommit(InternalTransaction tx, bool asyncCommit, AsyncCallback asyncCallback, object asyncState)
		{
			throw TransactionException.CreateTransactionStateException(SR.GetString("TraceSourceLtm"), tx.innerException);
		}

		// Token: 0x060001A5 RID: 421 RVA: 0x0002D4B0 File Offset: 0x0002C8B0
		internal override void ChangeStatePromotedAborted(InternalTransaction tx)
		{
			TransactionState._TransactionStatePromotedAborted.EnterState(tx);
		}
	}
}
