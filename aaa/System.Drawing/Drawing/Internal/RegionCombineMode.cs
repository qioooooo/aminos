using System;

namespace System.Drawing.Internal
{
	// Token: 0x02000021 RID: 33
	internal enum RegionCombineMode
	{
		// Token: 0x04000112 RID: 274
		AND = 1,
		// Token: 0x04000113 RID: 275
		OR,
		// Token: 0x04000114 RID: 276
		XOR,
		// Token: 0x04000115 RID: 277
		DIFF,
		// Token: 0x04000116 RID: 278
		COPY,
		// Token: 0x04000117 RID: 279
		MIN = 1,
		// Token: 0x04000118 RID: 280
		MAX = 5
	}
}
