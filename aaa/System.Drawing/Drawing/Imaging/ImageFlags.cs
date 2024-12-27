using System;

namespace System.Drawing.Imaging
{
	// Token: 0x020000BF RID: 191
	[Flags]
	public enum ImageFlags
	{
		// Token: 0x04000A45 RID: 2629
		None = 0,
		// Token: 0x04000A46 RID: 2630
		Scalable = 1,
		// Token: 0x04000A47 RID: 2631
		HasAlpha = 2,
		// Token: 0x04000A48 RID: 2632
		HasTranslucent = 4,
		// Token: 0x04000A49 RID: 2633
		PartiallyScalable = 8,
		// Token: 0x04000A4A RID: 2634
		ColorSpaceRgb = 16,
		// Token: 0x04000A4B RID: 2635
		ColorSpaceCmyk = 32,
		// Token: 0x04000A4C RID: 2636
		ColorSpaceGray = 64,
		// Token: 0x04000A4D RID: 2637
		ColorSpaceYcbcr = 128,
		// Token: 0x04000A4E RID: 2638
		ColorSpaceYcck = 256,
		// Token: 0x04000A4F RID: 2639
		HasRealDpi = 4096,
		// Token: 0x04000A50 RID: 2640
		HasRealPixelSize = 8192,
		// Token: 0x04000A51 RID: 2641
		ReadOnly = 65536,
		// Token: 0x04000A52 RID: 2642
		Caching = 131072
	}
}
