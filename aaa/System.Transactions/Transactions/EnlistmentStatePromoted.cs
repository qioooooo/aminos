using System;
using System.Threading;

namespace System.Transactions
{
	// Token: 0x02000044 RID: 68
	internal class EnlistmentStatePromoted : EnlistmentState
	{
		// Token: 0x0600020A RID: 522 RVA: 0x0002E688 File Offset: 0x0002DA88
		internal override void EnterState(InternalEnlistment enlistment)
		{
			enlistment.State = this;
		}

		// Token: 0x0600020B RID: 523 RVA: 0x0002E69C File Offset: 0x0002DA9C
		internal override void EnlistmentDone(InternalEnlistment enlistment)
		{
			Monitor.Exit(enlistment.SyncRoot);
			try
			{
				enlistment.PromotedEnlistment.EnlistmentDone();
			}
			finally
			{
				Monitor.Enter(enlistment.SyncRoot);
			}
		}

		// Token: 0x0600020C RID: 524 RVA: 0x0002E6EC File Offset: 0x0002DAEC
		internal override void Prepared(InternalEnlistment enlistment)
		{
			Monitor.Exit(enlistment.SyncRoot);
			try
			{
				enlistment.PromotedEnlistment.Prepared();
			}
			finally
			{
				Monitor.Enter(enlistment.SyncRoot);
			}
		}

		// Token: 0x0600020D RID: 525 RVA: 0x0002E73C File Offset: 0x0002DB3C
		internal override void ForceRollback(InternalEnlistment enlistment, Exception e)
		{
			Monitor.Exit(enlistment.SyncRoot);
			try
			{
				enlistment.PromotedEnlistment.ForceRollback(e);
			}
			finally
			{
				Monitor.Enter(enlistment.SyncRoot);
			}
		}

		// Token: 0x0600020E RID: 526 RVA: 0x0002E78C File Offset: 0x0002DB8C
		internal override void Committed(InternalEnlistment enlistment)
		{
			Monitor.Exit(enlistment.SyncRoot);
			try
			{
				enlistment.PromotedEnlistment.Committed();
			}
			finally
			{
				Monitor.Enter(enlistment.SyncRoot);
			}
		}

		// Token: 0x0600020F RID: 527 RVA: 0x0002E7DC File Offset: 0x0002DBDC
		internal override void Aborted(InternalEnlistment enlistment, Exception e)
		{
			Monitor.Exit(enlistment.SyncRoot);
			try
			{
				enlistment.PromotedEnlistment.Aborted(e);
			}
			finally
			{
				Monitor.Enter(enlistment.SyncRoot);
			}
		}

		// Token: 0x06000210 RID: 528 RVA: 0x0002E82C File Offset: 0x0002DC2C
		internal override void InDoubt(InternalEnlistment enlistment, Exception e)
		{
			Monitor.Exit(enlistment.SyncRoot);
			try
			{
				enlistment.PromotedEnlistment.InDoubt(e);
			}
			finally
			{
				Monitor.Enter(enlistment.SyncRoot);
			}
		}

		// Token: 0x06000211 RID: 529 RVA: 0x0002E87C File Offset: 0x0002DC7C
		internal override byte[] RecoveryInformation(InternalEnlistment enlistment)
		{
			Monitor.Exit(enlistment.SyncRoot);
			byte[] recoveryInformation;
			try
			{
				recoveryInformation = enlistment.PromotedEnlistment.GetRecoveryInformation();
			}
			finally
			{
				Monitor.Enter(enlistment.SyncRoot);
			}
			return recoveryInformation;
		}
	}
}
