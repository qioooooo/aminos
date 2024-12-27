using System;
using System.Runtime.InteropServices;

namespace System.Runtime.CompilerServices
{
	// Token: 0x020005D5 RID: 1493
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum MethodImplOptions
	{
		// Token: 0x04001CA0 RID: 7328
		Unmanaged = 4,
		// Token: 0x04001CA1 RID: 7329
		ForwardRef = 16,
		// Token: 0x04001CA2 RID: 7330
		PreserveSig = 128,
		// Token: 0x04001CA3 RID: 7331
		InternalCall = 4096,
		// Token: 0x04001CA4 RID: 7332
		Synchronized = 32,
		// Token: 0x04001CA5 RID: 7333
		NoInlining = 8,
		// Token: 0x04001CA6 RID: 7334
		NoOptimization = 64
	}
}
