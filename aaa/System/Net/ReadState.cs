using System;

namespace System.Net
{
	// Token: 0x020004C0 RID: 1216
	internal enum ReadState
	{
		// Token: 0x04002553 RID: 9555
		Start,
		// Token: 0x04002554 RID: 9556
		StatusLine,
		// Token: 0x04002555 RID: 9557
		Headers,
		// Token: 0x04002556 RID: 9558
		Data
	}
}
