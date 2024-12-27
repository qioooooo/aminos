using System;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000285 RID: 645
	[Flags]
	public enum SelectionRules
	{
		// Token: 0x040013B7 RID: 5047
		None = 0,
		// Token: 0x040013B8 RID: 5048
		Moveable = 268435456,
		// Token: 0x040013B9 RID: 5049
		Visible = 1073741824,
		// Token: 0x040013BA RID: 5050
		Locked = -2147483648,
		// Token: 0x040013BB RID: 5051
		TopSizeable = 1,
		// Token: 0x040013BC RID: 5052
		BottomSizeable = 2,
		// Token: 0x040013BD RID: 5053
		LeftSizeable = 4,
		// Token: 0x040013BE RID: 5054
		RightSizeable = 8,
		// Token: 0x040013BF RID: 5055
		AllSizeable = 15
	}
}
