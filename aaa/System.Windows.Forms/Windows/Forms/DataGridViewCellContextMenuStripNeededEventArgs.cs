using System;

namespace System.Windows.Forms
{
	// Token: 0x02000313 RID: 787
	public class DataGridViewCellContextMenuStripNeededEventArgs : DataGridViewCellEventArgs
	{
		// Token: 0x06003334 RID: 13108 RVA: 0x000B3CE0 File Offset: 0x000B2CE0
		public DataGridViewCellContextMenuStripNeededEventArgs(int columnIndex, int rowIndex)
			: base(columnIndex, rowIndex)
		{
		}

		// Token: 0x06003335 RID: 13109 RVA: 0x000B3CEA File Offset: 0x000B2CEA
		internal DataGridViewCellContextMenuStripNeededEventArgs(int columnIndex, int rowIndex, ContextMenuStrip contextMenuStrip)
			: base(columnIndex, rowIndex)
		{
			this.contextMenuStrip = contextMenuStrip;
		}

		// Token: 0x17000903 RID: 2307
		// (get) Token: 0x06003336 RID: 13110 RVA: 0x000B3CFB File Offset: 0x000B2CFB
		// (set) Token: 0x06003337 RID: 13111 RVA: 0x000B3D03 File Offset: 0x000B2D03
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

		// Token: 0x04001AA9 RID: 6825
		private ContextMenuStrip contextMenuStrip;
	}
}
