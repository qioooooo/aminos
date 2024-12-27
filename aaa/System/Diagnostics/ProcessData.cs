using System;

namespace System.Diagnostics
{
	// Token: 0x02000797 RID: 1943
	internal class ProcessData
	{
		// Token: 0x06003BED RID: 15341 RVA: 0x001004F4 File Offset: 0x000FF4F4
		public ProcessData(int pid, long startTime)
		{
			this.ProcessId = pid;
			this.StartupTime = startTime;
		}

		// Token: 0x0400348A RID: 13450
		public int ProcessId;

		// Token: 0x0400348B RID: 13451
		public long StartupTime;
	}
}
