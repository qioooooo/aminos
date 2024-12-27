using System;

namespace System.Windows.Forms
{
	// Token: 0x020001D8 RID: 472
	[Flags]
	public enum AccessibleSelection
	{
		// Token: 0x04000FF2 RID: 4082
		None = 0,
		// Token: 0x04000FF3 RID: 4083
		TakeFocus = 1,
		// Token: 0x04000FF4 RID: 4084
		TakeSelection = 2,
		// Token: 0x04000FF5 RID: 4085
		ExtendSelection = 4,
		// Token: 0x04000FF6 RID: 4086
		AddSelection = 8,
		// Token: 0x04000FF7 RID: 4087
		RemoveSelection = 16
	}
}
