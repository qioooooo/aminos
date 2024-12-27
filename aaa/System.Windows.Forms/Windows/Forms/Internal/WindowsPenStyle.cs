using System;

namespace System.Windows.Forms.Internal
{
	// Token: 0x0200002C RID: 44
	[Flags]
	internal enum WindowsPenStyle
	{
		// Token: 0x04000AE6 RID: 2790
		Solid = 0,
		// Token: 0x04000AE7 RID: 2791
		Dash = 1,
		// Token: 0x04000AE8 RID: 2792
		Dot = 2,
		// Token: 0x04000AE9 RID: 2793
		DashDot = 3,
		// Token: 0x04000AEA RID: 2794
		DashDotDot = 4,
		// Token: 0x04000AEB RID: 2795
		Null = 5,
		// Token: 0x04000AEC RID: 2796
		InsideFrame = 6,
		// Token: 0x04000AED RID: 2797
		UserStyle = 7,
		// Token: 0x04000AEE RID: 2798
		Alternate = 8,
		// Token: 0x04000AEF RID: 2799
		EndcapRound = 0,
		// Token: 0x04000AF0 RID: 2800
		EndcapSquare = 256,
		// Token: 0x04000AF1 RID: 2801
		EndcapFlat = 512,
		// Token: 0x04000AF2 RID: 2802
		JoinRound = 0,
		// Token: 0x04000AF3 RID: 2803
		JoinBevel = 4096,
		// Token: 0x04000AF4 RID: 2804
		JoinMiter = 8192,
		// Token: 0x04000AF5 RID: 2805
		Cosmetic = 0,
		// Token: 0x04000AF6 RID: 2806
		Geometric = 65536,
		// Token: 0x04000AF7 RID: 2807
		Default = 0
	}
}
