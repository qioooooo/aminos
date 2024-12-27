using System;

namespace System.Windows.Forms
{
	// Token: 0x020002C1 RID: 705
	public interface IDataGridEditingService
	{
		// Token: 0x06002701 RID: 9985
		bool BeginEdit(DataGridColumnStyle gridColumn, int rowNumber);

		// Token: 0x06002702 RID: 9986
		bool EndEdit(DataGridColumnStyle gridColumn, int rowNumber, bool shouldAbort);
	}
}
