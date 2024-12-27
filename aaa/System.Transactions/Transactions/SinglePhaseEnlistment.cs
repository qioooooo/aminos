using System;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000041 RID: 65
	public class SinglePhaseEnlistment : Enlistment
	{
		// Token: 0x060001E8 RID: 488 RVA: 0x0002DF40 File Offset: 0x0002D340
		internal SinglePhaseEnlistment(InternalEnlistment enlistment)
			: base(enlistment)
		{
		}

		// Token: 0x060001E9 RID: 489 RVA: 0x0002DF54 File Offset: 0x0002D354
		public void Aborted()
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "SinglePhaseEnlistment.Aborted");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
			{
				global::System.Transactions.Diagnostics.EnlistmentCallbackNegativeTraceRecord.Trace(SR.GetString("TraceSourceLtm"), this.internalEnlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.EnlistmentCallback.Aborted);
			}
			lock (this.internalEnlistment.SyncRoot)
			{
				this.internalEnlistment.State.Aborted(this.internalEnlistment, null);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "SinglePhaseEnlistment.Aborted");
			}
		}

		// Token: 0x060001EA RID: 490 RVA: 0x0002E008 File Offset: 0x0002D408
		public void Aborted(Exception e)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "SinglePhaseEnlistment.Aborted");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
			{
				global::System.Transactions.Diagnostics.EnlistmentCallbackNegativeTraceRecord.Trace(SR.GetString("TraceSourceLtm"), this.internalEnlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.EnlistmentCallback.Aborted);
			}
			lock (this.internalEnlistment.SyncRoot)
			{
				this.internalEnlistment.State.Aborted(this.internalEnlistment, e);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "SinglePhaseEnlistment.Aborted");
			}
		}

		// Token: 0x060001EB RID: 491 RVA: 0x0002E0BC File Offset: 0x0002D4BC
		public void Committed()
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "SinglePhaseEnlistment.Committed");
				global::System.Transactions.Diagnostics.EnlistmentCallbackPositiveTraceRecord.Trace(SR.GetString("TraceSourceLtm"), this.internalEnlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.EnlistmentCallback.Committed);
			}
			lock (this.internalEnlistment.SyncRoot)
			{
				this.internalEnlistment.State.Committed(this.internalEnlistment);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "SinglePhaseEnlistment.Committed");
			}
		}

		// Token: 0x060001EC RID: 492 RVA: 0x0002E168 File Offset: 0x0002D568
		public void InDoubt()
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "SinglePhaseEnlistment.InDoubt");
			}
			lock (this.internalEnlistment.SyncRoot)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
				{
					global::System.Transactions.Diagnostics.EnlistmentCallbackNegativeTraceRecord.Trace(SR.GetString("TraceSourceLtm"), this.internalEnlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.EnlistmentCallback.InDoubt);
				}
				this.internalEnlistment.State.InDoubt(this.internalEnlistment, null);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "SinglePhaseEnlistment.InDoubt");
			}
		}

		// Token: 0x060001ED RID: 493 RVA: 0x0002E21C File Offset: 0x0002D61C
		public void InDoubt(Exception e)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "SinglePhaseEnlistment.InDoubt");
			}
			lock (this.internalEnlistment.SyncRoot)
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
				{
					global::System.Transactions.Diagnostics.EnlistmentCallbackNegativeTraceRecord.Trace(SR.GetString("TraceSourceLtm"), this.internalEnlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.EnlistmentCallback.InDoubt);
				}
				this.internalEnlistment.State.InDoubt(this.internalEnlistment, e);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "SinglePhaseEnlistment.InDoubt");
			}
		}
	}
}
