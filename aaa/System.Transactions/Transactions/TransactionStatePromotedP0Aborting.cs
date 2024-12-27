using System;
using System.Threading;

namespace System.Transactions
{
	// Token: 0x0200002A RID: 42
	internal class TransactionStatePromotedP0Aborting : TransactionStatePromotedAborting
	{
		// Token: 0x06000153 RID: 339 RVA: 0x0002C6E4 File Offset: 0x0002BAE4
		internal override void EnterState(InternalTransaction tx)
		{
			base.CommonEnterState(tx);
			this.ChangeStatePromotedAborted(tx);
			if (tx.phase0Volatiles.VolatileDemux.preparingEnlistment != null)
			{
				Monitor.Exit(tx);
				try
				{
					tx.phase0Volatiles.VolatileDemux.oletxEnlistment.ForceRollback();
					return;
				}
				finally
				{
					Monitor.Enter(tx);
				}
			}
			tx.PromotedTransaction.Rollback();
		}

		// Token: 0x06000154 RID: 340 RVA: 0x0002C75C File Offset: 0x0002BB5C
		internal override void Phase0VolatilePrepareDone(InternalTransaction tx)
		{
		}
	}
}
