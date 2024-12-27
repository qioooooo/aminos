using System;

namespace System.Windows.Forms
{
	// Token: 0x02000386 RID: 902
	public class DataGridViewRowContextMenuStripNeededEventArgs : EventArgs
	{
		// Token: 0x06003780 RID: 14208 RVA: 0x000CA3CE File Offset: 0x000C93CE
		public DataGridViewRowContextMenuStripNeededEventArgs(int rowIndex)
		{
			if (rowIndex < -1)
			{
				throw new ArgumentOutOfRangeException("rowIndex");
			}
			this.rowIndex = rowIndex;
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x000CA3EC File Offset: 0x000C93EC
		internal DataGridViewRowContextMenuStripNeededEventArgs(int rowIndex, ContextMenuStrip contextMenuStrip)
			: this(rowIndex)
		{
			this.contextMenuStrip = contextMenuStrip;
		}

		// Token: 0x17000A3B RID: 2619
		// (get) Token: 0x06003782 RID: 14210 RVA: 0x000CA3FC File Offset: 0x000C93FC
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x17000A3C RID: 2620
		// (get) Token: 0x06003783 RID: 14211 RVA: 0x000CA404 File Offset: 0x000C9404
		// (set) Token: 0x06003784 RID: 14212 RVA: 0x000CA40C File Offset: 0x000C940C
		public ContextMenuStrip ContextMenuStrip
		{
			get
			{
				return this.contextMenuStrip;
			}
			set
			{
				this.contextMenuStrip = value;
			}
		}

		// Token: 0x04001C17 RID: 7191
		private int rowIndex;

		// Token: 0x04001C18 RID: 7192
		private ContextMenuStrip contextMenuStrip;
	}
}
