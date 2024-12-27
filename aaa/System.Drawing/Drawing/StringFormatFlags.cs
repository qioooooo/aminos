using System;

namespace System.Drawing
{
	// Token: 0x020000E9 RID: 233
	[Flags]
	public enum StringFormatFlags
	{
		// Token: 0x04000B25 RID: 2853
		DirectionRightToLeft = 1,
		// Token: 0x04000B26 RID: 2854
		DirectionVertical = 2,
		// Token: 0x04000B27 RID: 2855
		FitBlackBox = 4,
		// Token: 0x04000B28 RID: 2856
		DisplayFormatControl = 32,
		// Token: 0x04000B29 RID: 2857
		NoFontFallback = 1024,
		// Token: 0x04000B2A RID: 2858
		MeasureTrailingSpaces = 2048,
		// Token: 0x04000B2B RID: 2859
		NoWrap = 4096,
		// Token: 0x04000B2C RID: 2860
		LineLimit = 8192,
		// Token: 0x04000B2D RID: 2861
		NoClip = 16384
	}
}
