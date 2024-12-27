using System;

namespace System.Windows.Forms
{
	// Token: 0x0200031E RID: 798
	public class DataGridViewCellStateChangedEventArgs : EventArgs
	{
		// Token: 0x0600337A RID: 13178 RVA: 0x000B4638 File Offset: 0x000B3638
		public DataGridViewCellStateChangedEventArgs(DataGridViewCell dataGridViewCell, DataGridViewElementStates stateChanged)
		{
			if (dataGridViewCell == null)
			{
				throw new ArgumentNullException("dataGridViewCell");
			}
			this.dataGridViewCell = dataGridViewCell;
			this.stateChanged = stateChanged;
		}

		// Token: 0x17000927 RID: 2343
		// (get) Token: 0x0600337B RID: 13179 RVA: 0x000B465C File Offset: 0x000B365C
		public DataGridViewCell Cell
		{
			get
			{
				return this.dataGridViewCell;
			}
		}

		// Token: 0x17000928 RID: 2344
		// (get) Token: 0x0600337C RID: 13180 RVA: 0x000B4664 File Offset: 0x000B3664
		public DataGridViewElementStates StateChanged
		{
			get
			{
				return this.stateChanged;
			}
		}

		// Token: 0x04001AD0 RID: 6864
		private DataGridViewCell dataGridViewCell;

		// Token: 0x04001AD1 RID: 6865
		private DataGridViewElementStates stateChanged;
	}
}
