using System;

namespace System.Transactions
{
	// Token: 0x0200004A RID: 74
	internal class DurableEnlistmentEnded : DurableEnlistmentState
	{
		// Token: 0x06000230 RID: 560 RVA: 0x0002EF28 File Offset: 0x0002E328
		internal override void EnterState(InternalEnlistment enlistment)
		{
			enlistment.State = this;
		}

		// Token: 0x06000231 RID: 561 RVA: 0x0002EF3C File Offset: 0x0002E33C
		internal override void InternalAborted(InternalEnlistment enlistment)
		{
		}

		// Token: 0x06000232 RID: 562 RVA: 0x0002EF4C File Offset: 0x0002E34C
		internal override void InDoubt(InternalEnlistment enlistment, Exception e)
		{
		}
	}
}
