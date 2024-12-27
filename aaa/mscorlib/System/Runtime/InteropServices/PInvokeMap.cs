using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000502 RID: 1282
	[Serializable]
	internal enum PInvokeMap
	{
		// Token: 0x0400198C RID: 6540
		NoMangle = 1,
		// Token: 0x0400198D RID: 6541
		CharSetMask = 6,
		// Token: 0x0400198E RID: 6542
		CharSetNotSpec = 0,
		// Token: 0x0400198F RID: 6543
		CharSetAnsi = 2,
		// Token: 0x04001990 RID: 6544
		CharSetUnicode = 4,
		// Token: 0x04001991 RID: 6545
		CharSetAuto = 6,
		// Token: 0x04001992 RID: 6546
		PinvokeOLE = 32,
		// Token: 0x04001993 RID: 6547
		SupportsLastError = 64,
		// Token: 0x04001994 RID: 6548
		BestFitMask = 48,
		// Token: 0x04001995 RID: 6549
		BestFitEnabled = 16,
		// Token: 0x04001996 RID: 6550
		BestFitDisabled = 32,
		// Token: 0x04001997 RID: 6551
		BestFitUseAsm = 48,
		// Token: 0x04001998 RID: 6552
		ThrowOnUnmappableCharMask = 12288,
		// Token: 0x04001999 RID: 6553
		ThrowOnUnmappableCharEnabled = 4096,
		// Token: 0x0400199A RID: 6554
		ThrowOnUnmappableCharDisabled = 8192,
		// Token: 0x0400199B RID: 6555
		ThrowOnUnmappableCharUseAsm = 12288,
		// Token: 0x0400199C RID: 6556
		CallConvMask = 1792,
		// Token: 0x0400199D RID: 6557
		CallConvWinapi = 256,
		// Token: 0x0400199E RID: 6558
		CallConvCdecl = 512,
		// Token: 0x0400199F RID: 6559
		CallConvStdcall = 768,
		// Token: 0x040019A0 RID: 6560
		CallConvThiscall = 1024,
		// Token: 0x040019A1 RID: 6561
		CallConvFastcall = 1280
	}
}
