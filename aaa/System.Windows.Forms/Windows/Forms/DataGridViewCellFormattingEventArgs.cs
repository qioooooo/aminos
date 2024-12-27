using System;

namespace System.Windows.Forms
{
	// Token: 0x02000316 RID: 790
	public class DataGridViewCellFormattingEventArgs : ConvertEventArgs
	{
		// Token: 0x0600333E RID: 13118 RVA: 0x000B3DB4 File Offset: 0x000B2DB4
		public DataGridViewCellFormattingEventArgs(int columnIndex, int rowIndex, object value, Type desiredType, DataGridViewCellStyle cellStyle)
			: base(value, desiredType)
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
			this.cellStyle = cellStyle;
		}

		// Token: 0x17000905 RID: 2309
		// (get) Token: 0x0600333F RID: 13119 RVA: 0x000B3DF3 File Offset: 0x000B2DF3
		// (set) Token: 0x06003340 RID: 13120 RVA: 0x000B3DFB File Offset: 0x000B2DFB
		public DataGridViewCellStyle CellStyle
		{
			get
			{
				return this.cellStyle;
			}
			set
			{
				this.cellStyle = value;
			}
		}

		// Token: 0x17000906 RID: 2310
		// (get) Token: 0x06003341 RID: 13121 RVA: 0x000B3E04 File Offset: 0x000B2E04
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		// Token: 0x17000907 RID: 2311
		// (get) Token: 0x06003342 RID: 13122 RVA: 0x000B3E0C File Offset: 0x000B2E0C
		// (set) Token: 0x06003343 RID: 13123 RVA: 0x000B3E14 File Offset: 0x000B2E14
		public bool FormattingApplied
		{
			get
			{
				return this.formattingApplied;
			}
			set
			{
				this.formattingApplied = value;
			}
		}

		// Token: 0x17000908 RID: 2312
		// (get) Token: 0x06003344 RID: 13124 RVA: 0x000B3E1D File Offset: 0x000B2E1D
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x04001AAB RID: 6827
		private int columnIndex;

		// Token: 0x04001AAC RID: 6828
		private int rowIndex;

		// Token: 0x04001AAD RID: 6829
		private DataGridViewCellStyle cellStyle;

		// Token: 0x04001AAE RID: 6830
		private bool formattingApplied;
	}
}
