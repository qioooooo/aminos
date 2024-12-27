using System;
using System.Collections;
using System.Net;

namespace System.Runtime.Remoting.Channels
{
	// Token: 0x02000005 RID: 5
	[Serializable]
	internal class BaseTransportHeaders : ITransportHeaders
	{
		// Token: 0x06000001 RID: 1 RVA: 0x000020D0 File Offset: 0x000010D0
		public BaseTransportHeaders()
		{
			this._otherHeaders = null;
		}

		// Token: 0x17000001 RID: 1
		// (get) Token: 0x06000002 RID: 2 RVA: 0x000020DF File Offset: 0x000010DF
		// (set) Token: 0x06000003 RID: 3 RVA: 0x000020E7 File Offset: 0x000010E7
		public string RequestUri
		{
			get
			{
				return this._requestUri;
			}
			set
			{
				this._requestUri = value;
			}
		}

		// Token: 0x17000002 RID: 2
		// (get) Token: 0x06000004 RID: 4 RVA: 0x000020F0 File Offset: 0x000010F0
		// (set) Token: 0x06000005 RID: 5 RVA: 0x000020F8 File Offset: 0x000010F8
		public string ContentType
		{
			get
			{
				return this._contentType;
			}
			set
			{
				this._contentType = value;
			}
		}

		// Token: 0x17000003 RID: 3
		// (set) Token: 0x06000006 RID: 6 RVA: 0x00002101 File Offset: 0x00001101
		public object ConnectionId
		{
			set
			{
				this._connectionId = value;
			}
		}

		// Token: 0x17000004 RID: 4
		// (set) Token: 0x06000007 RID: 7 RVA: 0x0000210A File Offset: 0x0000110A
		public IPAddress IPAddress
		{
			set
			{
				this._ipAddress = value;
			}
		}

		// Token: 0x17000005 RID: 5
		public object this[object key]
		{
			get
			{
				string text = key as string;
				if (text != null)
				{
					int num = this.MapHeaderNameToIndex(text);
					if (num != -1)
					{
						return this.GetValueFromHeaderIndex(num);
					}
				}
				if (this._otherHeaders != null)
				{
					return this._otherHeaders[key];
				}
				return null;
			}
			set
			{
				bool flag = false;
				string text = key as string;
				if (text != null)
				{
					int num = this.MapHeaderNameToIndex(text);
					if (num != -1)
					{
						this.SetValueFromHeaderIndex(num, value);
						flag = true;
					}
				}
				if (!flag)
				{
					if (this._otherHeaders == null)
					{
						this._otherHeaders = new TransportHeaders();
					}
					this._otherHeaders[key] = value;
				}
			}
		}

		// Token: 0x0600000A RID: 10 RVA: 0x000021AA File Offset: 0x000011AA
		public IEnumerator GetEnumerator()
		{
			return new BaseTransportHeadersEnumerator(this);
		}

		// Token: 0x0600000B RID: 11 RVA: 0x000021B2 File Offset: 0x000011B2
		internal IEnumerator GetOtherHeadersEnumerator()
		{
			if (this._otherHeaders == null)
			{
				return null;
			}
			return this._otherHeaders.GetEnumerator();
		}

		// Token: 0x0600000C RID: 12 RVA: 0x000021CC File Offset: 0x000011CC
		internal int MapHeaderNameToIndex(string headerName)
		{
			if (string.Compare(headerName, "__ConnectionId", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return 0;
			}
			if (string.Compare(headerName, "__IPAddress", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return 1;
			}
			if (string.Compare(headerName, "__RequestUri", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return 2;
			}
			if (string.Compare(headerName, "Content-Type", StringComparison.OrdinalIgnoreCase) == 0)
			{
				return 3;
			}
			return -1;
		}

		// Token: 0x0600000D RID: 13 RVA: 0x0000221C File Offset: 0x0000121C
		internal string MapHeaderIndexToName(int index)
		{
			switch (index)
			{
			case 0:
				return "__ConnectionId";
			case 1:
				return "__IPAddress";
			case 2:
				return "__RequestUri";
			case 3:
				return "Content-Type";
			default:
				return null;
			}
		}

		// Token: 0x0600000E RID: 14 RVA: 0x0000225C File Offset: 0x0000125C
		internal object GetValueFromHeaderIndex(int index)
		{
			switch (index)
			{
			case 0:
				return this._connectionId;
			case 1:
				return this._ipAddress;
			case 2:
				return this._requestUri;
			case 3:
				return this._contentType;
			default:
				return null;
			}
		}

		// Token: 0x0600000F RID: 15 RVA: 0x000022A0 File Offset: 0x000012A0
		internal void SetValueFromHeaderIndex(int index, object value)
		{
			switch (index)
			{
			case 0:
				this._connectionId = value;
				return;
			case 1:
				this._ipAddress = value;
				return;
			case 2:
				this._requestUri = (string)value;
				return;
			case 3:
				this._contentType = (string)value;
				return;
			default:
				return;
			}
		}

		// Token: 0x04000030 RID: 48
		internal const int WellknownHeaderCount = 4;

		// Token: 0x04000031 RID: 49
		private object _connectionId;

		// Token: 0x04000032 RID: 50
		private object _ipAddress;

		// Token: 0x04000033 RID: 51
		private string _requestUri;

		// Token: 0x04000034 RID: 52
		private string _contentType;

		// Token: 0x04000035 RID: 53
		private ITransportHeaders _otherHeaders;
	}
}
