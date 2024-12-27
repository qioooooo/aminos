using System;

namespace System.Windows.Forms.Internal
{
	// Token: 0x02000010 RID: 16
	[Flags]
	internal enum DeviceContextLayout
	{
		// Token: 0x040009C3 RID: 2499
		Normal = 0,
		// Token: 0x040009C4 RID: 2500
		RightToLeft = 1,
		// Token: 0x040009C5 RID: 2501
		BottomToTop = 2,
		// Token: 0x040009C6 RID: 2502
		VerticalBeforeHorizontal = 4,
		// Token: 0x040009C7 RID: 2503
		BitmapOrientationPreserved = 8
	}
}
