using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002DD RID: 733
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum AssemblyNameFlags
	{
		// Token: 0x04000A99 RID: 2713
		None = 0,
		// Token: 0x04000A9A RID: 2714
		PublicKey = 1,
		// Token: 0x04000A9B RID: 2715
		EnableJITcompileOptimizer = 16384,
		// Token: 0x04000A9C RID: 2716
		EnableJITcompileTracking = 32768,
		// Token: 0x04000A9D RID: 2717
		Retargetable = 256
	}
}
