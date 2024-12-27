using System;

namespace System.Transactions
{
	// Token: 0x0200005D RID: 93
	public interface ISimpleTransactionSuperior : ITransactionPromoter
	{
		// Token: 0x060002A6 RID: 678
		void Rollback();
	}
}
