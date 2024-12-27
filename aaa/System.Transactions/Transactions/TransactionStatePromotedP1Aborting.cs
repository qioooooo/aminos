using System;
using System.Threading;

namespace System.Transactions
{
	// Token: 0x0200002B RID: 43
	internal class TransactionStatePromotedP1Aborting : TransactionStatePromotedAborting
	{
		// Token: 0x06000156 RID: 342 RVA: 0x0002C780 File Offset: 0x0002BB80
		internal override void EnterState(InternalTransaction tx)
		{
			base.CommonEnterState(tx);
			this.ChangeStatePromotedAborted(tx);
			Monitor.Exit(tx);
			try
			{
				tx.phase1Volatiles.VolatileDemux.oletxEnlistment.ForceRollback();
			}
			finally
			{
				Monitor.Enter(tx);
			}
		}

		// Token: 0x06000157 RID: 343 RVA: 0x0002C7DC File Offset: 0x0002BBDC
		internal override void Phase1VolatilePrepareDone(InternalTransaction tx)
		{
		}
	}
}
