using System;

namespace System.Windows.Forms
{
	// Token: 0x02000256 RID: 598
	[Flags]
	public enum BoundsSpecified
	{
		// Token: 0x04001434 RID: 5172
		X = 1,
		// Token: 0x04001435 RID: 5173
		Y = 2,
		// Token: 0x04001436 RID: 5174
		Width = 4,
		// Token: 0x04001437 RID: 5175
		Height = 8,
		// Token: 0x04001438 RID: 5176
		Location = 3,
		// Token: 0x04001439 RID: 5177
		Size = 12,
		// Token: 0x0400143A RID: 5178
		All = 15,
		// Token: 0x0400143B RID: 5179
		None = 0
	}
}
