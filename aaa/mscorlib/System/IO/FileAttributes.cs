using System;
using System.Runtime.InteropServices;

namespace System.IO
{
	// Token: 0x020005A8 RID: 1448
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum FileAttributes
	{
		// Token: 0x04001C2C RID: 7212
		ReadOnly = 1,
		// Token: 0x04001C2D RID: 7213
		Hidden = 2,
		// Token: 0x04001C2E RID: 7214
		System = 4,
		// Token: 0x04001C2F RID: 7215
		Directory = 16,
		// Token: 0x04001C30 RID: 7216
		Archive = 32,
		// Token: 0x04001C31 RID: 7217
		Device = 64,
		// Token: 0x04001C32 RID: 7218
		Normal = 128,
		// Token: 0x04001C33 RID: 7219
		Temporary = 256,
		// Token: 0x04001C34 RID: 7220
		SparseFile = 512,
		// Token: 0x04001C35 RID: 7221
		ReparsePoint = 1024,
		// Token: 0x04001C36 RID: 7222
		Compressed = 2048,
		// Token: 0x04001C37 RID: 7223
		Offline = 4096,
		// Token: 0x04001C38 RID: 7224
		NotContentIndexed = 8192,
		// Token: 0x04001C39 RID: 7225
		Encrypted = 16384
	}
}
