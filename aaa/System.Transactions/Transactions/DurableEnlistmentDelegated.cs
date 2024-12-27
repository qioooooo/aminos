using System;

namespace System.Transactions
{
	// Token: 0x02000049 RID: 73
	internal class DurableEnlistmentDelegated : DurableEnlistmentState
	{
		// Token: 0x0600022B RID: 555 RVA: 0x0002EE40 File Offset: 0x0002E240
		internal override void EnterState(InternalEnlistment enlistment)
		{
			enlistment.State = this;
		}

		// Token: 0x0600022C RID: 556 RVA: 0x0002EE54 File Offset: 0x0002E254
		internal override void Committed(InternalEnlistment enlistment)
		{
			DurableEnlistmentState._DurableEnlistmentEnded.EnterState(enlistment);
			enlistment.Transaction.State.ChangeStatePromotedCommitted(enlistment.Transaction);
		}

		// Token: 0x0600022D RID: 557 RVA: 0x0002EE84 File Offset: 0x0002E284
		internal override void Aborted(InternalEnlistment enlistment, Exception e)
		{
			DurableEnlistmentState._DurableEnlistmentEnded.EnterState(enlistment);
			if (enlistment.Transaction.innerException == null)
			{
				enlistment.Transaction.innerException = e;
			}
			enlistment.Transaction.State.ChangeStatePromotedAborted(enlistment.Transaction);
		}

		// Token: 0x0600022E RID: 558 RVA: 0x0002EECC File Offset: 0x0002E2CC
		internal override void InDoubt(InternalEnlistment enlistment, Exception e)
		{
			DurableEnlistmentState._DurableEnlistmentEnded.EnterState(enlistment);
			if (enlistment.Transaction.innerException == null)
			{
				enlistment.Transaction.innerException = e;
			}
			enlistment.Transaction.State.InDoubtFromEnlistment(enlistment.Transaction);
		}
	}
}
