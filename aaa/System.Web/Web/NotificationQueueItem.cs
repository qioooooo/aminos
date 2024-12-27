using System;

namespace System.Web
{
	// Token: 0x02000035 RID: 53
	internal sealed class NotificationQueueItem
	{
		// Token: 0x06000114 RID: 276 RVA: 0x00005DA0 File Offset: 0x00004DA0
		internal NotificationQueueItem(FileChangeEventHandler callback, FileAction action, string filename)
		{
			this.Callback = callback;
			this.Action = action;
			this.Filename = filename;
		}

		// Token: 0x04000DBF RID: 3519
		internal readonly FileChangeEventHandler Callback;

		// Token: 0x04000DC0 RID: 3520
		internal readonly string Filename;

		// Token: 0x04000DC1 RID: 3521
		internal readonly FileAction Action;
	}
}
