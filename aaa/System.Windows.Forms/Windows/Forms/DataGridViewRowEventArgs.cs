using System;

namespace System.Windows.Forms
{
	// Token: 0x0200038A RID: 906
	public class DataGridViewRowEventArgs : EventArgs
	{
		// Token: 0x0600378E RID: 14222 RVA: 0x000CA520 File Offset: 0x000C9520
		public DataGridViewRowEventArgs(DataGridViewRow dataGridViewRow)
		{
			if (dataGridViewRow == null)
			{
				throw new ArgumentNullException("dataGridViewRow");
			}
			this.dataGridViewRow = dataGridViewRow;
		}

		// Token: 0x17000A40 RID: 2624
		// (get) Token: 0x0600378F RID: 14223 RVA: 0x000CA53D File Offset: 0x000C953D
		public DataGridViewRow Row
		{
			get
			{
				return this.dataGridViewRow;
			}
		}

		// Token: 0x04001C1C RID: 7196
		private DataGridViewRow dataGridViewRow;
	}
}
