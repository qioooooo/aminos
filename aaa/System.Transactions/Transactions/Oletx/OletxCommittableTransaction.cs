using System;
using System.Transactions.Diagnostics;

namespace System.Transactions.Oletx
{
	// Token: 0x0200008A RID: 138
	[Serializable]
	internal class OletxCommittableTransaction : OletxTransaction
	{
		// Token: 0x06000387 RID: 903 RVA: 0x00034A4C File Offset: 0x00033E4C
		internal OletxCommittableTransaction(RealOletxTransaction realOletxTransaction)
			: base(realOletxTransaction)
		{
			realOletxTransaction.committableTransaction = this;
		}

		// Token: 0x17000086 RID: 134
		// (get) Token: 0x06000388 RID: 904 RVA: 0x00034A68 File Offset: 0x00033E68
		internal bool CommitCalled
		{
			get
			{
				return this.commitCalled;
			}
		}

		// Token: 0x06000389 RID: 905 RVA: 0x00034A7C File Offset: 0x00033E7C
		internal void BeginCommit(InternalTransaction internalTransaction)
		{
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodEnteredTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "CommittableTransaction.BeginCommit");
				global::System.Transactions.Diagnostics.TransactionCommitCalledTraceRecord.Trace(SR.GetString("TraceSourceOletx"), base.TransactionTraceId);
			}
			this.realOletxTransaction.InternalTransaction = internalTransaction;
			this.commitCalled = true;
			this.realOletxTransaction.Commit();
			if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
			{
				global::System.Transactions.Diagnostics.MethodExitedTraceRecord.Trace(SR.GetString("TraceSourceOletx"), "CommittableTransaction.BeginCommit");
			}
		}

		// Token: 0x040001D5 RID: 469
		private bool commitCalled;
	}
}
