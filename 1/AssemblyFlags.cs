using System;

namespace Microsoft.JScript
{
	// Token: 0x0200008B RID: 139
	internal enum AssemblyFlags
	{
		// Token: 0x040002EF RID: 751
		PublicKey = 1,
		// Token: 0x040002F0 RID: 752
		CompatibilityMask = 112,
		// Token: 0x040002F1 RID: 753
		SideBySideCompatible = 0,
		// Token: 0x040002F2 RID: 754
		NonSideBySideAppDomain = 16,
		// Token: 0x040002F3 RID: 755
		NonSideBySideProcess = 32,
		// Token: 0x040002F4 RID: 756
		NonSideBySideMachine = 48,
		// Token: 0x040002F5 RID: 757
		EnableJITcompileTracking = 32768,
		// Token: 0x040002F6 RID: 758
		DisableJITcompileOptimizer = 16384
	}
}
