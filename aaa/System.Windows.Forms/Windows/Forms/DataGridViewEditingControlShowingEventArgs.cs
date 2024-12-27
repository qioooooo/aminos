using System;

namespace System.Windows.Forms
{
	// Token: 0x02000348 RID: 840
	public class DataGridViewEditingControlShowingEventArgs : EventArgs
	{
		// Token: 0x06003588 RID: 13704 RVA: 0x000C0B42 File Offset: 0x000BFB42
		public DataGridViewEditingControlShowingEventArgs(Control control, DataGridViewCellStyle cellStyle)
		{
			this.control = control;
			this.cellStyle = cellStyle;
		}

		// Token: 0x170009D2 RID: 2514
		// (get) Token: 0x06003589 RID: 13705 RVA: 0x000C0B58 File Offset: 0x000BFB58
		// (set) Token: 0x0600358A RID: 13706 RVA: 0x000C0B60 File Offset: 0x000BFB60
		public DataGridViewCellStyle CellStyle
		{
			get
			{
				return this.cellStyle;
			}
			set
			{
				if (value == null)
				{
					throw new ArgumentNullException("value");
				}
				this.cellStyle = value;
			}
		}

		// Token: 0x170009D3 RID: 2515
		// (get) Token: 0x0600358B RID: 13707 RVA: 0x000C0B77 File Offset: 0x000BFB77
		public Control Control
		{
			get
			{
				return this.control;
			}
		}

		// Token: 0x04001B95 RID: 7061
		private Control control;

		// Token: 0x04001B96 RID: 7062
		private DataGridViewCellStyle cellStyle;
	}
}
