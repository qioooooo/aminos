using System;
using System.Threading;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000051 RID: 81
	internal class VolatileEnlistmentAborting : VolatileEnlistmentState
	{
		// Token: 0x06000260 RID: 608 RVA: 0x0002F81C File Offset: 0x0002EC1C
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
				enlistment.EnlistmentNotification.Rollback(enlistment.SinglePhaseEnlistment);
			}
			finally
			{
				Monitor.Enter(enlistment.Transaction);
			}
		}

		// Token: 0x06000261 RID: 609 RVA: 0x0002F894 File Offset: 0x0002EC94
		internal override void ChangeStatePreparing(InternalEnlistment enlistment)
		{
		}

		// Token: 0x06000262 RID: 610 RVA: 0x0002F8A4 File Offset: 0x0002ECA4
		internal override void EnlistmentDone(InternalEnlistment enlistment)
		{
			VolatileEnlistmentState._VolatileEnlistmentEnded.EnterState(enlistment);
		}

		// Token: 0x06000263 RID: 611 RVA: 0x0002F8BC File Offset: 0x0002ECBC
		internal override void InternalAborted(InternalEnlistment enlistment)
		{
		}
	}
}
