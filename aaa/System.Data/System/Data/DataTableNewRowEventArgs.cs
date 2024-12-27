using System;

namespace System.Data
{
	// Token: 0x0200009F RID: 159
	public sealed class DataTableNewRowEventArgs : EventArgs
	{
		// Token: 0x06000AA0 RID: 2720 RVA: 0x001F4448 File Offset: 0x001F3848
		public DataTableNewRowEventArgs(DataRow dataRow)
		{
			this.dataRow = dataRow;
		}

		// Token: 0x17000155 RID: 341
		// (get) Token: 0x06000AA1 RID: 2721 RVA: 0x001F4464 File Offset: 0x001F3864
		public DataRow Row
		{
			get
			{
				return this.dataRow;
			}
		}

		// Token: 0x04000816 RID: 2070
		private readonly DataRow dataRow;
	}
}
