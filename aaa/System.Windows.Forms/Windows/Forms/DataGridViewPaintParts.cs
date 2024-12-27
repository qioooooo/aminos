using System;

namespace System.Windows.Forms
{
	// Token: 0x0200037C RID: 892
	[Flags]
	public enum DataGridViewPaintParts
	{
		// Token: 0x04001BF0 RID: 7152
		None = 0,
		// Token: 0x04001BF1 RID: 7153
		All = 127,
		// Token: 0x04001BF2 RID: 7154
		Background = 1,
		// Token: 0x04001BF3 RID: 7155
		Border = 2,
		// Token: 0x04001BF4 RID: 7156
		ContentBackground = 4,
		// Token: 0x04001BF5 RID: 7157
		ContentForeground = 8,
		// Token: 0x04001BF6 RID: 7158
		ErrorIcon = 16,
		// Token: 0x04001BF7 RID: 7159
		Focus = 32,
		// Token: 0x04001BF8 RID: 7160
		SelectionBackground = 64
	}
}
