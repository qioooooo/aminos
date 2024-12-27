using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x02000323 RID: 803
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum ParameterAttributes
	{
		// Token: 0x04000D51 RID: 3409
		None = 0,
		// Token: 0x04000D52 RID: 3410
		In = 1,
		// Token: 0x04000D53 RID: 3411
		Out = 2,
		// Token: 0x04000D54 RID: 3412
		Lcid = 4,
		// Token: 0x04000D55 RID: 3413
		Retval = 8,
		// Token: 0x04000D56 RID: 3414
		Optional = 16,
		// Token: 0x04000D57 RID: 3415
		ReservedMask = 61440,
		// Token: 0x04000D58 RID: 3416
		HasDefault = 4096,
		// Token: 0x04000D59 RID: 3417
		HasFieldMarshal = 8192,
		// Token: 0x04000D5A RID: 3418
		Reserved3 = 16384,
		// Token: 0x04000D5B RID: 3419
		Reserved4 = 32768
	}
}
