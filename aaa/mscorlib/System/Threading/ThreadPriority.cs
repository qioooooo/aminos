using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	// Token: 0x02000163 RID: 355
	[ComVisible(true)]
	[Serializable]
	public enum ThreadPriority
	{
		// Token: 0x04000678 RID: 1656
		Lowest,
		// Token: 0x04000679 RID: 1657
		BelowNormal,
		// Token: 0x0400067A RID: 1658
		Normal,
		// Token: 0x0400067B RID: 1659
		AboveNormal,
		// Token: 0x0400067C RID: 1660
		Highest
	}
}
