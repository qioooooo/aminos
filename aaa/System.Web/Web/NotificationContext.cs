using System;

namespace System.Web
{
	// Token: 0x020000B0 RID: 176
	internal class NotificationContext
	{
		// Token: 0x0600088C RID: 2188 RVA: 0x000264BB File Offset: 0x000254BB
		internal NotificationContext(int flags, bool isReEntry)
		{
			this.CurrentNotificationFlags = flags;
			this.IsReEntry = isReEntry;
		}

		// Token: 0x040011C2 RID: 4546
		internal bool IsPostNotification;

		// Token: 0x040011C3 RID: 4547
		internal RequestNotification CurrentNotification;

		// Token: 0x040011C4 RID: 4548
		internal int CurrentModuleIndex;

		// Token: 0x040011C5 RID: 4549
		internal int CurrentModuleEventIndex;

		// Token: 0x040011C6 RID: 4550
		internal int CurrentNotificationFlags;

		// Token: 0x040011C7 RID: 4551
		internal HttpAsyncResult AsyncResult;

		// Token: 0x040011C8 RID: 4552
		internal bool PendingAsyncCompletion;

		// Token: 0x040011C9 RID: 4553
		internal Exception Error;

		// Token: 0x040011CA RID: 4554
		internal bool RequestCompleted;

		// Token: 0x040011CB RID: 4555
		internal bool IsReEntry;
	}
}
