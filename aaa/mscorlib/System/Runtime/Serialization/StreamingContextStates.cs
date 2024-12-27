using System;
using System.Runtime.InteropServices;

namespace System.Runtime.Serialization
{
	// Token: 0x02000367 RID: 871
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum StreamingContextStates
	{
		// Token: 0x04000E61 RID: 3681
		CrossProcess = 1,
		// Token: 0x04000E62 RID: 3682
		CrossMachine = 2,
		// Token: 0x04000E63 RID: 3683
		File = 4,
		// Token: 0x04000E64 RID: 3684
		Persistence = 8,
		// Token: 0x04000E65 RID: 3685
		Remoting = 16,
		// Token: 0x04000E66 RID: 3686
		Other = 32,
		// Token: 0x04000E67 RID: 3687
		Clone = 64,
		// Token: 0x04000E68 RID: 3688
		CrossAppDomain = 128,
		// Token: 0x04000E69 RID: 3689
		All = 255
	}
}
