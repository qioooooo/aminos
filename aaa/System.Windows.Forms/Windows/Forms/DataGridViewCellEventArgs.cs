using System;

namespace System.Windows.Forms
{
	// Token: 0x02000312 RID: 786
	public class DataGridViewCellEventArgs : EventArgs
	{
		// Token: 0x06003330 RID: 13104 RVA: 0x000B3C88 File Offset: 0x000B2C88
		internal DataGridViewCellEventArgs(DataGridViewCell dataGridViewCell)
			: this(dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex)
		{
		}

		// Token: 0x06003331 RID: 13105 RVA: 0x000B3C9C File Offset: 0x000B2C9C
		public DataGridViewCellEventArgs(int columnIndex, int rowIndex)
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

		// Token: 0x17000901 RID: 2305
		// (get) Token: 0x06003332 RID: 13106 RVA: 0x000B3CD0 File Offset: 0x000B2CD0
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		// Token: 0x17000902 RID: 2306
		// (get) Token: 0x06003333 RID: 13107 RVA: 0x000B3CD8 File Offset: 0x000B2CD8
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x04001AA7 RID: 6823
		private int columnIndex;

		// Token: 0x04001AA8 RID: 6824
		private int rowIndex;
	}
}
