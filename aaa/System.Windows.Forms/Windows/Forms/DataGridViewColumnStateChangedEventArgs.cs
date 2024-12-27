using System;

namespace System.Windows.Forms
{
	// Token: 0x0200033C RID: 828
	public class DataGridViewColumnStateChangedEventArgs : EventArgs
	{
		// Token: 0x060034C6 RID: 13510 RVA: 0x000BC3CA File Offset: 0x000BB3CA
		public DataGridViewColumnStateChangedEventArgs(DataGridViewColumn dataGridViewColumn, DataGridViewElementStates stateChanged)
		{
			this.dataGridViewColumn = dataGridViewColumn;
			this.stateChanged = stateChanged;
		}

		// Token: 0x1700098D RID: 2445
		// (get) Token: 0x060034C7 RID: 13511 RVA: 0x000BC3E0 File Offset: 0x000BB3E0
		public DataGridViewColumn Column
		{
			get
			{
				return this.dataGridViewColumn;
			}
		}

		// Token: 0x1700098E RID: 2446
		// (get) Token: 0x060034C8 RID: 13512 RVA: 0x000BC3E8 File Offset: 0x000BB3E8
		public DataGridViewElementStates StateChanged
		{
			get
			{
				return this.stateChanged;
			}
		}

		// Token: 0x04001B46 RID: 6982
		private DataGridViewColumn dataGridViewColumn;

		// Token: 0x04001B47 RID: 6983
		private DataGridViewElementStates stateChanged;
	}
}
