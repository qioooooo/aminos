using System;

namespace System.ServiceProcess
{
	// Token: 0x0200001E RID: 30
	public enum PowerBroadcastStatus
	{
		// Token: 0x040001CF RID: 463
		BatteryLow = 9,
		// Token: 0x040001D0 RID: 464
		OemEvent = 11,
		// Token: 0x040001D1 RID: 465
		PowerStatusChange = 10,
		// Token: 0x040001D2 RID: 466
		QuerySuspend = 0,
		// Token: 0x040001D3 RID: 467
		QuerySuspendFailed = 2,
		// Token: 0x040001D4 RID: 468
		ResumeAutomatic = 18,
		// Token: 0x040001D5 RID: 469
		ResumeCritical = 6,
		// Token: 0x040001D6 RID: 470
		ResumeSuspend,
		// Token: 0x040001D7 RID: 471
		Suspend = 4
	}
}
