using System;

namespace System.Data.OracleClient
{
	// Token: 0x02000067 RID: 103
	public sealed class OracleInfoMessageEventArgs : EventArgs
	{
		// Token: 0x060004B3 RID: 1203 RVA: 0x00067134 File Offset: 0x00066534
		internal OracleInfoMessageEventArgs(OracleException exception)
		{
			this.exception = exception;
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060004B4 RID: 1204 RVA: 0x00067150 File Offset: 0x00066550
		public int Code
		{
			get
			{
				return this.exception.Code;
			}
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060004B5 RID: 1205 RVA: 0x00067168 File Offset: 0x00066568
		public string Message
		{
			get
			{
				return this.exception.Message;
			}
		}

		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060004B6 RID: 1206 RVA: 0x00067180 File Offset: 0x00066580
		public string Source
		{
			get
			{
				return this.exception.Source;
			}
		}

		// Token: 0x060004B7 RID: 1207 RVA: 0x00067198 File Offset: 0x00066598
		public override string ToString()
		{
			return this.Message;
		}

		// Token: 0x0400042D RID: 1069
		private OracleException exception;
	}
}
