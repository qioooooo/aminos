using System;

namespace System.Windows.Forms
{
	// Token: 0x02000334 RID: 820
	public class DataGridViewColumnDividerDoubleClickEventArgs : HandledMouseEventArgs
	{
		// Token: 0x06003479 RID: 13433 RVA: 0x000B98E8 File Offset: 0x000B88E8
		public DataGridViewColumnDividerDoubleClickEventArgs(int columnIndex, HandledMouseEventArgs e)
			: base(e.Button, e.Clicks, e.X, e.Y, e.Delta, e.Handled)
		{
			if (columnIndex < -1)
			{
				throw new ArgumentOutOfRangeException("columnIndex");
			}
			this.columnIndex = columnIndex;
		}

		// Token: 0x17000972 RID: 2418
		// (get) Token: 0x0600347A RID: 13434 RVA: 0x000B9935 File Offset: 0x000B8935
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		// Token: 0x04001B2B RID: 6955
		private int columnIndex;
	}
}
