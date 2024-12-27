using System;
using System.Threading;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000053 RID: 83
	internal class VolatileEnlistmentInDoubt : VolatileEnlistmentState
	{
		// Token: 0x06000268 RID: 616 RVA: 0x0002F984 File Offset: 0x0002ED84
		internal override void EnterState(InternalEnlistment enlistment)
		{
			enlistment.State = this;
			Monitor.Exit(enlistment.Transaction);
			try
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.EnlistmentNotificationCallTraceRecord.Trace(SR.GetString("TraceSourceLtm"), enlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.NotificationCall.InDoubt);
				}
				enlistment.EnlistmentNotification.InDoubt(enlistment.PreparingEnlistment);
			}
			finally
			{
				Monitor.Enter(enlistment.Transaction);
			}
		}

		// Token: 0x06000269 RID: 617 RVA: 0x0002F9FC File Offset: 0x0002EDFC
		internal override void EnlistmentDone(InternalEnlistment enlistment)
		{
			VolatileEnlistmentState._VolatileEnlistmentEnded.EnterState(enlistment);
		}
	}
}
