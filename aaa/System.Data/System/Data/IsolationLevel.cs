using System;

namespace System.Data
{
	// Token: 0x020000C1 RID: 193
	public enum IsolationLevel
	{
		// Token: 0x0400089B RID: 2203
		Unspecified = -1,
		// Token: 0x0400089C RID: 2204
		Chaos = 16,
		// Token: 0x0400089D RID: 2205
		ReadUncommitted = 256,
		// Token: 0x0400089E RID: 2206
		ReadCommitted = 4096,
		// Token: 0x0400089F RID: 2207
		RepeatableRead = 65536,
		// Token: 0x040008A0 RID: 2208
		Serializable = 1048576,
		// Token: 0x040008A1 RID: 2209
		Snapshot = 16777216
	}
}
