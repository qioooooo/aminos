using System;

namespace System.Data
{
	// Token: 0x02000059 RID: 89
	internal enum AggregateType
	{
		// Token: 0x0400069D RID: 1693
		None,
		// Token: 0x0400069E RID: 1694
		Sum = 4,
		// Token: 0x0400069F RID: 1695
		Mean,
		// Token: 0x040006A0 RID: 1696
		Min,
		// Token: 0x040006A1 RID: 1697
		Max,
		// Token: 0x040006A2 RID: 1698
		First,
		// Token: 0x040006A3 RID: 1699
		Count,
		// Token: 0x040006A4 RID: 1700
		Var,
		// Token: 0x040006A5 RID: 1701
		StDev
	}
}
