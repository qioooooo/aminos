using System;
using System.ComponentModel;

namespace System.Windows.Forms
{
	// Token: 0x02000327 RID: 807
	public class DataGridViewCellValidatingEventArgs : CancelEventArgs
	{
		// Token: 0x060033C1 RID: 13249 RVA: 0x000B5827 File Offset: 0x000B4827
		internal DataGridViewCellValidatingEventArgs(int columnIndex, int rowIndex, object formattedValue)
		{
			this.rowIndex = rowIndex;
			this.columnIndex = columnIndex;
			this.formattedValue = formattedValue;
		}

		// Token: 0x17000943 RID: 2371
		// (get) Token: 0x060033C2 RID: 13250 RVA: 0x000B5844 File Offset: 0x000B4844
		public int ColumnIndex
		{
			get
			{
				return this.columnIndex;
			}
		}

		// Token: 0x17000944 RID: 2372
		// (get) Token: 0x060033C3 RID: 13251 RVA: 0x000B584C File Offset: 0x000B484C
		public object FormattedValue
		{
			get
			{
				return this.formattedValue;
			}
		}

		// Token: 0x17000945 RID: 2373
		// (get) Token: 0x060033C4 RID: 13252 RVA: 0x000B5854 File Offset: 0x000B4854
		public int RowIndex
		{
			get
			{
				return this.rowIndex;
			}
		}

		// Token: 0x04001AFA RID: 6906
		private int rowIndex;

		// Token: 0x04001AFB RID: 6907
		private int columnIndex;

		// Token: 0x04001AFC RID: 6908
		private object formattedValue;
	}
}
