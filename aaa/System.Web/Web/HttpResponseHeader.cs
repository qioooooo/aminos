using System;
using System.Text;

namespace System.Web
{
	// Token: 0x02000088 RID: 136
	internal class HttpResponseHeader
	{
		// Token: 0x06000673 RID: 1651 RVA: 0x0001BC22 File Offset: 0x0001AC22
		internal HttpResponseHeader(int knownHeaderIndex, string value)
		{
			this._unknownHeader = null;
			this._knownHeaderIndex = knownHeaderIndex;
			if (HttpRuntime.EnableHeaderChecking)
			{
				this._value = HttpResponseHeader.MaybeEncodeHeader(value);
				return;
			}
			this._value = value;
		}

		// Token: 0x06000674 RID: 1652 RVA: 0x0001BC54 File Offset: 0x0001AC54
		internal HttpResponseHeader(string unknownHeader, string value)
		{
			if (HttpRuntime.EnableHeaderChecking)
			{
				this._unknownHeader = HttpResponseHeader.MaybeEncodeHeader(unknownHeader);
				this._knownHeaderIndex = HttpWorkerRequest.GetKnownResponseHeaderIndex(this._unknownHeader);
				this._value = HttpResponseHeader.MaybeEncodeHeader(value);
				return;
			}
			this._unknownHeader = unknownHeader;
			this._knownHeaderIndex = HttpWorkerRequest.GetKnownResponseHeaderIndex(this._unknownHeader);
			this._value = value;
		}

		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06000675 RID: 1653 RVA: 0x0001BCB7 File Offset: 0x0001ACB7
		internal virtual string Name
		{
			get
			{
				if (this._unknownHeader != null)
				{
					return this._unknownHeader;
				}
				return HttpWorkerRequest.GetKnownResponseHeaderName(this._knownHeaderIndex);
			}
		}

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06000676 RID: 1654 RVA: 0x0001BCD3 File Offset: 0x0001ACD3
		internal string Value
		{
			get
			{
				return this._value;
			}
		}

		// Token: 0x06000677 RID: 1655 RVA: 0x0001BCDB File Offset: 0x0001ACDB
		internal void Send(HttpWorkerRequest wr)
		{
			if (this._knownHeaderIndex >= 0)
			{
				wr.SendKnownResponseHeader(this._knownHeaderIndex, this._value);
				return;
			}
			wr.SendUnknownResponseHeader(this._unknownHeader, this._value);
		}

		// Token: 0x06000678 RID: 1656 RVA: 0x0001BD0C File Offset: 0x0001AD0C
		internal static string MaybeEncodeHeader(string value)
		{
			string text = value;
			if (HttpResponseHeader.NeedsEncoding(value))
			{
				StringBuilder stringBuilder = new StringBuilder();
				foreach (char c in value)
				{
					if (c < ' ' && c != '\t')
					{
						stringBuilder.Append(HttpResponseHeader.EncodingTable[(int)c]);
					}
					else if (c == '\u007f')
					{
						stringBuilder.Append("%7f");
					}
					else
					{
						stringBuilder.Append(c);
					}
				}
				text = stringBuilder.ToString();
			}
			return text;
		}

		// Token: 0x06000679 RID: 1657 RVA: 0x0001BD88 File Offset: 0x0001AD88
		internal static bool NeedsEncoding(string value)
		{
			foreach (char c in value)
			{
				if ((c < ' ' && c != '\t') || c == '\u007f')
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x040010EE RID: 4334
		private string _unknownHeader;

		// Token: 0x040010EF RID: 4335
		private int _knownHeaderIndex;

		// Token: 0x040010F0 RID: 4336
		private string _value;

		// Token: 0x040010F1 RID: 4337
		private static readonly string[] EncodingTable = new string[]
		{
			"%00", "%01", "%02", "%03", "%04", "%05", "%06", "%07", "%08", "%09",
			"%0a", "%0b", "%0c", "%0d", "%0e", "%0f", "%10", "%11", "%12", "%13",
			"%14", "%15", "%16", "%17", "%18", "%19", "%1a", "%1b", "%1c", "%1d",
			"%1e", "%1f"
		};
	}
}
