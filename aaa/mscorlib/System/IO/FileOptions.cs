using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x020005A4 RID: 1444
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum FileOptions
	{
		// Token: 0x04001BF2 RID: 7154
		None = 0,
		// Token: 0x04001BF3 RID: 7155
		WriteThrough = -2147483648,
		// Token: 0x04001BF4 RID: 7156
		Asynchronous = 1073741824,
		// Token: 0x04001BF5 RID: 7157
		RandomAccess = 268435456,
		// Token: 0x04001BF6 RID: 7158
		DeleteOnClose = 67108864,
		// Token: 0x04001BF7 RID: 7159
		SequentialScan = 134217728,
		// Token: 0x04001BF8 RID: 7160
		Encrypted = 16384
	}
}
