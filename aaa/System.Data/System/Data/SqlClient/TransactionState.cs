using System;

namespace System.Data.SqlClient
{
	// Token: 0x020002FD RID: 765
	internal enum TransactionState
	{
		// Token: 0x04001910 RID: 6416
		Pending,
		// Token: 0x04001911 RID: 6417
		Active,
		// Token: 0x04001912 RID: 6418
		Aborted,
		// Token: 0x04001913 RID: 6419
		Committed,
		// Token: 0x04001914 RID: 6420
		Unknown
	}
}
