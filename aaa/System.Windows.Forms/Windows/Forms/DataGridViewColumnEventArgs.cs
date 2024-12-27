using System;

namespace System.Windows.Forms
{
	// Token: 0x02000335 RID: 821
	public class DataGridViewColumnEventArgs : EventArgs
	{
		// Token: 0x0600347B RID: 13435 RVA: 0x000B993D File Offset: 0x000B893D
		public DataGridViewColumnEventArgs(DataGridViewColumn dataGridViewColumn)
		{
			if (dataGridViewColumn == null)
			{
				throw new ArgumentNullException("dataGridViewColumn");
			}
			this.dataGridViewColumn = dataGridViewColumn;
		}

		// Token: 0x17000973 RID: 2419
		// (get) Token: 0x0600347C RID: 13436 RVA: 0x000B995A File Offset: 0x000B895A
		public DataGridViewColumn Column
		{
			get
			{
				return this.dataGridViewColumn;
			}
		}

		// Token: 0x04001B2C RID: 6956
		private DataGridViewColumn dataGridViewColumn;
	}
}
