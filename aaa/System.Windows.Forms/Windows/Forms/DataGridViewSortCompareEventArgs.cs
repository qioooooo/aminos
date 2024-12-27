using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000399 RID: 921
	public class DataGridViewSortCompareEventArgs : HandledEventArgs
	{
		// Token: 0x06003840 RID: 14400 RVA: 0x000CCDD5 File Offset: 0x000CBDD5
		public DataGridViewSortCompareEventArgs(DataGridViewColumn dataGridViewColumn, object cellValue1, object cellValue2, int rowIndex1, int rowIndex2)
		{
			this.dataGridViewColumn = dataGridViewColumn;
			this.cellValue1 = cellValue1;
			this.cellValue2 = cellValue2;
			this.rowIndex1 = rowIndex1;
			this.rowIndex2 = rowIndex2;
		}

		// Token: 0x17000A88 RID: 2696
		// (get) Token: 0x06003841 RID: 14401 RVA: 0x000CCE02 File Offset: 0x000CBE02
		public object CellValue1
		{
			get
			{
				return this.cellValue1;
			}
		}

		// Token: 0x17000A89 RID: 2697
		// (get) Token: 0x06003842 RID: 14402 RVA: 0x000CCE0A File Offset: 0x000CBE0A
		public object CellValue2
		{
			get
			{
				return this.cellValue2;
			}
		}

		// Token: 0x17000A8A RID: 2698
		// (get) Token: 0x06003843 RID: 14403 RVA: 0x000CCE12 File Offset: 0x000CBE12
		public DataGridViewColumn Column
		{
			get
			{
				return this.dataGridViewColumn;
			}
		}

		// Token: 0x17000A8B RID: 2699
		// (get) Token: 0x06003844 RID: 14404 RVA: 0x000CCE1A File Offset: 0x000CBE1A
		public int RowIndex1
		{
			get
			{
				return this.rowIndex1;
			}
		}

		// Token: 0x17000A8C RID: 2700
		// (get) Token: 0x06003845 RID: 14405 RVA: 0x000CCE22 File Offset: 0x000CBE22
		public int RowIndex2
		{
			get
			{
				return this.rowIndex2;
			}
		}

		// Token: 0x17000A8D RID: 2701
		// (get) Token: 0x06003846 RID: 14406 RVA: 0x000CCE2A File Offset: 0x000CBE2A
		// (set) Token: 0x06003847 RID: 14407 RVA: 0x000CCE32 File Offset: 0x000CBE32
		public int SortResult
		{
			get
			{
				return this.sortResult;
			}
			set
			{
				this.sortResult = value;
			}
		}

		// Token: 0x04001C5B RID: 7259
		private DataGridViewColumn dataGridViewColumn;

		// Token: 0x04001C5C RID: 7260
		private object cellValue1;

		// Token: 0x04001C5D RID: 7261
		private object cellValue2;

		// Token: 0x04001C5E RID: 7262
		private int sortResult;

		// Token: 0x04001C5F RID: 7263
		private int rowIndex1;

		// Token: 0x04001C60 RID: 7264
		private int rowIndex2;
	}
}
