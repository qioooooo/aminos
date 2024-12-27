using System;

namespace System.Windows.Forms
{
	// Token: 0x02000329 RID: 809
	public interface IDataGridViewEditingCell
	{
		// Token: 0x17000949 RID: 2377
		// (get) Token: 0x060033CC RID: 13260
		// (set) Token: 0x060033CD RID: 13261
		object EditingCellFormattedValue { get; set; }

		// Token: 0x1700094A RID: 2378
		// (get) Token: 0x060033CE RID: 13262
		// (set) Token: 0x060033CF RID: 13263
		bool EditingCellValueChanged { get; set; }

		// Token: 0x060033D0 RID: 13264
		object GetEditingCellFormattedValue(DataGridViewDataErrorContexts context);

		// Token: 0x060033D1 RID: 13265
		void PrepareEditingCellForEdit(bool selectAll);
	}
}
