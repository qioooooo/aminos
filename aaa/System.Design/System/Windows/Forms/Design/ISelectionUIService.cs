using System;
using System.Drawing;

namespace System.Windows.Forms.Design
{
	// Token: 0x02000257 RID: 599
	internal interface ISelectionUIService
	{
		// Token: 0x170003E8 RID: 1000
		// (get) Token: 0x060016C6 RID: 5830
		// (set) Token: 0x060016C7 RID: 5831
		bool Visible { get; set; }

		// Token: 0x14000016 RID: 22
		// (add) Token: 0x060016C8 RID: 5832
		// (remove) Token: 0x060016C9 RID: 5833
		event ContainerSelectorActiveEventHandler ContainerSelectorActive;

		// Token: 0x060016CA RID: 5834
		void AssignSelectionUIHandler(object component, ISelectionUIHandler handler);

		// Token: 0x060016CB RID: 5835
		void ClearSelectionUIHandler(object component, ISelectionUIHandler handler);

		// Token: 0x060016CC RID: 5836
		bool BeginDrag(SelectionRules rules, int initialX, int initialY);

		// Token: 0x170003E9 RID: 1001
		// (get) Token: 0x060016CD RID: 5837
		bool Dragging { get; }

		// Token: 0x060016CE RID: 5838
		void DragMoved(Rectangle offset);

		// Token: 0x060016CF RID: 5839
		void EndDrag(bool cancel);

		// Token: 0x060016D0 RID: 5840
		object[] FilterSelection(object[] components, SelectionRules selectionRules);

		// Token: 0x060016D1 RID: 5841
		Size GetAdornmentDimensions(AdornmentType adornmentType);

		// Token: 0x060016D2 RID: 5842
		bool GetAdornmentHitTest(object component, Point pt);

		// Token: 0x060016D3 RID: 5843
		bool GetContainerSelected(object component);

		// Token: 0x060016D4 RID: 5844
		SelectionRules GetSelectionRules(object component);

		// Token: 0x060016D5 RID: 5845
		SelectionStyles GetSelectionStyle(object component);

		// Token: 0x060016D6 RID: 5846
		void SetContainerSelected(object component, bool selected);

		// Token: 0x060016D7 RID: 5847
		void SetSelectionStyle(object component, SelectionStyles style);

		// Token: 0x060016D8 RID: 5848
		void SyncSelection();

		// Token: 0x060016D9 RID: 5849
		void SyncComponent(object component);
	}
}
