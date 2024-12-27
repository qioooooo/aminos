using System;
using System.Threading;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000052 RID: 82
	internal class VolatileEnlistmentCommitting : VolatileEnlistmentState
	{
		// Token: 0x06000265 RID: 613 RVA: 0x0002F8E0 File Offset: 0x0002ECE0
		internal override void EnterState(InternalEnlistment enlistment)
		{
			enlistment.State = this;
			Monitor.Exit(enlistment.Transaction);
			try
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.EnlistmentNotificationCallTraceRecord.Trace(SR.GetString("TraceSourceLtm"), enlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.NotificationCall.Commit);
				}
				enlistment.EnlistmentNotification.Commit(enlistment.Enlistment);
			}
			finally
			{
				Monitor.Enter(enlistment.Transaction);
			}
		}

		// Token: 0x06000266 RID: 614 RVA: 0x0002F958 File Offset: 0x0002ED58
		internal override void EnlistmentDone(InternalEnlistment enlistment)
		{
			VolatileEnlistmentState._VolatileEnlistmentEnded.EnterState(enlistment);
		}
	}
}
