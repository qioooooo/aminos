using System;
using System.Collections;

namespace System.Web
{
	// Token: 0x0200007F RID: 127
	internal class HttpRawResponse
	{
		// Token: 0x06000546 RID: 1350 RVA: 0x000155A0 File Offset: 0x000145A0
		internal HttpRawResponse(int statusCode, string statusDescription, ArrayList headers, ArrayList buffers, bool hasSubstBlocks)
		{
			this._statusCode = statusCode;
			this._statusDescr = statusDescription;
			this._headers = headers;
			this._buffers = buffers;
			this._hasSubstBlocks = hasSubstBlocks;
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x06000547 RID: 1351 RVA: 0x000155CD File Offset: 0x000145CD
		internal int StatusCode
		{
			get
			{
				return this._statusCode;
			}
		}

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x06000548 RID: 1352 RVA: 0x000155D5 File Offset: 0x000145D5
		internal string StatusDescription
		{
			get
			{
				return this._statusDescr;
			}
		}

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x06000549 RID: 1353 RVA: 0x000155DD File Offset: 0x000145DD
		internal ArrayList Headers
		{
			get
			{
				return this._headers;
			}
		}

		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x0600054A RID: 1354 RVA: 0x000155E5 File Offset: 0x000145E5
		internal ArrayList Buffers
		{
			get
			{
				return this._buffers;
			}
		}

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x0600054B RID: 1355 RVA: 0x000155ED File Offset: 0x000145ED
		internal bool HasSubstBlocks
		{
			get
			{
				return this._hasSubstBlocks;
			}
		}

		// Token: 0x0400105E RID: 4190
		private int _statusCode;

		// Token: 0x0400105F RID: 4191
		private string _statusDescr;

		// Token: 0x04001060 RID: 4192
		private ArrayList _headers;

		// Token: 0x04001061 RID: 4193
		private ArrayList _buffers;

		// Token: 0x04001062 RID: 4194
		private bool _hasSubstBlocks;
	}
}
