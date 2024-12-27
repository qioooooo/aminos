using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x020004DE RID: 1246
	[ComVisible(true)]
	[Serializable]
	public enum UnmanagedType
	{
		// Token: 0x04001913 RID: 6419
		Bool = 2,
		// Token: 0x04001914 RID: 6420
		I1,
		// Token: 0x04001915 RID: 6421
		U1,
		// Token: 0x04001916 RID: 6422
		I2,
		// Token: 0x04001917 RID: 6423
		U2,
		// Token: 0x04001918 RID: 6424
		I4,
		// Token: 0x04001919 RID: 6425
		U4,
		// Token: 0x0400191A RID: 6426
		I8,
		// Token: 0x0400191B RID: 6427
		U8,
		// Token: 0x0400191C RID: 6428
		R4,
		// Token: 0x0400191D RID: 6429
		R8,
		// Token: 0x0400191E RID: 6430
		Currency = 15,
		// Token: 0x0400191F RID: 6431
		BStr = 19,
		// Token: 0x04001920 RID: 6432
		LPStr,
		// Token: 0x04001921 RID: 6433
		LPWStr,
		// Token: 0x04001922 RID: 6434
		LPTStr,
		// Token: 0x04001923 RID: 6435
		ByValTStr,
		// Token: 0x04001924 RID: 6436
		IUnknown = 25,
		// Token: 0x04001925 RID: 6437
		IDispatch,
		// Token: 0x04001926 RID: 6438
		Struct,
		// Token: 0x04001927 RID: 6439
		Interface,
		// Token: 0x04001928 RID: 6440
		SafeArray,
		// Token: 0x04001929 RID: 6441
		ByValArray,
		// Token: 0x0400192A RID: 6442
		SysInt,
		// Token: 0x0400192B RID: 6443
		SysUInt,
		// Token: 0x0400192C RID: 6444
		VBByRefStr = 34,
		// Token: 0x0400192D RID: 6445
		AnsiBStr,
		// Token: 0x0400192E RID: 6446
		TBStr,
		// Token: 0x0400192F RID: 6447
		VariantBool,
		// Token: 0x04001930 RID: 6448
		FunctionPtr,
		// Token: 0x04001931 RID: 6449
		AsAny = 40,
		// Token: 0x04001932 RID: 6450
		LPArray = 42,
		// Token: 0x04001933 RID: 6451
		LPStruct,
		// Token: 0x04001934 RID: 6452
		CustomMarshaler,
		// Token: 0x04001935 RID: 6453
		Error
	}
}
