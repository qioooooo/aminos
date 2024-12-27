using System;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000040 RID: 64
	public class PreparingEnlistment : Enlistment
	{
		// Token: 0x060001E3 RID: 483 RVA: 0x0002DC68 File Offset: 0x0002D068
		internal PreparingEnlistment(InternalEnlistment enlistment)
			: base(enlistment)
		{
		}

		// Token: 0x060001E4 RID: 484 RVA: 0x0002DC7C File Offset: 0x0002D07C
		public void Prepared()
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "PreparingEnlistment.Prepared");
				global::System.Transactions.Diagnostics.EnlistmentCallbackPositiveTraceRecord.Trace(SR.GetString("TraceSourceLtm"), this.internalEnlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.EnlistmentCallback.Prepared);
			}
			lock (this.internalEnlistment.SyncRoot)
			{
				this.internalEnlistment.State.Prepared(this.internalEnlistment);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "PreparingEnlistment.Prepared");
			}
		}

		// Token: 0x060001E5 RID: 485 RVA: 0x0002DD28 File Offset: 0x0002D128
		public void ForceRollback()
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "PreparingEnlistment.ForceRollback");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
			{
				global::System.Transactions.Diagnostics.EnlistmentCallbackNegativeTraceRecord.Trace(SR.GetString("TraceSourceLtm"), this.internalEnlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.EnlistmentCallback.ForceRollback);
			}
			lock (this.internalEnlistment.SyncRoot)
			{
				this.internalEnlistment.State.ForceRollback(this.internalEnlistment, null);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "PreparingEnlistment.ForceRollback");
			}
		}

		// Token: 0x060001E6 RID: 486 RVA: 0x0002DDDC File Offset: 0x0002D1DC
		public void ForceRollback(Exception e)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "PreparingEnlistment.ForceRollback");
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Warning)
			{
				global::System.Transactions.Diagnostics.EnlistmentCallbackNegativeTraceRecord.Trace(SR.GetString("TraceSourceLtm"), this.internalEnlistment.EnlistmentTraceId, global::System.Transactions.Diagnostics.EnlistmentCallback.ForceRollback);
			}
			lock (this.internalEnlistment.SyncRoot)
			{
				this.internalEnlistment.State.ForceRollback(this.internalEnlistment, e);
			}
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "PreparingEnlistment.ForceRollback");
			}
		}

		// Token: 0x060001E7 RID: 487 RVA: 0x0002DE90 File Offset: 0x0002D290
		public byte[] RecoveryInformation()
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "PreparingEnlistment.RecoveryInformation");
			}
			byte[] array;
			try
			{
				lock (this.internalEnlistment.SyncRoot)
				{
					array = this.internalEnlistment.State.RecoveryInformation(this.internalEnlistment);
				}
			}
			finally
			{
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), "PreparingEnlistment.RecoveryInformation");
				}
			}
			return array;
		}
	}
}
