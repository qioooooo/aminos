using System;

namespace System.Windows.Forms
{
	// Token: 0x0200031B RID: 795
	public class DataGridViewCellMouseEventArgs : MouseEventArgs
	{
		// Token: 0x0600335E RID: 13150 RVA: 0x000B4180 File Offset: 0x000B3180
		public DataGridViewCellMouseEventArgs(int columnIndex, int rowIndex, int localX, int localY, MouseEventArgs e)
			: base(e.Button, e.Clicks, localX, localY, e.Delta)
		{
			if (columnIndex < -1)
			{
				throw new ArgumentOutOfRangeException("columnIndex");
			}
			if (rowIndex < -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			this.columnIndex = columnIndex;
			this.rowIndex = rowIndex;
		}

		// Token: 0x17000915 RID: 2325
		// (get) Token: 0x0600335F RID: 13151 RVA: 0x000B41D7 File Offset: 0x000B31D7
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		// Token: 0x17000916 RID: 2326
		// (get) Token: 0x06003360 RID: 13152 RVA: 0x000B41DF File Offset: 0x000B31DF
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x04001ABD RID: 6845
		private int rowIndex;

		// Token: 0x04001ABE RID: 6846
		private int columnIndex;
	}
}
