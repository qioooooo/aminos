using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x0200031A RID: 794
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum PortableExecutableKinds
	{
		// Token: 0x04000D1D RID: 3357
		NotAPortableExecutableImage = 0,
		// Token: 0x04000D1E RID: 3358
		ILOnly = 1,
		// Token: 0x04000D1F RID: 3359
		Required32Bit = 2,
		// Token: 0x04000D20 RID: 3360
		PE32Plus = 4,
		// Token: 0x04000D21 RID: 3361
		Unmanaged32Bit = 8
	}
}
