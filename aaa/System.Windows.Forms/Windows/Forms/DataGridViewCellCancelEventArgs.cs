using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000310 RID: 784
	public class DataGridViewCellCancelEventArgs : CancelEventArgs
	{
		// Token: 0x06003307 RID: 13063 RVA: 0x000B34D3 File Offset: 0x000B24D3
		internal DataGridViewCellCancelEventArgs(DataGridViewCell dataGridViewCell)
			: this(dataGridViewCell.ColumnIndex, dataGridViewCell.RowIndex)
		{
		}

		// Token: 0x06003308 RID: 13064 RVA: 0x000B34E7 File Offset: 0x000B24E7
		public DataGridViewCellCancelEventArgs(int columnIndex, int rowIndex)
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

		// Token: 0x170008F6 RID: 2294
		// (get) Token: 0x06003309 RID: 13065 RVA: 0x000B351B File Offset: 0x000B251B
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		// Token: 0x170008F7 RID: 2295
		// (get) Token: 0x0600330A RID: 13066 RVA: 0x000B3523 File Offset: 0x000B2523
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x04001AA2 RID: 6818
		private int columnIndex;

		// Token: 0x04001AA3 RID: 6819
		private int rowIndex;
	}
}
