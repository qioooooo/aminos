using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004D9 RID: 1241
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum TypeLibVarFlags
	{
		// Token: 0x040018D5 RID: 6357
		FReadOnly = 1,
		// Token: 0x040018D6 RID: 6358
		FSource = 2,
		// Token: 0x040018D7 RID: 6359
		FBindable = 4,
		// Token: 0x040018D8 RID: 6360
		FRequestEdit = 8,
		// Token: 0x040018D9 RID: 6361
		FDisplayBind = 16,
		// Token: 0x040018DA RID: 6362
		FDefaultBind = 32,
		// Token: 0x040018DB RID: 6363
		FHidden = 64,
		// Token: 0x040018DC RID: 6364
		FRestricted = 128,
		// Token: 0x040018DD RID: 6365
		FDefaultCollelem = 256,
		// Token: 0x040018DE RID: 6366
		FUiDefault = 512,
		// Token: 0x040018DF RID: 6367
		FNonBrowsable = 1024,
		// Token: 0x040018E0 RID: 6368
		FReplaceable = 2048,
		// Token: 0x040018E1 RID: 6369
		FImmediateBind = 4096
	}
}
