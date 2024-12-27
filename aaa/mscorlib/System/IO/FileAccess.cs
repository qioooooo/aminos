using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x0200059F RID: 1439
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum FileAccess
	{
		// Token: 0x04001BE2 RID: 7138
		Read = 1,
		// Token: 0x04001BE3 RID: 7139
		Write = 2,
		// Token: 0x04001BE4 RID: 7140
		ReadWrite = 3
	}
}
