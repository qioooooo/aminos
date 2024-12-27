using System;

namespace System.Windows.Forms
{
	// Token: 0x02000278 RID: 632
	public enum CloseReason
	{
		// Token: 0x040014EC RID: 5356
		None,
		// Token: 0x040014ED RID: 5357
		WindowsShutDown,
		// Token: 0x040014EE RID: 5358
		MdiFormClosing,
		// Token: 0x040014EF RID: 5359
		UserClosing,
		// Token: 0x040014F0 RID: 5360
		TaskManagerClosing,
		// Token: 0x040014F1 RID: 5361
		FormOwnerClosing,
		// Token: 0x040014F2 RID: 5362
		ApplicationExitCall
	}
}
