using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002FD RID: 765
	[Flags]
	[ComVisible(true)]
	[Serializable]
	public enum ResourceLocation
	{
		// Token: 0x04000B22 RID: 2850
		Embedded = 1,
		// Token: 0x04000B23 RID: 2851
		ContainedInAnotherAssembly = 2,
		// Token: 0x04000B24 RID: 2852
		ContainedInManifestFile = 4
	}
}
