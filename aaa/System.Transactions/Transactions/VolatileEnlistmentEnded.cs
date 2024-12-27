using System;

namespace System.Transactions
{
	// Token: 0x02000054 RID: 84
	internal class VolatileEnlistmentEnded : VolatileEnlistmentState
	{
		// Token: 0x0600026B RID: 619 RVA: 0x0002FA28 File Offset: 0x0002EE28
		internal override void EnterState(InternalEnlistment enlistment)
		{
			enlistment.State = this;
		}

		// Token: 0x0600026C RID: 620 RVA: 0x0002FA3C File Offset: 0x0002EE3C
		internal override void ChangeStatePreparing(InternalEnlistment enlistment)
		{
		}

		// Token: 0x0600026D RID: 621 RVA: 0x0002FA4C File Offset: 0x0002EE4C
		internal override void InternalAborted(InternalEnlistment enlistment)
		{
		}

		// Token: 0x0600026E RID: 622 RVA: 0x0002FA5C File Offset: 0x0002EE5C
		internal override void InternalCommitted(InternalEnlistment enlistment)
		{
		}

		// Token: 0x0600026F RID: 623 RVA: 0x0002FA6C File Offset: 0x0002EE6C
		internal override void InternalIndoubt(InternalEnlistment enlistment)
		{
		}

		// Token: 0x06000270 RID: 624 RVA: 0x0002FA7C File Offset: 0x0002EE7C
		internal override void InDoubt(InternalEnlistment enlistment, Exception e)
		{
		}
	}
}
