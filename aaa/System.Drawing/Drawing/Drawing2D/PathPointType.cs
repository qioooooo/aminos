using System;

namespace System.Drawing.Drawing2D
{
	// Token: 0x020000D5 RID: 213
	public enum PathPointType
	{
		// Token: 0x04000AC9 RID: 2761
		Start,
		// Token: 0x04000ACA RID: 2762
		Line,
		// Token: 0x04000ACB RID: 2763
		Bezier = 3,
		// Token: 0x04000ACC RID: 2764
		PathTypeMask = 7,
		// Token: 0x04000ACD RID: 2765
		DashMode = 16,
		// Token: 0x04000ACE RID: 2766
		PathMarker = 32,
		// Token: 0x04000ACF RID: 2767
		CloseSubpath = 128,
		// Token: 0x04000AD0 RID: 2768
		Bezier3 = 3
	}
}
