using System;
using System.Runtime.InteropServices;

namespace System.Windows.Forms
{
	// Token: 0x0200034A RID: 842
	[ComVisible(true)]
	[Flags]
	public enum DataGridViewElementStates
	{
		// Token: 0x04001B9E RID: 7070
		None = 0,
		// Token: 0x04001B9F RID: 7071
		Displayed = 1,
		// Token: 0x04001BA0 RID: 7072
		Frozen = 2,
		// Token: 0x04001BA1 RID: 7073
		ReadOnly = 4,
		// Token: 0x04001BA2 RID: 7074
		Resizable = 8,
		// Token: 0x04001BA3 RID: 7075
		ResizableSet = 16,
		// Token: 0x04001BA4 RID: 7076
		Selected = 32,
		// Token: 0x04001BA5 RID: 7077
		Visible = 64
	}
}
