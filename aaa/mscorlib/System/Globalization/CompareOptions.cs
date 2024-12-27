using System;
using System.Runtime.InteropServices;

namespace System.Globalization
{
	// Token: 0x0200037C RID: 892
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum CompareOptions
	{
		// Token: 0x04000F04 RID: 3844
		None = 0,
		// Token: 0x04000F05 RID: 3845
		IgnoreCase = 1,
		// Token: 0x04000F06 RID: 3846
		IgnoreNonSpace = 2,
		// Token: 0x04000F07 RID: 3847
		IgnoreSymbols = 4,
		// Token: 0x04000F08 RID: 3848
		IgnoreKanaType = 8,
		// Token: 0x04000F09 RID: 3849
		IgnoreWidth = 16,
		// Token: 0x04000F0A RID: 3850
		OrdinalIgnoreCase = 268435456,
		// Token: 0x04000F0B RID: 3851
		StringSort = 536870912,
		// Token: 0x04000F0C RID: 3852
		Ordinal = 1073741824
	}
}
