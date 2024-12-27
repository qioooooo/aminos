using System;

namespace System.Data.Odbc
{
	// Token: 0x020001EB RID: 491
	[Serializable]
	public sealed class OdbcError
	{
		// Token: 0x06001B81 RID: 7041 RVA: 0x00248230 File Offset: 0x00247630
		internal OdbcError(string source, string message, string state, int nativeerror)
		{
			this._source = source;
			this._message = message;
			this._state = state;
			this._nativeerror = nativeerror;
		}

		// Token: 0x170003A9 RID: 937
		// (get) Token: 0x06001B82 RID: 7042 RVA: 0x00248260 File Offset: 0x00247660
		public string Message
		{
			get
			{
				if (this._message == null)
				{
					return string.Empty;
				}
				return this._message;
			}
		}

		// Token: 0x170003AA RID: 938
		// (get) Token: 0x06001B83 RID: 7043 RVA: 0x00248284 File Offset: 0x00247684
		public string SQLState
		{
			get
			{
				return this._state;
			}
		}

		// Token: 0x170003AB RID: 939
		// (get) Token: 0x06001B84 RID: 7044 RVA: 0x00248298 File Offset: 0x00247698
		public int NativeError
		{
			get
			{
				return this._nativeerror;
			}
		}

		// Token: 0x170003AC RID: 940
		// (get) Token: 0x06001B85 RID: 7045 RVA: 0x002482AC File Offset: 0x002476AC
		public string Source
		{
			get
			{
				if (this._source == null)
				{
					return string.Empty;
				}
				return this._source;
			}
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x002482D0 File Offset: 0x002476D0
		internal void SetSource(string Source)
		{
			this._source = Source;
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x002482E4 File Offset: 0x002476E4
		public override string ToString()
		{
			return this.Message;
		}

		// Token: 0x04001007 RID: 4103
		internal string _message;

		// Token: 0x04001008 RID: 4104
		internal string _state;

		// Token: 0x04001009 RID: 4105
		internal int _nativeerror;

		// Token: 0x0400100A RID: 4106
		internal string _source;
	}
}
