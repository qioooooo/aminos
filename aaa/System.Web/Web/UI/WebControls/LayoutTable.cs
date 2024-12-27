using System;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005C5 RID: 1477
	[SupportsEventValidation]
	internal sealed class LayoutTable : Table
	{
		// Token: 0x0600481A RID: 18458 RVA: 0x00126A44 File Offset: 0x00125A44
		public LayoutTable(int rows, int columns, Page page)
		{
			if (rows <= 0)
			{
				throw new ArgumentOutOfRangeException("rows");
			}
			if (columns <= 0)
			{
				throw new ArgumentOutOfRangeException("columns");
			}
			if (page != null)
			{
				this.Page = page;
			}
			for (int i = 0; i < rows; i++)
			{
				TableRow tableRow = new TableRow();
				this.Rows.Add(tableRow);
				for (int j = 0; j < columns; j++)
				{
					TableCell tableCell = new LayoutTableCell();
					tableRow.Cells.Add(tableCell);
				}
			}
		}

		// Token: 0x170011C6 RID: 4550
		public TableCell this[int row, int column]
		{
			get
			{
				return this.Rows[row].Cells[column];
			}
		}
	}
}
