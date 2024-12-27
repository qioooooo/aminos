using System;

namespace System.Net
{
	// Token: 0x020003F9 RID: 1017
	internal enum BufferType
	{
		// Token: 0x0400202A RID: 8234
		Empty,
		// Token: 0x0400202B RID: 8235
		Data,
		// Token: 0x0400202C RID: 8236
		Token,
		// Token: 0x0400202D RID: 8237
		Parameters,
		// Token: 0x0400202E RID: 8238
		Missing,
		// Token: 0x0400202F RID: 8239
		Extra,
		// Token: 0x04002030 RID: 8240
		Trailer,
		// Token: 0x04002031 RID: 8241
		Header,
		// Token: 0x04002032 RID: 8242
		Padding = 9,
		// Token: 0x04002033 RID: 8243
		Stream,
		// Token: 0x04002034 RID: 8244
		ChannelBindings = 14,
		// Token: 0x04002035 RID: 8245
		TargetHost = 16,
		// Token: 0x04002036 RID: 8246
		ReadOnlyFlag = -2147483648,
		// Token: 0x04002037 RID: 8247
		ReadOnlyWithChecksum = 268435456
	}
}
