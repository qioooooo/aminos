using System;

namespace System.Web.Services.Interop
{
	// Token: 0x0200001F RID: 31
	internal enum NotifyFilter
	{
		// Token: 0x0400022D RID: 557
		OnSyncCallOut = 1,
		// Token: 0x0400022E RID: 558
		OnSyncCallEnter,
		// Token: 0x0400022F RID: 559
		OnSyncCallExit = 4,
		// Token: 0x04000230 RID: 560
		OnSyncCallReturn = 8,
		// Token: 0x04000231 RID: 561
		AllSync = 15,
		// Token: 0x04000232 RID: 562
		All = -1,
		// Token: 0x04000233 RID: 563
		None
	}
}
