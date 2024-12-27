using System;

namespace System.Data.Common
{
	// Token: 0x02000166 RID: 358
	[Flags]
	public enum SupportedJoinOperators
	{
		// Token: 0x04000CE8 RID: 3304
		None = 0,
		// Token: 0x04000CE9 RID: 3305
		Inner = 1,
		// Token: 0x04000CEA RID: 3306
		LeftOuter = 2,
		// Token: 0x04000CEB RID: 3307
		RightOuter = 4,
		// Token: 0x04000CEC RID: 3308
		FullOuter = 8
	}
}
