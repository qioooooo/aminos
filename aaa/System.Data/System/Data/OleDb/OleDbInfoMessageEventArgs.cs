using System;

namespace System.Data.OleDb
{
	// Token: 0x0200022B RID: 555
	public sealed class OleDbInfoMessageEventArgs : EventArgs
	{
		// Token: 0x06001FB2 RID: 8114 RVA: 0x0025E9D0 File Offset: 0x0025DDD0
		internal OleDbInfoMessageEventArgs(OleDbException exception)
		{
			this.exception = exception;
		}

		// Token: 0x17000453 RID: 1107
		// (get) Token: 0x06001FB3 RID: 8115 RVA: 0x0025E9EC File Offset: 0x0025DDEC
		public int ErrorCode
		{
			get
			{
				return this.exception.ErrorCode;
			}
		}

		// Token: 0x17000454 RID: 1108
		// (get) Token: 0x06001FB4 RID: 8116 RVA: 0x0025EA04 File Offset: 0x0025DE04
		public OleDbErrorCollection Errors
		{
			get
			{
				return this.exception.Errors;
			}
		}

		// Token: 0x06001FB5 RID: 8117 RVA: 0x0025EA1C File Offset: 0x0025DE1C
		internal bool ShouldSerializeErrors()
		{
			return this.exception.ShouldSerializeErrors();
		}

		// Token: 0x17000455 RID: 1109
		// (get) Token: 0x06001FB6 RID: 8118 RVA: 0x0025EA34 File Offset: 0x0025DE34
		public string Message
		{
			get
			{
				return this.exception.Message;
			}
		}

		// Token: 0x17000456 RID: 1110
		// (get) Token: 0x06001FB7 RID: 8119 RVA: 0x0025EA4C File Offset: 0x0025DE4C
		public string Source
		{
			get
			{
				return this.exception.Source;
			}
		}

		// Token: 0x06001FB8 RID: 8120 RVA: 0x0025EA64 File Offset: 0x0025DE64
		public override string ToString()
		{
			return this.Message;
		}

		// Token: 0x04001401 RID: 5121
		private readonly OleDbException exception;
	}
}
