using System;

namespace System.Transactions
{
	// Token: 0x02000046 RID: 70
	internal class DurableEnlistmentActive : DurableEnlistmentState
	{
		// Token: 0x0600021A RID: 538 RVA: 0x0002EB14 File Offset: 0x0002DF14
		internal override void EnterState(InternalEnlistment enlistment)
		{
			enlistment.State = this;
		}

		// Token: 0x0600021B RID: 539 RVA: 0x0002EB28 File Offset: 0x0002DF28
		internal override void EnlistmentDone(InternalEnlistment enlistment)
		{
			DurableEnlistmentState._DurableEnlistmentEnded.EnterState(enlistment);
		}

		// Token: 0x0600021C RID: 540 RVA: 0x0002EB40 File Offset: 0x0002DF40
		internal override void InternalAborted(InternalEnlistment enlistment)
		{
			DurableEnlistmentState._DurableEnlistmentAborting.EnterState(enlistment);
		}

		// Token: 0x0600021D RID: 541 RVA: 0x0002EB58 File Offset: 0x0002DF58
		internal override void ChangeStateCommitting(InternalEnlistment enlistment)
		{
			DurableEnlistmentState._DurableEnlistmentCommitting.EnterState(enlistment);
		}

		// Token: 0x0600021E RID: 542 RVA: 0x0002EB70 File Offset: 0x0002DF70
		internal override void ChangeStatePromoted(InternalEnlistment enlistment, IPromotedEnlistment promotedEnlistment)
		{
			enlistment.PromotedEnlistment = promotedEnlistment;
			EnlistmentState._EnlistmentStatePromoted.EnterState(enlistment);
		}

		// Token: 0x0600021F RID: 543 RVA: 0x0002EB90 File Offset: 0x0002DF90
		internal override void ChangeStateDelegated(InternalEnlistment enlistment)
		{
			DurableEnlistmentState._DurableEnlistmentDelegated.EnterState(enlistment);
		}
	}
}
