using System;

namespace System.Data
{
	// Token: 0x020000B4 RID: 180
	public class FillErrorEventArgs : EventArgs
	{
		// Token: 0x06000C27 RID: 3111 RVA: 0x001F9B20 File Offset: 0x001F8F20
		public FillErrorEventArgs(DataTable dataTable, object[] values)
		{
			this.dataTable = dataTable;
			this.values = values;
			if (this.values == null)
			{
				this.values = new object[0];
			}
		}

		// Token: 0x170001B4 RID: 436
		// (get) Token: 0x06000C28 RID: 3112 RVA: 0x001F9B58 File Offset: 0x001F8F58
		// (set) Token: 0x06000C29 RID: 3113 RVA: 0x001F9B6C File Offset: 0x001F8F6C
		public bool Continue
		{
			get
			{
				return this.continueFlag;
			}
			set
			{
				this.continueFlag = value;
			}
		}

		// Token: 0x170001B5 RID: 437
		// (get) Token: 0x06000C2A RID: 3114 RVA: 0x001F9B80 File Offset: 0x001F8F80
		public DataTable DataTable
		{
			get
			{
				return this.dataTable;
			}
		}

		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x06000C2B RID: 3115 RVA: 0x001F9B94 File Offset: 0x001F8F94
		// (set) Token: 0x06000C2C RID: 3116 RVA: 0x001F9BA8 File Offset: 0x001F8FA8
		public Exception Errors
		{
			get
			{
				return this.errors;
			}
			set
			{
				this.errors = value;
			}
		}

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x06000C2D RID: 3117 RVA: 0x001F9BBC File Offset: 0x001F8FBC
		public object[] Values
		{
			get
			{
				object[] array = new object[this.values.Length];
				for (int i = 0; i < this.values.Length; i++)
				{
					array[i] = this.values[i];
				}
				return array;
			}
		}

		// Token: 0x0400088A RID: 2186
		private bool continueFlag;

		// Token: 0x0400088B RID: 2187
		private DataTable dataTable;

		// Token: 0x0400088C RID: 2188
		private Exception errors;

		// Token: 0x0400088D RID: 2189
		private object[] values;
	}
}
