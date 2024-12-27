using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004D8 RID: 1240
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum TypeLibFuncFlags
	{
		// Token: 0x040018C7 RID: 6343
		FRestricted = 1,
		// Token: 0x040018C8 RID: 6344
		FSource = 2,
		// Token: 0x040018C9 RID: 6345
		FBindable = 4,
		// Token: 0x040018CA RID: 6346
		FRequestEdit = 8,
		// Token: 0x040018CB RID: 6347
		FDisplayBind = 16,
		// Token: 0x040018CC RID: 6348
		FDefaultBind = 32,
		// Token: 0x040018CD RID: 6349
		FHidden = 64,
		// Token: 0x040018CE RID: 6350
		FUsesGetLastError = 128,
		// Token: 0x040018CF RID: 6351
		FDefaultCollelem = 256,
		// Token: 0x040018D0 RID: 6352
		FUiDefault = 512,
		// Token: 0x040018D1 RID: 6353
		FNonBrowsable = 1024,
		// Token: 0x040018D2 RID: 6354
		FReplaceable = 2048,
		// Token: 0x040018D3 RID: 6355
		FImmediateBind = 4096
	}
}
