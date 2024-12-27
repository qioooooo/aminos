using System;

namespace System.Transactions
{
	// Token: 0x0200004F RID: 79
	internal class VolatileEnlistmentPrepared : VolatileEnlistmentState
	{
		// Token: 0x06000254 RID: 596 RVA: 0x0002F6F4 File Offset: 0x0002EAF4
		internal override void EnterState(InternalEnlistment enlistment)
		{
			enlistment.State = this;
		}

		// Token: 0x06000255 RID: 597 RVA: 0x0002F708 File Offset: 0x0002EB08
		internal override void InternalAborted(InternalEnlistment enlistment)
		{
			VolatileEnlistmentState._VolatileEnlistmentAborting.EnterState(enlistment);
		}

		// Token: 0x06000256 RID: 598 RVA: 0x0002F720 File Offset: 0x0002EB20
		internal override void InternalCommitted(InternalEnlistment enlistment)
		{
			VolatileEnlistmentState._VolatileEnlistmentCommitting.EnterState(enlistment);
		}

		// Token: 0x06000257 RID: 599 RVA: 0x0002F738 File Offset: 0x0002EB38
		internal override void InternalIndoubt(InternalEnlistment enlistment)
		{
			VolatileEnlistmentState._VolatileEnlistmentInDoubt.EnterState(enlistment);
		}

		// Token: 0x06000258 RID: 600 RVA: 0x0002F750 File Offset: 0x0002EB50
		internal override void ChangeStatePreparing(InternalEnlistment enlistment)
		{
		}
	}
}
