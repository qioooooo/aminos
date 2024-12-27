using System;

namespace System.Windows.Forms
{
	// Token: 0x020001E6 RID: 486
	public interface IDropTarget
	{
		// Token: 0x0600133E RID: 4926
		void OnDragEnter(DragEventArgs e);

		// Token: 0x0600133F RID: 4927
		void OnDragLeave(EventArgs e);

		// Token: 0x06001340 RID: 4928
		void OnDragDrop(DragEventArgs e);

		// Token: 0x06001341 RID: 4929
		void OnDragOver(DragEventArgs e);
	}
}
