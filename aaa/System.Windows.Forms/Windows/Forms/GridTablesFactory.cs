using System;

namespace System.Windows.Forms
{
	// Token: 0x020002E3 RID: 739
	public sealed class GridTablesFactory
	{
		// Token: 0x06002B90 RID: 11152 RVA: 0x00074FC2 File Offset: 0x00073FC2
		private GridTablesFactory()
		{
		}

		// Token: 0x06002B91 RID: 11153 RVA: 0x00074FCC File Offset: 0x00073FCC
		public static DataGridTableStyle[] CreateGridTables(DataGridTableStyle gridTable, object dataSource, string dataMember, BindingContext bindingManager)
		{
			return new DataGridTableStyle[] { gridTable };
		}
	}
}
