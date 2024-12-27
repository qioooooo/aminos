using System;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x020000A6 RID: 166
	[Serializable]
	public enum TransactionState
	{
		// Token: 0x040001D6 RID: 470
		Active,
		// Token: 0x040001D7 RID: 471
		Committed,
		// Token: 0x040001D8 RID: 472
		Aborted,
		// Token: 0x040001D9 RID: 473
		Indoubt
	}
}
