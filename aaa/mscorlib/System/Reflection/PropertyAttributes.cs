using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000326 RID: 806
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum PropertyAttributes
	{
		// Token: 0x04000D60 RID: 3424
		None = 0,
		// Token: 0x04000D61 RID: 3425
		SpecialName = 512,
		// Token: 0x04000D62 RID: 3426
		ReservedMask = 62464,
		// Token: 0x04000D63 RID: 3427
		RTSpecialName = 1024,
		// Token: 0x04000D64 RID: 3428
		HasDefault = 4096,
		// Token: 0x04000D65 RID: 3429
		Reserved2 = 8192,
		// Token: 0x04000D66 RID: 3430
		Reserved3 = 16384,
		// Token: 0x04000D67 RID: 3431
		Reserved4 = 32768
	}
}
