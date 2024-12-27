using System;

namespace System.Windows.Forms
{
	// Token: 0x02000325 RID: 805
	[Flags]
	public enum DataGridViewCellStyleScopes
	{
		// Token: 0x04001AF0 RID: 6896
		None = 0,
		// Token: 0x04001AF1 RID: 6897
		Cell = 1,
		// Token: 0x04001AF2 RID: 6898
		Column = 2,
		// Token: 0x04001AF3 RID: 6899
		Row = 4,
		// Token: 0x04001AF4 RID: 6900
		DataGridView = 8,
		// Token: 0x04001AF5 RID: 6901
		ColumnHeaders = 16,
		// Token: 0x04001AF6 RID: 6902
		RowHeaders = 32,
		// Token: 0x04001AF7 RID: 6903
		Rows = 64,
		// Token: 0x04001AF8 RID: 6904
		AlternatingRows = 128
	}
}
