using System;

namespace System.Data
{
	// Token: 0x0200009B RID: 155
	public sealed class DataTableClearEventArgs : EventArgs
	{
		// Token: 0x06000A65 RID: 2661 RVA: 0x001F3438 File Offset: 0x001F2838
		public DataTableClearEventArgs(DataTable dataTable)
		{
			this.dataTable = dataTable;
		}

		// Token: 0x1700014D RID: 333
		// (get) Token: 0x06000A66 RID: 2662 RVA: 0x001F3454 File Offset: 0x001F2854
		public DataTable Table
		{
			get
			{
				return this.dataTable;
			}
		}

		// Token: 0x1700014E RID: 334
		// (get) Token: 0x06000A67 RID: 2663 RVA: 0x001F3468 File Offset: 0x001F2868
		public string TableName
		{
			get
			{
				return this.dataTable.TableName;
			}
		}

		// Token: 0x1700014F RID: 335
		// (get) Token: 0x06000A68 RID: 2664 RVA: 0x001F3480 File Offset: 0x001F2880
		public string TableNamespace
		{
			get
			{
				return this.dataTable.Namespace;
			}
		}

		// Token: 0x0400080D RID: 2061
		private readonly DataTable dataTable;
	}
}
