using System;

namespace System.Windows.Forms
{
	// Token: 0x020003C6 RID: 966
	[Flags]
	public enum DragDropEffects
	{
		// Token: 0x04001D2F RID: 7471
		None = 0,
		// Token: 0x04001D30 RID: 7472
		Copy = 1,
		// Token: 0x04001D31 RID: 7473
		Move = 2,
		// Token: 0x04001D32 RID: 7474
		Link = 4,
		// Token: 0x04001D33 RID: 7475
		Scroll = -2147483648,
		// Token: 0x04001D34 RID: 7476
		All = -2147483645
	}
}
