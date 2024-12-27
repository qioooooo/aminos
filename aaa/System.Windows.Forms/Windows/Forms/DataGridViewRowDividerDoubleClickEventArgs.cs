using System;

namespace System.Windows.Forms
{
	// Token: 0x02000388 RID: 904
	public class DataGridViewRowDividerDoubleClickEventArgs : HandledMouseEventArgs
	{
		// Token: 0x06003788 RID: 14216 RVA: 0x000CA49C File Offset: 0x000C949C
		public DataGridViewRowDividerDoubleClickEventArgs(int rowIndex, HandledMouseEventArgs e)
			: base(e.Button, e.Clicks, e.X, e.Y, e.Delta, e.Handled)
		{
			if (rowIndex < -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			this.rowIndex = rowIndex;
		}

		// Token: 0x17000A3D RID: 2621
		// (get) Token: 0x06003789 RID: 14217 RVA: 0x000CA4E9 File Offset: 0x000C94E9
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x04001C19 RID: 7193
		private int rowIndex;
	}
}
