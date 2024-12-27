using System;
using System.Runtime.InteropServices;

namespace System.Reflection
{
	// Token: 0x020002E1 RID: 737
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum CallingConventions
	{
		// Token: 0x04000ABA RID: 2746
		Standard = 1,
		// Token: 0x04000ABB RID: 2747
		VarArgs = 2,
		// Token: 0x04000ABC RID: 2748
		Any = 3,
		// Token: 0x04000ABD RID: 2749
		HasThis = 32,
		// Token: 0x04000ABE RID: 2750
		ExplicitThis = 64
	}
}
