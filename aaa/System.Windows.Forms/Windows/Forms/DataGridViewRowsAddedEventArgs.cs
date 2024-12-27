using System;

namespace System.Windows.Forms
{
	// Token: 0x02000392 RID: 914
	public class DataGridViewRowsAddedEventArgs : EventArgs
	{
		// Token: 0x060037EE RID: 14318 RVA: 0x000CC895 File Offset: 0x000CB895
		public DataGridViewRowsAddedEventArgs(int rowIndex, int rowCount)
		{
			this.rowIndex = rowIndex;
			this.rowCount = rowCount;
		}

		// Token: 0x17000A6A RID: 2666
		// (get) Token: 0x060037EF RID: 14319 RVA: 0x000CC8AB File Offset: 0x000CB8AB
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x17000A6B RID: 2667
		// (get) Token: 0x060037F0 RID: 14320 RVA: 0x000CC8B3 File Offset: 0x000CB8B3
		public int RowCount
		{
			get
			{
				return this.rowCount;
			}
		}

		// Token: 0x04001C4C RID: 7244
		private int rowIndex;

		// Token: 0x04001C4D RID: 7245
		private int rowCount;
	}
}
