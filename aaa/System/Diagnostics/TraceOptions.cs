using System;

namespace System.Diagnostics
{
	// Token: 0x020001DB RID: 475
	[Flags]
	public enum TraceOptions
	{
		// Token: 0x04000F36 RID: 3894
		None = 0,
		// Token: 0x04000F37 RID: 3895
		LogicalOperationStack = 1,
		// Token: 0x04000F38 RID: 3896
		DateTime = 2,
		// Token: 0x04000F39 RID: 3897
		Timestamp = 4,
		// Token: 0x04000F3A RID: 3898
		ProcessId = 8,
		// Token: 0x04000F3B RID: 3899
		ThreadId = 16,
		// Token: 0x04000F3C RID: 3900
		Callstack = 32
	}
}
