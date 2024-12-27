using System;

namespace System.Transactions
{
	// Token: 0x02000034 RID: 52
	internal class TransactionStateDelegatedP0Wave : TransactionStatePromotedP0Wave
	{
		// Token: 0x0600019F RID: 415 RVA: 0x0002D350 File Offset: 0x0002C750
		internal override void Phase0VolatilePrepareDone(InternalTransaction tx)
		{
			TransactionState._TransactionStateDelegatedCommitting.EnterState(tx);
		}
	}
}
