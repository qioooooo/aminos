using System;
using System.Runtime.InteropServices;

namespace System.Threading
{
	// Token: 0x02000165 RID: 357
	[ComVisible(true)]
	[Flags]
	[Serializable]
	public enum ThreadState
	{
		// Token: 0x0400067E RID: 1662
		Running = 0,
		// Token: 0x0400067F RID: 1663
		StopRequested = 1,
		// Token: 0x04000680 RID: 1664
		SuspendRequested = 2,
		// Token: 0x04000681 RID: 1665
		Background = 4,
		// Token: 0x04000682 RID: 1666
		Unstarted = 8,
		// Token: 0x04000683 RID: 1667
		Stopped = 16,
		// Token: 0x04000684 RID: 1668
		WaitSleepJoin = 32,
		// Token: 0x04000685 RID: 1669
		Suspended = 64,
		// Token: 0x04000686 RID: 1670
		AbortRequested = 128,
		// Token: 0x04000687 RID: 1671
		Aborted = 256
	}
}
