using System;

namespace System.Data.SqlTypes
{
	// Token: 0x02000356 RID: 854
	[Flags]
	[Serializable]
	public enum SqlCompareOptions
	{
		// Token: 0x04001D2C RID: 7468
		None = 0,
		// Token: 0x04001D2D RID: 7469
		IgnoreCase = 1,
		// Token: 0x04001D2E RID: 7470
		IgnoreNonSpace = 2,
		// Token: 0x04001D2F RID: 7471
		IgnoreKanaType = 8,
		// Token: 0x04001D30 RID: 7472
		IgnoreWidth = 16,
		// Token: 0x04001D31 RID: 7473
		BinarySort = 32768,
		// Token: 0x04001D32 RID: 7474
		BinarySort2 = 16384
	}
}
