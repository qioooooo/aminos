using System;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	internal interface ISelectionUIService
	{
		bool Visible { get; set; }

		event ContainerSelectorActiveEventHandler ContainerSelectorActive;

		void AssignSelectionUIHandler(object component, ISelectionUIHandler handler);

		void ClearSelectionUIHandler(object component, ISelectionUIHandler handler);

		bool BeginDrag(SelectionRules rules, int initialX, int initialY);

		bool Dragging { get; }

		void DragMoved(Rectangle offset);

		void EndDrag(bool cancel);

		object[] FilterSelection(object[] components, SelectionRules selectionRules);

		Size GetAdornmentDimensions(AdornmentType adornmentType);

		bool GetAdornmentHitTest(object component, Point pt);

		bool GetContainerSelected(object component);

		SelectionRules GetSelectionRules(object component);

		SelectionStyles GetSelectionStyle(object component);

		void SetContainerSelected(object component, bool selected);

		void SetSelectionStyle(object component, SelectionStyles style);

		void SyncSelection();

		void SyncComponent(object component);
	}
}
