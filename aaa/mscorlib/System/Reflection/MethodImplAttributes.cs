using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000318 RID: 792
	[ComVisible(true)]
	[Serializable]
	public enum MethodImplAttributes
	{
		// Token: 0x04000D0C RID: 3340
		CodeTypeMask = 3,
		// Token: 0x04000D0D RID: 3341
		IL = 0,
		// Token: 0x04000D0E RID: 3342
		Native,
		// Token: 0x04000D0F RID: 3343
		OPTIL,
		// Token: 0x04000D10 RID: 3344
		Runtime,
		// Token: 0x04000D11 RID: 3345
		ManagedMask,
		// Token: 0x04000D12 RID: 3346
		Unmanaged = 4,
		// Token: 0x04000D13 RID: 3347
		Managed = 0,
		// Token: 0x04000D14 RID: 3348
		ForwardRef = 16,
		// Token: 0x04000D15 RID: 3349
		PreserveSig = 128,
		// Token: 0x04000D16 RID: 3350
		InternalCall = 4096,
		// Token: 0x04000D17 RID: 3351
		Synchronized = 32,
		// Token: 0x04000D18 RID: 3352
		NoInlining = 8,
		// Token: 0x04000D19 RID: 3353
		NoOptimization = 64,
		// Token: 0x04000D1A RID: 3354
		MaxMethodImplVal = 65535
	}
}
