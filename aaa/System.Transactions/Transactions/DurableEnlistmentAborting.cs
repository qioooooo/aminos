using System;
using System.Threading;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000047 RID: 71
	internal class DurableEnlistmentAborting : DurableEnlistmentState
	{
		// Token: 0x06000221 RID: 545 RVA: 0x0002EBBC File Offset: 0x0002DFBC
		internal override void EnterState(InternalEnlistment enlistment)
		{
			enlistment.State = this;
			Monitor.Exit(enlistment.Transaction);
			try
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.EnlistmentNotificationCallTraceRecord.Trace(SR.GetString("TraceSourceLtm"), enlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.NotificationCall.Rollback);
				}
				if (enlistment.SinglePhaseNotification != null)
				{
					enlistment.SinglePhaseNotification.Rollback(enlistment.SinglePhaseEnlistment);
				}
				else
				{
					enlistment.PromotableSinglePhaseNotification.Rollback(enlistment.SinglePhaseEnlistment);
				}
			}
			finally
			{
				Monitor.Enter(enlistment.Transaction);
			}
		}

		// Token: 0x06000222 RID: 546 RVA: 0x0002EC50 File Offset: 0x0002E050
		internal override void Aborted(InternalEnlistment enlistment, Exception e)
		{
			if (enlistment.Transaction.innerException == null)
			{
				enlistment.Transaction.innerException = e;
			}
			DurableEnlistmentState._DurableEnlistmentEnded.EnterState(enlistment);
		}

		// Token: 0x06000223 RID: 547 RVA: 0x0002EC84 File Offset: 0x0002E084
		internal override void EnlistmentDone(InternalEnlistment enlistment)
		{
			DurableEnlistmentState._DurableEnlistmentEnded.EnterState(enlistment);
		}
	}
}
