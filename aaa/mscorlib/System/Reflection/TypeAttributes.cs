using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x0200032D RID: 813
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum TypeAttributes
	{
		// Token: 0x04000D72 RID: 3442
		VisibilityMask = 7,
		// Token: 0x04000D73 RID: 3443
		NotPublic = 0,
		// Token: 0x04000D74 RID: 3444
		Public = 1,
		// Token: 0x04000D75 RID: 3445
		NestedPublic = 2,
		// Token: 0x04000D76 RID: 3446
		NestedPrivate = 3,
		// Token: 0x04000D77 RID: 3447
		NestedFamily = 4,
		// Token: 0x04000D78 RID: 3448
		NestedAssembly = 5,
		// Token: 0x04000D79 RID: 3449
		NestedFamANDAssem = 6,
		// Token: 0x04000D7A RID: 3450
		NestedFamORAssem = 7,
		// Token: 0x04000D7B RID: 3451
		LayoutMask = 24,
		// Token: 0x04000D7C RID: 3452
		AutoLayout = 0,
		// Token: 0x04000D7D RID: 3453
		SequentialLayout = 8,
		// Token: 0x04000D7E RID: 3454
		ExplicitLayout = 16,
		// Token: 0x04000D7F RID: 3455
		ClassSemanticsMask = 32,
		// Token: 0x04000D80 RID: 3456
		Class = 0,
		// Token: 0x04000D81 RID: 3457
		Interface = 32,
		// Token: 0x04000D82 RID: 3458
		Abstract = 128,
		// Token: 0x04000D83 RID: 3459
		Sealed = 256,
		// Token: 0x04000D84 RID: 3460
		SpecialName = 1024,
		// Token: 0x04000D85 RID: 3461
		Import = 4096,
		// Token: 0x04000D86 RID: 3462
		Serializable = 8192,
		// Token: 0x04000D87 RID: 3463
		StringFormatMask = 196608,
		// Token: 0x04000D88 RID: 3464
		AnsiClass = 0,
		// Token: 0x04000D89 RID: 3465
		UnicodeClass = 65536,
		// Token: 0x04000D8A RID: 3466
		AutoClass = 131072,
		// Token: 0x04000D8B RID: 3467
		CustomFormatClass = 196608,
		// Token: 0x04000D8C RID: 3468
		CustomFormatMask = 12582912,
		// Token: 0x04000D8D RID: 3469
		BeforeFieldInit = 1048576,
		// Token: 0x04000D8E RID: 3470
		ReservedMask = 264192,
		// Token: 0x04000D8F RID: 3471
		RTSpecialName = 2048,
		// Token: 0x04000D90 RID: 3472
		HasSecurity = 262144
	}
}
