using System;

namespace System.Transactions
{
	// Token: 0x0200004C RID: 76
	internal class VolatileEnlistmentActive : VolatileEnlistmentState
	{
		// Token: 0x06000241 RID: 577 RVA: 0x0002F3C0 File Offset: 0x0002E7C0
		internal override void EnterState(InternalEnlistment enlistment)
		{
			enlistment.State = this;
		}

		// Token: 0x06000242 RID: 578 RVA: 0x0002F3D4 File Offset: 0x0002E7D4
		internal override void EnlistmentDone(InternalEnlistment enlistment)
		{
			VolatileEnlistmentState._VolatileEnlistmentDone.EnterState(enlistment);
			enlistment.FinishEnlistment();
		}

		// Token: 0x06000243 RID: 579 RVA: 0x0002F3F4 File Offset: 0x0002E7F4
		internal override void ChangeStatePreparing(InternalEnlistment enlistment)
		{
			VolatileEnlistmentState._VolatileEnlistmentPreparing.EnterState(enlistment);
		}

		// Token: 0x06000244 RID: 580 RVA: 0x0002F40C File Offset: 0x0002E80C
		internal override void ChangeStateSinglePhaseCommit(InternalEnlistment enlistment)
		{
			VolatileEnlistmentState._VolatileEnlistmentSPC.EnterState(enlistment);
		}

		// Token: 0x06000245 RID: 581 RVA: 0x0002F424 File Offset: 0x0002E824
		internal override void InternalAborted(InternalEnlistment enlistment)
		{
			VolatileEnlistmentState._VolatileEnlistmentAborting.EnterState(enlistment);
		}
	}
}
