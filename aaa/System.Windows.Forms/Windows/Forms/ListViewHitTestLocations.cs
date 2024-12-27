using System;

namespace System.Windows.Forms
{
	// Token: 0x02000490 RID: 1168
	[Flags]
	public enum ListViewHitTestLocations
	{
		// Token: 0x0400216E RID: 8558
		None = 1,
		// Token: 0x0400216F RID: 8559
		AboveClientArea = 256,
		// Token: 0x04002170 RID: 8560
		BelowClientArea = 16,
		// Token: 0x04002171 RID: 8561
		LeftOfClientArea = 64,
		// Token: 0x04002172 RID: 8562
		RightOfClientArea = 32,
		// Token: 0x04002173 RID: 8563
		Image = 2,
		// Token: 0x04002174 RID: 8564
		StateImage = 512,
		// Token: 0x04002175 RID: 8565
		Label = 4
	}
}
