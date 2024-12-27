using System;
using System.Threading;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000035 RID: 53
	internal class TransactionStateDelegatedCommitting : TransactionStatePromotedCommitting
	{
		// Token: 0x060001A1 RID: 417 RVA: 0x0002D37C File Offset: 0x0002C77C
		internal override void EnterState(InternalTransaction tx)
		{
			base.CommonEnterState(tx);
			Monitor.Exit(tx);
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.EnlistmentNotificationCallTraceRecord.Trace(SR.GetString("TraceSourceLtm"), tx.durableEnlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.NotificationCall.SinglePhaseCommit);
			}
			try
			{
				tx.durableEnlistment.PromotableSinglePhaseNotification.SinglePhaseCommit(tx.durableEnlistment.SinglePhaseEnlistment);
			}
			finally
			{
				Monitor.Enter(tx);
			}
		}
	}
}
