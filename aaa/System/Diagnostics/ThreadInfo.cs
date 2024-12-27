using System;

namespace System.Diagnostics
{
	// Token: 0x02000778 RID: 1912
	internal class ThreadInfo
	{
		// Token: 0x040033BF RID: 13247
		public int threadId;

		// Token: 0x040033C0 RID: 13248
		public int processId;

		// Token: 0x040033C1 RID: 13249
		public int basePriority;

		// Token: 0x040033C2 RID: 13250
		public int currentPriority;

		// Token: 0x040033C3 RID: 13251
		public IntPtr startAddress;

		// Token: 0x040033C4 RID: 13252
		public ThreadState threadState;

		// Token: 0x040033C5 RID: 13253
		public ThreadWaitReason threadWaitReason;
	}
}
