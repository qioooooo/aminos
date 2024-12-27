using System;

namespace System.Transactions
{
	// Token: 0x02000050 RID: 80
	internal class VolatileEnlistmentPreparingAborting : VolatileEnlistmentState
	{
		// Token: 0x0600025A RID: 602 RVA: 0x0002F774 File Offset: 0x0002EB74
		internal override void EnterState(InternalEnlistment enlistment)
		{
			enlistment.State = this;
		}

		// Token: 0x0600025B RID: 603 RVA: 0x0002F788 File Offset: 0x0002EB88
		internal override void EnlistmentDone(InternalEnlistment enlistment)
		{
			VolatileEnlistmentState._VolatileEnlistmentEnded.EnterState(enlistment);
		}

		// Token: 0x0600025C RID: 604 RVA: 0x0002F7A0 File Offset: 0x0002EBA0
		internal override void Prepared(InternalEnlistment enlistment)
		{
			VolatileEnlistmentState._VolatileEnlistmentAborting.EnterState(enlistment);
			enlistment.FinishEnlistment();
		}

		// Token: 0x0600025D RID: 605 RVA: 0x0002F7C0 File Offset: 0x0002EBC0
		internal override void ForceRollback(InternalEnlistment enlistment, Exception e)
		{
			VolatileEnlistmentState._VolatileEnlistmentEnded.EnterState(enlistment);
			if (enlistment.Transaction.innerException == null)
			{
				enlistment.Transaction.innerException = e;
			}
			enlistment.FinishEnlistment();
		}

		// Token: 0x0600025E RID: 606 RVA: 0x0002F7F8 File Offset: 0x0002EBF8
		internal override void InternalAborted(InternalEnlistment enlistment)
		{
		}
	}
}
