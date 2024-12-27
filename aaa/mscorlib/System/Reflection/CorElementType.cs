using System;

namespace System.Reflection
{
	// Token: 0x020002FF RID: 767
	[Serializable]
	internal enum CorElementType : byte
	{
		// Token: 0x04000B2E RID: 2862
		End,
		// Token: 0x04000B2F RID: 2863
		Void,
		// Token: 0x04000B30 RID: 2864
		Boolean,
		// Token: 0x04000B31 RID: 2865
		Char,
		// Token: 0x04000B32 RID: 2866
		I1,
		// Token: 0x04000B33 RID: 2867
		U1,
		// Token: 0x04000B34 RID: 2868
		I2,
		// Token: 0x04000B35 RID: 2869
		U2,
		// Token: 0x04000B36 RID: 2870
		I4,
		// Token: 0x04000B37 RID: 2871
		U4,
		// Token: 0x04000B38 RID: 2872
		I8,
		// Token: 0x04000B39 RID: 2873
		U8,
		// Token: 0x04000B3A RID: 2874
		R4,
		// Token: 0x04000B3B RID: 2875
		R8,
		// Token: 0x04000B3C RID: 2876
		String,
		// Token: 0x04000B3D RID: 2877
		Ptr,
		// Token: 0x04000B3E RID: 2878
		ByRef,
		// Token: 0x04000B3F RID: 2879
		ValueType,
		// Token: 0x04000B40 RID: 2880
		Class,
		// Token: 0x04000B41 RID: 2881
		Array = 20,
		// Token: 0x04000B42 RID: 2882
		TypedByRef = 22,
		// Token: 0x04000B43 RID: 2883
		I = 24,
		// Token: 0x04000B44 RID: 2884
		U,
		// Token: 0x04000B45 RID: 2885
		FnPtr = 27,
		// Token: 0x04000B46 RID: 2886
		Object,
		// Token: 0x04000B47 RID: 2887
		SzArray,
		// Token: 0x04000B48 RID: 2888
		CModReqd = 31,
		// Token: 0x04000B49 RID: 2889
		CModOpt,
		// Token: 0x04000B4A RID: 2890
		Internal,
		// Token: 0x04000B4B RID: 2891
		Modifier = 64,
		// Token: 0x04000B4C RID: 2892
		Sentinel,
		// Token: 0x04000B4D RID: 2893
		Pinned = 69
	}
}
