using System;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000057 RID: 87
	internal class Phase0VolatileDemultiplexer : VolatileDemultiplexer
	{
		// Token: 0x0600028A RID: 650 RVA: 0x0002FF7C File Offset: 0x0002F37C
		public Phase0VolatileDemultiplexer(InternalTransaction transaction)
			: base(transaction)
		{
		}

		// Token: 0x0600028B RID: 651 RVA: 0x0002FF90 File Offset: 0x0002F390
		protected override void InternalPrepare()
		{
			try
			{
				this.transaction.State.ChangeStatePromotedPhase0(this.transaction);
			}
			catch (TransactionAbortedException ex)
			{
				this.oletxEnlistment.ForceRollback(ex);
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), ex);
				}
			}
			catch (TransactionInDoubtException ex2)
			{
				this.oletxEnlistment.EnlistmentDone();
				if (global::System.Transactions.Diagnostics.DiagnosticTrace.Verbose)
				{
					global::System.Transactions.Diagnostics.ExceptionConsumedTraceRecord.Trace(SR.GetString("TraceSourceLtm"), ex2);
				}
			}
		}

		// Token: 0x0600028C RID: 652 RVA: 0x00030034 File Offset: 0x0002F434
		protected override void InternalCommit()
		{
			this.oletxEnlistment.EnlistmentDone();
			this.transaction.State.ChangeStatePromotedCommitted(this.transaction);
		}

		// Token: 0x0600028D RID: 653 RVA: 0x00030064 File Offset: 0x0002F464
		protected override void InternalRollback()
		{
			this.oletxEnlistment.EnlistmentDone();
			this.transaction.State.ChangeStatePromotedAborted(this.transaction);
		}

		// Token: 0x0600028E RID: 654 RVA: 0x00030094 File Offset: 0x0002F494
		protected override void InternalInDoubt()
		{
			this.transaction.State.InDoubtFromDtc(this.transaction);
		}

		// Token: 0x0600028F RID: 655 RVA: 0x000300B8 File Offset: 0x0002F4B8
		public override void Prepare(IPromotedEnlistment en)
		{
			this.preparingEnlistment = en;
			VolatileDemultiplexer.PoolablePrepare(this);
		}

		// Token: 0x06000290 RID: 656 RVA: 0x000300D4 File Offset: 0x0002F4D4
		public override void Commit(IPromotedEnlistment en)
		{
			this.oletxEnlistment = en;
			VolatileDemultiplexer.PoolableCommit(this);
		}

		// Token: 0x06000291 RID: 657 RVA: 0x000300F0 File Offset: 0x0002F4F0
		public override void Rollback(IPromotedEnlistment en)
		{
			this.oletxEnlistment = en;
			VolatileDemultiplexer.PoolableRollback(this);
		}

		// Token: 0x06000292 RID: 658 RVA: 0x0003010C File Offset: 0x0002F50C
		public override void InDoubt(IPromotedEnlistment en)
		{
			this.oletxEnlistment = en;
			VolatileDemultiplexer.PoolableInDoubt(this);
		}
	}
}
