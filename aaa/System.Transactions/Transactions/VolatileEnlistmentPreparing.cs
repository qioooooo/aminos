using System;
using System.Threading;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x0200004D RID: 77
	internal class VolatileEnlistmentPreparing : VolatileEnlistmentState
	{
		// Token: 0x06000247 RID: 583 RVA: 0x0002F450 File Offset: 0x0002E850
		internal override void EnterState(InternalEnlistment enlistment)
		{
			enlistment.State = this;
			Monitor.Exit(enlistment.Transaction);
			try
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.EnlistmentNotificationCallTraceRecord.Trace(SR.GetString("TraceSourceLtm"), enlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.NotificationCall.Prepare);
				}
				enlistment.EnlistmentNotification.Prepare(enlistment.PreparingEnlistment);
			}
			finally
			{
				Monitor.Enter(enlistment.Transaction);
			}
		}

		// Token: 0x06000248 RID: 584 RVA: 0x0002F4C8 File Offset: 0x0002E8C8
		internal override void EnlistmentDone(InternalEnlistment enlistment)
		{
			VolatileEnlistmentState._VolatileEnlistmentDone.EnterState(enlistment);
			enlistment.FinishEnlistment();
		}

		// Token: 0x06000249 RID: 585 RVA: 0x0002F4E8 File Offset: 0x0002E8E8
		internal override void Prepared(InternalEnlistment enlistment)
		{
			VolatileEnlistmentState._VolatileEnlistmentPrepared.EnterState(enlistment);
			enlistment.FinishEnlistment();
		}

		// Token: 0x0600024A RID: 586 RVA: 0x0002F508 File Offset: 0x0002E908
		internal override void ForceRollback(InternalEnlistment enlistment, Exception e)
		{
			VolatileEnlistmentState._VolatileEnlistmentEnded.EnterState(enlistment);
			enlistment.Transaction.State.ChangeStateTransactionAborted(enlistment.Transaction, e);
			enlistment.FinishEnlistment();
		}

		// Token: 0x0600024B RID: 587 RVA: 0x0002F540 File Offset: 0x0002E940
		internal override void ChangeStatePreparing(InternalEnlistment enlistment)
		{
		}

		// Token: 0x0600024C RID: 588 RVA: 0x0002F550 File Offset: 0x0002E950
		internal override void InternalAborted(InternalEnlistment enlistment)
		{
			VolatileEnlistmentState._VolatileEnlistmentPreparingAborting.EnterState(enlistment);
		}
	}
}
