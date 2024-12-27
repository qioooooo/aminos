using System;

namespace System.Transactions
{
	// Token: 0x0200003E RID: 62
	internal class Phase1VolatileEnlistment : InternalEnlistment
	{
		// Token: 0x060001D8 RID: 472 RVA: 0x0002DA1C File Offset: 0x0002CE1C
		public Phase1VolatileEnlistment(Enlistment enlistment, InternalTransaction transaction, IEnlistmentNotification twoPhaseNotifications, ISinglePhaseNotification singlePhaseNotifications, Transaction atomicTransaction)
			: base(enlistment, transaction, twoPhaseNotifications, singlePhaseNotifications, atomicTransaction)
		{
		}

		// Token: 0x060001D9 RID: 473 RVA: 0x0002DA38 File Offset: 0x0002CE38
		internal override void FinishEnlistment()
		{
			InternalTransaction transaction = this.transaction;
			transaction.phase1Volatiles.preparedVolatileEnlistments = transaction.phase1Volatiles.preparedVolatileEnlistments + 1;
			this.CheckComplete();
		}

		// Token: 0x060001DA RID: 474 RVA: 0x0002DA64 File Offset: 0x0002CE64
		internal override void CheckComplete()
		{
			if (this.transaction.phase1Volatiles.preparedVolatileEnlistments == this.transaction.phase1Volatiles.volatileEnlistmentCount + this.transaction.phase1Volatiles.dependentClones)
			{
				this.transaction.State.Phase1VolatilePrepareDone(this.transaction);
			}
		}
	}
}
