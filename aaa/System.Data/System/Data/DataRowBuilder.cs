using System;

namespace System.Data
{
	// Token: 0x02000082 RID: 130
	public sealed class DataRowBuilder
	{
		// Token: 0x060007D4 RID: 2004 RVA: 0x001E13BC File Offset: 0x001E07BC
		internal DataRowBuilder(DataTable table, int record)
		{
			this._table = table;
			this._record = record;
		}

		// Token: 0x04000745 RID: 1861
		internal readonly DataTable _table;

		// Token: 0x04000746 RID: 1862
		internal int _record;
	}
}
