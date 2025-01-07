using System;
using System.ComponentModel;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	internal interface ISelectionUIHandler
	{
		bool BeginDrag(object[] components, SelectionRules rules, int initialX, int initialY);

		void DragMoved(object[] components, Rectangle offset);

		void EndDrag(object[] components, bool cancel);

		Rectangle GetComponentBounds(object component);

		SelectionRules GetComponentRules(object component);

		Rectangle GetSelectionClipRect(object component);

		void OnSelectionDoubleClick(IComponent component);

		bool QueryBeginDrag(object[] components, SelectionRules rules, int initialX, int initialY);

		void ShowContextMenu(IComponent component);

		void OleDragEnter(DragEventArgs de);

		void OleDragDrop(DragEventArgs de);

		void OleDragOver(DragEventArgs de);

		void OleDragLeave();
	}
}
