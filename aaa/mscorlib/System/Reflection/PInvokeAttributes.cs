using System;

namespace System.Reflection
{
	// Token: 0x02000303 RID: 771
	[Flags]
	[Serializable]
	internal enum PInvokeAttributes
	{
		// Token: 0x04000B66 RID: 2918
		NoMangle = 1,
		// Token: 0x04000B67 RID: 2919
		CharSetMask = 6,
		// Token: 0x04000B68 RID: 2920
		CharSetNotSpec = 0,
		// Token: 0x04000B69 RID: 2921
		CharSetAnsi = 2,
		// Token: 0x04000B6A RID: 2922
		CharSetUnicode = 4,
		// Token: 0x04000B6B RID: 2923
		CharSetAuto = 6,
		// Token: 0x04000B6C RID: 2924
		BestFitUseAssem = 0,
		// Token: 0x04000B6D RID: 2925
		BestFitEnabled = 16,
		// Token: 0x04000B6E RID: 2926
		BestFitDisabled = 32,
		// Token: 0x04000B6F RID: 2927
		BestFitMask = 48,
		// Token: 0x04000B70 RID: 2928
		ThrowOnUnmappableCharUseAssem = 0,
		// Token: 0x04000B71 RID: 2929
		ThrowOnUnmappableCharEnabled = 4096,
		// Token: 0x04000B72 RID: 2930
		ThrowOnUnmappableCharDisabled = 8192,
		// Token: 0x04000B73 RID: 2931
		ThrowOnUnmappableCharMask = 12288,
		// Token: 0x04000B74 RID: 2932
		SupportsLastError = 64,
		// Token: 0x04000B75 RID: 2933
		CallConvMask = 1792,
		// Token: 0x04000B76 RID: 2934
		CallConvWinapi = 256,
		// Token: 0x04000B77 RID: 2935
		CallConvCdecl = 512,
		// Token: 0x04000B78 RID: 2936
		CallConvStdcall = 768,
		// Token: 0x04000B79 RID: 2937
		CallConvThiscall = 1024,
		// Token: 0x04000B7A RID: 2938
		CallConvFastcall = 1280,
		// Token: 0x04000B7B RID: 2939
		MaxValue = 65535
	}
}
