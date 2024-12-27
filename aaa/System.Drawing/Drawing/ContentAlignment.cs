using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Drawing
{
	// Token: 0x0200003C RID: 60
	[Editor("System.Drawing.Design.ContentAlignmentEditor, System.Drawing.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public enum ContentAlignment
	{
		// Token: 0x04000280 RID: 640
		TopLeft = 1,
		// Token: 0x04000281 RID: 641
		TopCenter,
		// Token: 0x04000282 RID: 642
		TopRight = 4,
		// Token: 0x04000283 RID: 643
		MiddleLeft = 16,
		// Token: 0x04000284 RID: 644
		MiddleCenter = 32,
		// Token: 0x04000285 RID: 645
		MiddleRight = 64,
		// Token: 0x04000286 RID: 646
		BottomLeft = 256,
		// Token: 0x04000287 RID: 647
		BottomCenter = 512,
		// Token: 0x04000288 RID: 648
		BottomRight = 1024
	}
}
