using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms
{
	// Token: 0x020003B7 RID: 951
	[Editor("System.Windows.Forms.Design.DockEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public enum DockStyle
	{
		// Token: 0x04001CF2 RID: 7410
		None,
		// Token: 0x04001CF3 RID: 7411
		Top,
		// Token: 0x04001CF4 RID: 7412
		Bottom,
		// Token: 0x04001CF5 RID: 7413
		Left,
		// Token: 0x04001CF6 RID: 7414
		Right,
		// Token: 0x04001CF7 RID: 7415
		Fill
	}
}
