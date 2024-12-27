using System;
using System.Text;

namespace System.Data.Odbc
{
	// Token: 0x020001F1 RID: 497
	public sealed class OdbcInfoMessageEventArgs : EventArgs
	{
		// Token: 0x06001BA9 RID: 7081 RVA: 0x002487A8 File Offset: 0x00247BA8
		internal OdbcInfoMessageEventArgs(OdbcErrorCollection errors)
		{
			this._errors = errors;
		}

		// Token: 0x170003B3 RID: 947
		// (get) Token: 0x06001BAA RID: 7082 RVA: 0x002487C4 File Offset: 0x00247BC4
		public OdbcErrorCollection Errors
		{
			get
			{
				return this._errors;
			}
		}

		// Token: 0x170003B4 RID: 948
		// (get) Token: 0x06001BAB RID: 7083 RVA: 0x002487D8 File Offset: 0x00247BD8
		public string Message
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (object obj in this.Errors)
				{
					OdbcError odbcError = (OdbcError)obj;
					if (0 < stringBuilder.Length)
					{
						stringBuilder.Append(Environment.NewLine);
					}
					stringBuilder.Append(odbcError.Message);
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x06001BAC RID: 7084 RVA: 0x00248864 File Offset: 0x00247C64
		public override string ToString()
		{
			return this.Message;
		}

		// Token: 0x0400100F RID: 4111
		private OdbcErrorCollection _errors;
	}
}
