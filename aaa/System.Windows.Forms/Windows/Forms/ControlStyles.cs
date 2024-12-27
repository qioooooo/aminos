using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x020002B6 RID: 694
	[Flags]
	public enum ControlStyles
	{
		// Token: 0x04001629 RID: 5673
		ContainerControl = 1,
		// Token: 0x0400162A RID: 5674
		UserPaint = 2,
		// Token: 0x0400162B RID: 5675
		Opaque = 4,
		// Token: 0x0400162C RID: 5676
		ResizeRedraw = 16,
		// Token: 0x0400162D RID: 5677
		FixedWidth = 32,
		// Token: 0x0400162E RID: 5678
		FixedHeight = 64,
		// Token: 0x0400162F RID: 5679
		StandardClick = 256,
		// Token: 0x04001630 RID: 5680
		Selectable = 512,
		// Token: 0x04001631 RID: 5681
		UserMouse = 1024,
		// Token: 0x04001632 RID: 5682
		SupportsTransparentBackColor = 2048,
		// Token: 0x04001633 RID: 5683
		StandardDoubleClick = 4096,
		// Token: 0x04001634 RID: 5684
		AllPaintingInWmPaint = 8192,
		// Token: 0x04001635 RID: 5685
		CacheText = 16384,
		// Token: 0x04001636 RID: 5686
		EnableNotifyMessage = 32768,
		// Token: 0x04001637 RID: 5687
		[EditorBrowsable(EditorBrowsableState.Never)]
		DoubleBuffer = 65536,
		// Token: 0x04001638 RID: 5688
		OptimizedDoubleBuffer = 131072,
		// Token: 0x04001639 RID: 5689
		UseTextForAccessibility = 262144
	}
}
