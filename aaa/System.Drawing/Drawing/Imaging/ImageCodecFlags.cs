using System;

namespace System.Drawing.Imaging
{
	// Token: 0x020000BC RID: 188
	[Flags]
	public enum ImageCodecFlags
	{
		// Token: 0x04000A23 RID: 2595
		Encoder = 1,
		// Token: 0x04000A24 RID: 2596
		Decoder = 2,
		// Token: 0x04000A25 RID: 2597
		SupportBitmap = 4,
		// Token: 0x04000A26 RID: 2598
		SupportVector = 8,
		// Token: 0x04000A27 RID: 2599
		SeekableEncode = 16,
		// Token: 0x04000A28 RID: 2600
		BlockingDecode = 32,
		// Token: 0x04000A29 RID: 2601
		Builtin = 65536,
		// Token: 0x04000A2A RID: 2602
		System = 131072,
		// Token: 0x04000A2B RID: 2603
		User = 262144
	}
}
