using System;

namespace System.Windows.Forms
{
	// Token: 0x02000328 RID: 808
	public class DataGridViewCellValueEventArgs : EventArgs
	{
		// Token: 0x060033C5 RID: 13253 RVA: 0x000B585C File Offset: 0x000B485C
		internal DataGridViewCellValueEventArgs()
		{
			this.columnIndex = (this.rowIndex = -1);
		}

		// Token: 0x060033C6 RID: 13254 RVA: 0x000B587F File Offset: 0x000B487F
		public DataGridViewCellValueEventArgs(int columnIndex, int rowIndex)
		{
			if (columnIndex < 0)
			{
				throw new ArgumentOutOfRangeException("columnIndex");
			}
			if (rowIndex < 0)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			this.rowIndex = rowIndex;
			this.columnIndex = columnIndex;
		}

		// Token: 0x17000946 RID: 2374
		// (get) Token: 0x060033C7 RID: 13255 RVA: 0x000B58B3 File Offset: 0x000B48B3
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		// Token: 0x17000947 RID: 2375
		// (get) Token: 0x060033C8 RID: 13256 RVA: 0x000B58BB File Offset: 0x000B48BB
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x17000948 RID: 2376
		// (get) Token: 0x060033C9 RID: 13257 RVA: 0x000B58C3 File Offset: 0x000B48C3
		// (set) Token: 0x060033CA RID: 13258 RVA: 0x000B58CB File Offset: 0x000B48CB
		public object Value
		{
			get
			{
				return this.val;
			}
			set
			{
				this.val = value;
			}
		}

		// Token: 0x060033CB RID: 13259 RVA: 0x000B58D4 File Offset: 0x000B48D4
		internal void SetProperties(int columnIndex, int rowIndex, object value)
		{
			this.columnIndex = columnIndex;
			this.rowIndex = rowIndex;
			this.val = value;
		}

		// Token: 0x04001AFD RID: 6909
		private int rowIndex;

		// Token: 0x04001AFE RID: 6910
		private int columnIndex;

		// Token: 0x04001AFF RID: 6911
		private object val;
	}
}
