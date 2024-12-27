using System;

namespace System.Diagnostics
{
	// Token: 0x0200079A RID: 1946
	public enum ThreadPriorityLevel
	{
		// Token: 0x0400349B RID: 13467
		Idle = -15,
		// Token: 0x0400349C RID: 13468
		Lowest = -2,
		// Token: 0x0400349D RID: 13469
		BelowNormal,
		// Token: 0x0400349E RID: 13470
		Normal,
		// Token: 0x0400349F RID: 13471
		AboveNormal,
		// Token: 0x040034A0 RID: 13472
		Highest,
		// Token: 0x040034A1 RID: 13473
		TimeCritical = 15
	}
}
