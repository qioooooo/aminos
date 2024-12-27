using System;

namespace System.Windows.Forms
{
	// Token: 0x02000346 RID: 838
	[Flags]
	public enum DataGridViewDataErrorContexts
	{
		// Token: 0x04001B87 RID: 7047
		Formatting = 1,
		// Token: 0x04001B88 RID: 7048
		Display = 2,
		// Token: 0x04001B89 RID: 7049
		PreferredSize = 4,
		// Token: 0x04001B8A RID: 7050
		RowDeletion = 8,
		// Token: 0x04001B8B RID: 7051
		Parsing = 256,
		// Token: 0x04001B8C RID: 7052
		Commit = 512,
		// Token: 0x04001B8D RID: 7053
		InitialValueRestoration = 1024,
		// Token: 0x04001B8E RID: 7054
		LeaveControl = 2048,
		// Token: 0x04001B8F RID: 7055
		CurrentCellChange = 4096,
		// Token: 0x04001B90 RID: 7056
		Scroll = 8192,
		// Token: 0x04001B91 RID: 7057
		ClipboardContent = 16384
	}
}
