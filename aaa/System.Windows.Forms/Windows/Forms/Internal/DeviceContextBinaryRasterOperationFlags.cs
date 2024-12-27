using System;

namespace System.Windows.Forms.Internal
{
	// Token: 0x0200000E RID: 14
	[Flags]
	internal enum DeviceContextBinaryRasterOperationFlags
	{
		// Token: 0x040009AE RID: 2478
		Black = 1,
		// Token: 0x040009AF RID: 2479
		NotMergePen = 2,
		// Token: 0x040009B0 RID: 2480
		MaskNotPen = 3,
		// Token: 0x040009B1 RID: 2481
		NotCopyPen = 4,
		// Token: 0x040009B2 RID: 2482
		MaskPenNot = 5,
		// Token: 0x040009B3 RID: 2483
		Not = 6,
		// Token: 0x040009B4 RID: 2484
		XorPen = 7,
		// Token: 0x040009B5 RID: 2485
		NotMaskPen = 8,
		// Token: 0x040009B6 RID: 2486
		MaskPen = 9,
		// Token: 0x040009B7 RID: 2487
		NotXorPen = 10,
		// Token: 0x040009B8 RID: 2488
		Nop = 11,
		// Token: 0x040009B9 RID: 2489
		MergeNotPen = 12,
		// Token: 0x040009BA RID: 2490
		CopyPen = 13,
		// Token: 0x040009BB RID: 2491
		MergePenNot = 14,
		// Token: 0x040009BC RID: 2492
		MergePen = 15,
		// Token: 0x040009BD RID: 2493
		White = 16
	}
}
