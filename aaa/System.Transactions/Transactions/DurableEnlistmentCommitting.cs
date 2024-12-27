using System;
using System.Threading;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000048 RID: 72
	internal class DurableEnlistmentCommitting : DurableEnlistmentState
	{
		// Token: 0x06000225 RID: 549 RVA: 0x0002ECB0 File Offset: 0x0002E0B0
		internal override void EnterState(InternalEnlistment enlistment)
		{
			bool flag = false;
			enlistment.State = this;
			Monitor.Exit(enlistment.Transaction);
			try
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.EnlistmentNotificationCallTraceRecord.Trace(SR.GetString("TraceSourceLtm"), enlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.NotificationCall.SinglePhaseCommit);
				}
				if (enlistment.SinglePhaseNotification != null)
				{
					enlistment.SinglePhaseNotification.SinglePhaseCommit(enlistment.SinglePhaseEnlistment);
				}
				else
				{
					enlistment.PromotableSinglePhaseNotification.SinglePhaseCommit(enlistment.SinglePhaseEnlistment);
				}
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

		// Token: 0x06000226 RID: 550 RVA: 0x0002ED54 File Offset: 0x0002E154
		internal override void EnlistmentDone(InternalEnlistment enlistment)
		{
			DurableEnlistmentState._DurableEnlistmentEnded.EnterState(enlistment);
			enlistment.Transaction.State.ChangeStateTransactionCommitted(enlistment.Transaction);
		}

		// Token: 0x06000227 RID: 551 RVA: 0x0002ED84 File Offset: 0x0002E184
		internal override void Committed(InternalEnlistment enlistment)
		{
			DurableEnlistmentState._DurableEnlistmentEnded.EnterState(enlistment);
			enlistment.Transaction.State.ChangeStateTransactionCommitted(enlistment.Transaction);
		}

		// Token: 0x06000228 RID: 552 RVA: 0x0002EDB4 File Offset: 0x0002E1B4
		internal override void Aborted(InternalEnlistment enlistment, Exception e)
		{
			DurableEnlistmentState._DurableEnlistmentEnded.EnterState(enlistment);
			enlistment.Transaction.State.ChangeStateTransactionAborted(enlistment.Transaction, e);
		}

		// Token: 0x06000229 RID: 553 RVA: 0x0002EDE4 File Offset: 0x0002E1E4
		internal override void InDoubt(InternalEnlistment enlistment, Exception e)
		{
			DurableEnlistmentState._DurableEnlistmentEnded.EnterState(enlistment);
			if (enlistment.Transaction.innerException == null)
			{
				enlistment.Transaction.innerException = e;
			}
			enlistment.Transaction.State.InDoubtFromEnlistment(enlistment.Transaction);
		}
	}
}
