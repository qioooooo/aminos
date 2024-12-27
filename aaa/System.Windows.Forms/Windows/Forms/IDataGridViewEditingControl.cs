using System;

namespace System.Windows.Forms
{
	// Token: 0x02000343 RID: 835
	public interface IDataGridViewEditingControl
	{
		// Token: 0x170009C3 RID: 2499
		// (get) Token: 0x06003564 RID: 13668
		// (set) Token: 0x06003565 RID: 13669
		DataGridView EditingControlDataGridView { get; set; }

		// Token: 0x170009C4 RID: 2500
		// (get) Token: 0x06003566 RID: 13670
		// (set) Token: 0x06003567 RID: 13671
		object EditingControlFormattedValue { get; set; }

		// Token: 0x170009C5 RID: 2501
		// (get) Token: 0x06003568 RID: 13672
		// (set) Token: 0x06003569 RID: 13673
		int EditingControlRowIndex { get; set; }

		// Token: 0x170009C6 RID: 2502
		// (get) Token: 0x0600356A RID: 13674
		// (set) Token: 0x0600356B RID: 13675
		bool EditingControlValueChanged { get; set; }

		// Token: 0x170009C7 RID: 2503
		// (get) Token: 0x0600356C RID: 13676
		Cursor EditingPanelCursor { get; }

		// Token: 0x170009C8 RID: 2504
		// (get) Token: 0x0600356D RID: 13677
		bool RepositionEditingControlOnValueChange { get; }

		// Token: 0x0600356E RID: 13678
		void ApplyCellStyleToEditingControl(DataGridViewCellStyle dataGridViewCellStyle);

		// Token: 0x0600356F RID: 13679
		bool EditingControlWantsInputKey(Keys keyData, bool dataGridViewWantsInputKey);

		// Token: 0x06003570 RID: 13680
		object GetEditingControlFormattedValue(DataGridViewDataErrorContexts context);

		// Token: 0x06003571 RID: 13681
		void PrepareEditingControlForEdit(bool selectAll);
	}
}
