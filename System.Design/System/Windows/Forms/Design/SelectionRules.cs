using System;

namespace System.Windows.Forms.Design
{
	[Flags]
	public enum SelectionRules
	{
		None = 0,
		Moveable = 268435456,
		Visible = 1073741824,
		Locked = -2147483648,
		TopSizeable = 1,
		BottomSizeable = 2,
		LeftSizeable = 4,
		RightSizeable = 8,
		AllSizeable = 15
	}
}
