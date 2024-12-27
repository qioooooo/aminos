using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x020005A5 RID: 1445
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum FileShare
	{
		// Token: 0x04001BFA RID: 7162
		None = 0,
		// Token: 0x04001BFB RID: 7163
		Read = 1,
		// Token: 0x04001BFC RID: 7164
		Write = 2,
		// Token: 0x04001BFD RID: 7165
		ReadWrite = 3,
		// Token: 0x04001BFE RID: 7166
		Delete = 4,
		// Token: 0x04001BFF RID: 7167
		Inheritable = 16
	}
}
