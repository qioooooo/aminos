using System;

namespace System.Diagnostics
{
	// Token: 0x02000788 RID: 1928
	public enum ProcessPriorityClass
	{
		// Token: 0x04003423 RID: 13347
		Normal = 32,
		// Token: 0x04003424 RID: 13348
		Idle = 64,
		// Token: 0x04003425 RID: 13349
		High = 128,
		// Token: 0x04003426 RID: 13350
		RealTime = 256,
		// Token: 0x04003427 RID: 13351
		BelowNormal = 16384,
		// Token: 0x04003428 RID: 13352
		AboveNormal = 32768
	}
}
