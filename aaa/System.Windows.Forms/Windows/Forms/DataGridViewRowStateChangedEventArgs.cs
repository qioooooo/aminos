using System;

namespace System.Windows.Forms
{
	// Token: 0x02000394 RID: 916
	public class DataGridViewRowStateChangedEventArgs : EventArgs
	{
		// Token: 0x060037F4 RID: 14324 RVA: 0x000CC97F File Offset: 0x000CB97F
		public DataGridViewRowStateChangedEventArgs(DataGridViewRow dataGridViewRow, DataGridViewElementStates stateChanged)
		{
			this.dataGridViewRow = dataGridViewRow;
			this.stateChanged = stateChanged;
		}

		// Token: 0x17000A6E RID: 2670
		// (get) Token: 0x060037F5 RID: 14325 RVA: 0x000CC995 File Offset: 0x000CB995
		public DataGridViewRow Row
		{
			get
			{
				return this.dataGridViewRow;
			}
		}

		// Token: 0x17000A6F RID: 2671
		// (get) Token: 0x060037F6 RID: 14326 RVA: 0x000CC99D File Offset: 0x000CB99D
		public DataGridViewElementStates StateChanged
		{
			get
			{
				return this.stateChanged;
			}
		}

		// Token: 0x04001C50 RID: 7248
		private DataGridViewRow dataGridViewRow;

		// Token: 0x04001C51 RID: 7249
		private DataGridViewElementStates stateChanged;
	}
}
