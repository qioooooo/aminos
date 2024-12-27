using System;

namespace System.Transactions
{
	// Token: 0x0200005A RID: 90
	public interface IEnlistmentNotification
	{
		// Token: 0x0600029E RID: 670
		void Prepare(PreparingEnlistment preparingEnlistment);

		// Token: 0x0600029F RID: 671
		void Commit(Enlistment enlistment);

		// Token: 0x060002A0 RID: 672
		void Rollback(Enlistment enlistment);

		// Token: 0x060002A1 RID: 673
		void InDoubt(Enlistment enlistment);
	}
}
