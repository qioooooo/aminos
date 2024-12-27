using System;

namespace System.Transactions
{
	// Token: 0x02000055 RID: 85
	internal class VolatileEnlistmentDone : VolatileEnlistmentEnded
	{
		// Token: 0x06000272 RID: 626 RVA: 0x0002FAA0 File Offset: 0x0002EEA0
		internal override void EnterState(InternalEnlistment enlistment)
		{
			enlistment.State = this;
		}

		// Token: 0x06000273 RID: 627 RVA: 0x0002FAB4 File Offset: 0x0002EEB4
		internal override void ChangeStatePreparing(InternalEnlistment enlistment)
		{
			enlistment.CheckComplete();
		}
	}
}
