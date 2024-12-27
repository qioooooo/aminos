using System;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x020000A4 RID: 164
	[Flags]
	[Serializable]
	public enum LogRecordFlags
	{
		// Token: 0x040001C8 RID: 456
		ForgetTarget = 1,
		// Token: 0x040001C9 RID: 457
		WrittenDuringPrepare = 2,
		// Token: 0x040001CA RID: 458
		WrittenDuringCommit = 4,
		// Token: 0x040001CB RID: 459
		WrittenDuringAbort = 8,
		// Token: 0x040001CC RID: 460
		WrittenDurringRecovery = 16,
		// Token: 0x040001CD RID: 461
		WrittenDuringReplay = 32,
		// Token: 0x040001CE RID: 462
		ReplayInProgress = 64
	}
}
