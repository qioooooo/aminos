using System;

namespace System.EnterpriseServices
{
	// Token: 0x02000062 RID: 98
	[Serializable]
	public enum TransactionIsolationLevel
	{
		// Token: 0x040000E0 RID: 224
		Any,
		// Token: 0x040000E1 RID: 225
		ReadUncommitted,
		// Token: 0x040000E2 RID: 226
		ReadCommitted,
		// Token: 0x040000E3 RID: 227
		RepeatableRead,
		// Token: 0x040000E4 RID: 228
		Serializable
	}
}
