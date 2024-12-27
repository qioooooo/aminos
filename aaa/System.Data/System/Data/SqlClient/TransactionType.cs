using System;

namespace System.Data.SqlClient
{
	// Token: 0x020002FE RID: 766
	internal enum TransactionType
	{
		// Token: 0x04001916 RID: 6422
		LocalFromTSQL = 1,
		// Token: 0x04001917 RID: 6423
		LocalFromAPI,
		// Token: 0x04001918 RID: 6424
		Delegated,
		// Token: 0x04001919 RID: 6425
		Distributed,
		// Token: 0x0400191A RID: 6426
		Context
	}
}
