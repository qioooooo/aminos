using System;

namespace System.ServiceProcess
{
	// Token: 0x02000035 RID: 53
	public enum SessionChangeReason
	{
		// Token: 0x04000235 RID: 565
		ConsoleConnect = 1,
		// Token: 0x04000236 RID: 566
		ConsoleDisconnect,
		// Token: 0x04000237 RID: 567
		RemoteConnect,
		// Token: 0x04000238 RID: 568
		RemoteDisconnect,
		// Token: 0x04000239 RID: 569
		SessionLogon,
		// Token: 0x0400023A RID: 570
		SessionLogoff,
		// Token: 0x0400023B RID: 571
		SessionLock,
		// Token: 0x0400023C RID: 572
		SessionUnlock,
		// Token: 0x0400023D RID: 573
		SessionRemoteControl
	}
}
