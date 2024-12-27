using System;

namespace Microsoft.Win32
{
	// Token: 0x020002A3 RID: 675
	public enum SessionSwitchReason
	{
		// Token: 0x040015BA RID: 5562
		ConsoleConnect = 1,
		// Token: 0x040015BB RID: 5563
		ConsoleDisconnect,
		// Token: 0x040015BC RID: 5564
		RemoteConnect,
		// Token: 0x040015BD RID: 5565
		RemoteDisconnect,
		// Token: 0x040015BE RID: 5566
		SessionLogon,
		// Token: 0x040015BF RID: 5567
		SessionLogoff,
		// Token: 0x040015C0 RID: 5568
		SessionLock,
		// Token: 0x040015C1 RID: 5569
		SessionUnlock,
		// Token: 0x040015C2 RID: 5570
		SessionRemoteControl
	}
}
