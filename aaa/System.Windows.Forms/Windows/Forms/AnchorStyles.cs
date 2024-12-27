using System;
using System.ComponentModel;
using System.Drawing.Design;

namespace System.Windows.Forms
{
	// Token: 0x020001DC RID: 476
	[Flags]
	[Editor("System.Windows.Forms.Design.AnchorEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
	public enum AnchorStyles
	{
		// Token: 0x04001020 RID: 4128
		Top = 1,
		// Token: 0x04001021 RID: 4129
		Bottom = 2,
		// Token: 0x04001022 RID: 4130
		Left = 4,
		// Token: 0x04001023 RID: 4131
		Right = 8,
		// Token: 0x04001024 RID: 4132
		None = 0
	}
}
