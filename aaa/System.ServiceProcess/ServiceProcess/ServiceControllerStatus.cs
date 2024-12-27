using System;

namespace System.ServiceProcess
{
	// Token: 0x0200002D RID: 45
	public enum ServiceControllerStatus
	{
		// Token: 0x0400020F RID: 527
		ContinuePending = 5,
		// Token: 0x04000210 RID: 528
		Paused = 7,
		// Token: 0x04000211 RID: 529
		PausePending = 6,
		// Token: 0x04000212 RID: 530
		Running = 4,
		// Token: 0x04000213 RID: 531
		StartPending = 2,
		// Token: 0x04000214 RID: 532
		Stopped = 1,
		// Token: 0x04000215 RID: 533
		StopPending = 3
	}
}
