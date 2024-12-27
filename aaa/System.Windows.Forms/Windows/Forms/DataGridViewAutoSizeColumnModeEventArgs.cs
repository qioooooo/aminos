using System;

namespace System.Windows.Forms
{
	// Token: 0x020002F9 RID: 761
	public class DataGridViewAutoSizeColumnModeEventArgs : EventArgs
	{
		// Token: 0x06003140 RID: 12608 RVA: 0x000A9828 File Offset: 0x000A8828
		public DataGridViewAutoSizeColumnModeEventArgs(DataGridViewColumn dataGridViewColumn, DataGridViewAutoSizeColumnMode previousMode)
		{
			this.dataGridViewColumn = dataGridViewColumn;
			this.previousMode = previousMode;
		}

		// Token: 0x17000865 RID: 2149
		// (get) Token: 0x06003141 RID: 12609 RVA: 0x000A983E File Offset: 0x000A883E
		public DataGridViewColumn Column
		{
			get
			{
				return this.dataGridViewColumn;
			}
		}

		// Token: 0x17000866 RID: 2150
		// (get) Token: 0x06003142 RID: 12610 RVA: 0x000A9846 File Offset: 0x000A8846
		public DataGridViewAutoSizeColumnMode PreviousMode
		{
			get
			{
				return this.previousMode;
			}
		}

		// Token: 0x04001A06 RID: 6662
		private DataGridViewAutoSizeColumnMode previousMode;

		// Token: 0x04001A07 RID: 6663
		private DataGridViewColumn dataGridViewColumn;
	}
}
