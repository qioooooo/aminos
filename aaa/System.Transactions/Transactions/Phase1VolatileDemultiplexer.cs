using System;
using System.Transactions.Diagnostics;

namespace System.Transactions
{
	// Token: 0x02000058 RID: 88
	internal class Phase1VolatileDemultiplexer : VolatileDemultiplexer
	{
		// Token: 0x06000293 RID: 659 RVA: 0x00030128 File Offset: 0x0002F528
		public Phase1VolatileDemultiplexer(InternalTransaction transaction)
			: base(transaction)
		{
		}

		// Token: 0x06000294 RID: 660 RVA: 0x0003013C File Offset: 0x0002F53C
		protected override void InternalPrepare()
		{
			try
			{
				this.transaction.State.ChangeStatePromotedPhase1(this.transaction);
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

		// Token: 0x06000295 RID: 661 RVA: 0x000301E0 File Offset: 0x0002F5E0
		protected override void InternalCommit()
		{
			this.oletxEnlistment.EnlistmentDone();
			this.transaction.State.ChangeStatePromotedCommitted(this.transaction);
		}

		// Token: 0x06000296 RID: 662 RVA: 0x00030210 File Offset: 0x0002F610
		protected override void InternalRollback()
		{
			this.oletxEnlistment.EnlistmentDone();
			this.transaction.State.ChangeStatePromotedAborted(this.transaction);
		}

		// Token: 0x06000297 RID: 663 RVA: 0x00030240 File Offset: 0x0002F640
		protected override void InternalInDoubt()
		{
			this.transaction.State.InDoubtFromDtc(this.transaction);
		}

		// Token: 0x06000298 RID: 664 RVA: 0x00030264 File Offset: 0x0002F664
		public override void Prepare(IPromotedEnlistment en)
		{
			this.preparingEnlistment = en;
			VolatileDemultiplexer.PoolablePrepare(this);
		}

		// Token: 0x06000299 RID: 665 RVA: 0x00030280 File Offset: 0x0002F680
		public override void Commit(IPromotedEnlistment en)
		{
			this.oletxEnlistment = en;
			VolatileDemultiplexer.PoolableCommit(this);
		}

		// Token: 0x0600029A RID: 666 RVA: 0x0003029C File Offset: 0x0002F69C
		public override void Rollback(IPromotedEnlistment en)
		{
			this.oletxEnlistment = en;
			VolatileDemultiplexer.PoolableRollback(this);
		}

		// Token: 0x0600029B RID: 667 RVA: 0x000302B8 File Offset: 0x0002F6B8
		public override void InDoubt(IPromotedEnlistment en)
		{
			this.oletxEnlistment = en;
			VolatileDemultiplexer.PoolableInDoubt(this);
		}
	}
}
