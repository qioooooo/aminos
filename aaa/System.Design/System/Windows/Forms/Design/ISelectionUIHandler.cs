using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	// Token: 0x020001B5 RID: 437
	internal interface ISelectionUIHandler
	{
		// Token: 0x060010B6 RID: 4278
		bool BeginDrag(object[] components, SelectionRules rules, int initialX, int initialY);

		// Token: 0x060010B7 RID: 4279
		void DragMoved(object[] components, Rectangle offset);

		// Token: 0x060010B8 RID: 4280
		void EndDrag(object[] components, bool cancel);

		// Token: 0x060010B9 RID: 4281
		Rectangle GetComponentBounds(object component);

		// Token: 0x060010BA RID: 4282
		SelectionRules GetComponentRules(object component);

		// Token: 0x060010BB RID: 4283
		Rectangle GetSelectionClipRect(object component);

		// Token: 0x060010BC RID: 4284
		void OnSelectionDoubleClick(IComponent component);

		// Token: 0x060010BD RID: 4285
		bool QueryBeginDrag(object[] components, SelectionRules rules, int initialX, int initialY);

		// Token: 0x060010BE RID: 4286
		void ShowContextMenu(IComponent component);

		// Token: 0x060010BF RID: 4287
		void OleDragEnter(DragEventArgs de);

		// Token: 0x060010C0 RID: 4288
		void OleDragDrop(DragEventArgs de);

		// Token: 0x060010C1 RID: 4289
		void OleDragOver(DragEventArgs de);

		// Token: 0x060010C2 RID: 4290
		void OleDragLeave();
	}
}
