using System;
using System.Threading;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x0200004E RID: 78
	internal class VolatileEnlistmentSPC : VolatileEnlistmentState
	{
		// Token: 0x0600024E RID: 590 RVA: 0x0002F57C File Offset: 0x0002E97C
		internal override void EnterState(InternalEnlistment enlistment)
		{
			bool flag = false;
			enlistment.State = this;
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.EnlistmentNotificationCallTraceRecord.Trace(SR.GetString("TraceSourceLtm"), enlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.NotificationCall.SinglePhaseCommit);
			}
			Monitor.Exit(enlistment.Transaction);
			try
			{
				enlistment.SinglePhaseNotification.SinglePhaseCommit(enlistment.SinglePhaseEnlistment);
				flag = true;
			}
			finally
			{
				if (!flag)
				{
					enlistment.SinglePhaseEnlistment.InDoubt();
				}
				Monitor.Enter(enlistment.Transaction);
			}
		}

		// Token: 0x0600024F RID: 591 RVA: 0x0002F608 File Offset: 0x0002EA08
		internal override void EnlistmentDone(InternalEnlistment enlistment)
		{
			VolatileEnlistmentState._VolatileEnlistmentEnded.EnterState(enlistment);
			enlistment.Transaction.State.ChangeStateTransactionCommitted(enlistment.Transaction);
		}

		// Token: 0x06000250 RID: 592 RVA: 0x0002F638 File Offset: 0x0002EA38
		internal override void Committed(InternalEnlistment enlistment)
		{
			VolatileEnlistmentState._VolatileEnlistmentEnded.EnterState(enlistment);
			enlistment.Transaction.State.ChangeStateTransactionCommitted(enlistment.Transaction);
		}

		// Token: 0x06000251 RID: 593 RVA: 0x0002F668 File Offset: 0x0002EA68
		internal override void Aborted(InternalEnlistment enlistment, Exception e)
		{
			VolatileEnlistmentState._VolatileEnlistmentEnded.EnterState(enlistment);
			enlistment.Transaction.State.ChangeStateTransactionAborted(enlistment.Transaction, e);
		}

		// Token: 0x06000252 RID: 594 RVA: 0x0002F698 File Offset: 0x0002EA98
		internal override void InDoubt(InternalEnlistment enlistment, Exception e)
		{
			VolatileEnlistmentState._VolatileEnlistmentEnded.EnterState(enlistment);
			if (enlistment.Transaction.innerException == null)
			{
				enlistment.Transaction.innerException = e;
			}
			enlistment.Transaction.State.InDoubtFromEnlistment(enlistment.Transaction);
		}
	}
}
