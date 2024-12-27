using System;

namespace System.Reflection
{
	// Token: 0x02000302 RID: 770
	[Flags]
	[Serializable]
	internal enum MdSigCallingConvention : byte
	{
		// Token: 0x04000B56 RID: 2902
		CallConvMask = 15,
		// Token: 0x04000B57 RID: 2903
		Default = 0,
		// Token: 0x04000B58 RID: 2904
		C = 1,
		// Token: 0x04000B59 RID: 2905
		StdCall = 2,
		// Token: 0x04000B5A RID: 2906
		ThisCall = 3,
		// Token: 0x04000B5B RID: 2907
		FastCall = 4,
		// Token: 0x04000B5C RID: 2908
		Vararg = 5,
		// Token: 0x04000B5D RID: 2909
		Field = 6,
		// Token: 0x04000B5E RID: 2910
		LoclaSig = 7,
		// Token: 0x04000B5F RID: 2911
		Property = 8,
		// Token: 0x04000B60 RID: 2912
		Unmgd = 9,
		// Token: 0x04000B61 RID: 2913
		GenericInst = 10,
		// Token: 0x04000B62 RID: 2914
		Generic = 16,
		// Token: 0x04000B63 RID: 2915
		HasThis = 32,
		// Token: 0x04000B64 RID: 2916
		ExplicitThis = 64
	}
}
