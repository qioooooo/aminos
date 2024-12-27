using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000380 RID: 896
	public class DataGridViewRowCancelEventArgs : CancelEventArgs
	{
		// Token: 0x06003716 RID: 14102 RVA: 0x000C6DC3 File Offset: 0x000C5DC3
		public DataGridViewRowCancelEventArgs(DataGridViewRow dataGridViewRow)
		{
			this.dataGridViewRow = dataGridViewRow;
		}

		// Token: 0x17000A2D RID: 2605
		// (get) Token: 0x06003717 RID: 14103 RVA: 0x000C6DD2 File Offset: 0x000C5DD2
		public DataGridViewRow Row
		{
			get
			{
				return this.dataGridViewRow;
			}
		}

		// Token: 0x04001C02 RID: 7170
		private DataGridViewRow dataGridViewRow;
	}
}
