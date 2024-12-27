using System;

namespace System.Net
{
	// Token: 0x020004F6 RID: 1270
	[Flags]
	internal enum ContextFlags
	{
		// Token: 0x040026D7 RID: 9943
		Zero = 0,
		// Token: 0x040026D8 RID: 9944
		Delegate = 1,
		// Token: 0x040026D9 RID: 9945
		MutualAuth = 2,
		// Token: 0x040026DA RID: 9946
		ReplayDetect = 4,
		// Token: 0x040026DB RID: 9947
		SequenceDetect = 8,
		// Token: 0x040026DC RID: 9948
		Confidentiality = 16,
		// Token: 0x040026DD RID: 9949
		UseSessionKey = 32,
		// Token: 0x040026DE RID: 9950
		AllocateMemory = 256,
		// Token: 0x040026DF RID: 9951
		Connection = 2048,
		// Token: 0x040026E0 RID: 9952
		InitExtendedError = 16384,
		// Token: 0x040026E1 RID: 9953
		AcceptExtendedError = 32768,
		// Token: 0x040026E2 RID: 9954
		InitStream = 32768,
		// Token: 0x040026E3 RID: 9955
		AcceptStream = 65536,
		// Token: 0x040026E4 RID: 9956
		InitIntegrity = 65536,
		// Token: 0x040026E5 RID: 9957
		AcceptIntegrity = 131072,
		// Token: 0x040026E6 RID: 9958
		InitManualCredValidation = 524288,
		// Token: 0x040026E7 RID: 9959
		InitUseSuppliedCreds = 128,
		// Token: 0x040026E8 RID: 9960
		InitIdentify = 131072,
		// Token: 0x040026E9 RID: 9961
		AcceptIdentify = 524288,
		// Token: 0x040026EA RID: 9962
		ProxyBindings = 67108864,
		// Token: 0x040026EB RID: 9963
		AllowMissingBindings = 268435456
	}
}
