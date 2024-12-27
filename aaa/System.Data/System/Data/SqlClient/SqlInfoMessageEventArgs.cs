using System;

namespace System.Data.SqlClient
{
	// Token: 0x020002F5 RID: 757
	public sealed class SqlInfoMessageEventArgs : EventArgs
	{
		// Token: 0x0600274A RID: 10058 RVA: 0x00289EB4 File Offset: 0x002892B4
		internal SqlInfoMessageEventArgs(SqlException exception)
		{
			this.exception = exception;
		}

		// Token: 0x17000644 RID: 1604
		// (get) Token: 0x0600274B RID: 10059 RVA: 0x00289ED0 File Offset: 0x002892D0
		public SqlErrorCollection Errors
		{
			get
			{
				return this.exception.Errors;
			}
		}

		// Token: 0x0600274C RID: 10060 RVA: 0x00289EE8 File Offset: 0x002892E8
		private bool ShouldSerializeErrors()
		{
			return this.exception != null && 0 < this.exception.Errors.Count;
		}

		// Token: 0x17000645 RID: 1605
		// (get) Token: 0x0600274D RID: 10061 RVA: 0x00289F14 File Offset: 0x00289314
		public string Message
		{
			get
			{
				return this.exception.Message;
			}
		}

		// Token: 0x17000646 RID: 1606
		// (get) Token: 0x0600274E RID: 10062 RVA: 0x00289F2C File Offset: 0x0028932C
		public string Source
		{
			get
			{
				return this.exception.Source;
			}
		}

		// Token: 0x0600274F RID: 10063 RVA: 0x00289F44 File Offset: 0x00289344
		public override string ToString()
		{
			return this.Message;
		}

		// Token: 0x040018E4 RID: 6372
		private SqlException exception;
	}
}
