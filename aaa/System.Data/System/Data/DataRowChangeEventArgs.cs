using System;

namespace System.Data
{
	// Token: 0x02000084 RID: 132
	public class DataRowChangeEventArgs : EventArgs
	{
		// Token: 0x060007D5 RID: 2005 RVA: 0x001E13E0 File Offset: 0x001E07E0
		public DataRowChangeEventArgs(DataRow row, DataRowAction action)
		{
			this.row = row;
			this.action = action;
		}

		// Token: 0x170000F4 RID: 244
		// (get) Token: 0x060007D6 RID: 2006 RVA: 0x001E1404 File Offset: 0x001E0804
		public DataRow Row
		{
			get
			{
				return this.row;
			}
		}

		// Token: 0x170000F5 RID: 245
		// (get) Token: 0x060007D7 RID: 2007 RVA: 0x001E1418 File Offset: 0x001E0818
		public DataRowAction Action
		{
			get
			{
				return this.action;
			}
		}

		// Token: 0x04000750 RID: 1872
		private DataRow row;

		// Token: 0x04000751 RID: 1873
		private DataRowAction action;
	}
}
