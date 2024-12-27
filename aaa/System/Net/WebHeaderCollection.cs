using System;
using System.Collections;
using System.Collections.Specialized;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Security.Permissions;
using System.Text;

namespace System.Net
{
	// Token: 0x020004A3 RID: 1187
	[ComVisible(true)]
	[Serializable]
	public class WebHeaderCollection : NameValueCollection, ISerializable
	{
		// Token: 0x17000776 RID: 1910
		// (get) Token: 0x0600241E RID: 9246 RVA: 0x0008D3DF File Offset: 0x0008C3DF
		internal string ContentLength
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[1]);
				}
				return this.m_CommonHeaders[1];
			}
		}

		// Token: 0x17000777 RID: 1911
		// (get) Token: 0x0600241F RID: 9247 RVA: 0x0008D3FF File Offset: 0x0008C3FF
		internal string CacheControl
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[2]);
				}
				return this.m_CommonHeaders[2];
			}
		}

		// Token: 0x17000778 RID: 1912
		// (get) Token: 0x06002420 RID: 9248 RVA: 0x0008D41F File Offset: 0x0008C41F
		internal string ContentType
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[3]);
				}
				return this.m_CommonHeaders[3];
			}
		}

		// Token: 0x17000779 RID: 1913
		// (get) Token: 0x06002421 RID: 9249 RVA: 0x0008D43F File Offset: 0x0008C43F
		internal string Date
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[4]);
				}
				return this.m_CommonHeaders[4];
			}
		}

		// Token: 0x1700077A RID: 1914
		// (get) Token: 0x06002422 RID: 9250 RVA: 0x0008D45F File Offset: 0x0008C45F
		internal string Expires
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[5]);
				}
				return this.m_CommonHeaders[5];
			}
		}

		// Token: 0x1700077B RID: 1915
		// (get) Token: 0x06002423 RID: 9251 RVA: 0x0008D47F File Offset: 0x0008C47F
		internal string ETag
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[6]);
				}
				return this.m_CommonHeaders[6];
			}
		}

		// Token: 0x1700077C RID: 1916
		// (get) Token: 0x06002424 RID: 9252 RVA: 0x0008D49F File Offset: 0x0008C49F
		internal string LastModified
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[7]);
				}
				return this.m_CommonHeaders[7];
			}
		}

		// Token: 0x1700077D RID: 1917
		// (get) Token: 0x06002425 RID: 9253 RVA: 0x0008D4BF File Offset: 0x0008C4BF
		internal string Location
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[8]);
				}
				return this.m_CommonHeaders[8];
			}
		}

		// Token: 0x1700077E RID: 1918
		// (get) Token: 0x06002426 RID: 9254 RVA: 0x0008D4DF File Offset: 0x0008C4DF
		internal string ProxyAuthenticate
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[9]);
				}
				return this.m_CommonHeaders[9];
			}
		}

		// Token: 0x1700077F RID: 1919
		// (get) Token: 0x06002427 RID: 9255 RVA: 0x0008D501 File Offset: 0x0008C501
		internal string SetCookie2
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[11]);
				}
				return this.m_CommonHeaders[11];
			}
		}

		// Token: 0x17000780 RID: 1920
		// (get) Token: 0x06002428 RID: 9256 RVA: 0x0008D523 File Offset: 0x0008C523
		internal string SetCookie
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[12]);
				}
				return this.m_CommonHeaders[12];
			}
		}

		// Token: 0x17000781 RID: 1921
		// (get) Token: 0x06002429 RID: 9257 RVA: 0x0008D545 File Offset: 0x0008C545
		internal string Server
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[13]);
				}
				return this.m_CommonHeaders[13];
			}
		}

		// Token: 0x17000782 RID: 1922
		// (get) Token: 0x0600242A RID: 9258 RVA: 0x0008D567 File Offset: 0x0008C567
		internal string Via
		{
			get
			{
				if (this.m_CommonHeaders == null)
				{
					return this.Get(WebHeaderCollection.s_CommonHeaderNames[14]);
				}
				return this.m_CommonHeaders[14];
			}
		}

		// Token: 0x0600242B RID: 9259 RVA: 0x0008D58C File Offset: 0x0008C58C
		private void NormalizeCommonHeaders()
		{
			if (this.m_CommonHeaders == null)
			{
				return;
			}
			for (int i = 0; i < this.m_CommonHeaders.Length; i++)
			{
				if (this.m_CommonHeaders[i] != null)
				{
					this.InnerCollection.Add(WebHeaderCollection.s_CommonHeaderNames[i], this.m_CommonHeaders[i]);
				}
			}
			this.m_CommonHeaders = null;
			this.m_NumCommonHeaders = 0;
		}

		// Token: 0x17000783 RID: 1923
		// (get) Token: 0x0600242C RID: 9260 RVA: 0x0008D5E7 File Offset: 0x0008C5E7
		private NameValueCollection InnerCollection
		{
			get
			{
				if (this.m_InnerCollection == null)
				{
					this.m_InnerCollection = new NameValueCollection(16, CaseInsensitiveAscii.StaticInstance);
				}
				return this.m_InnerCollection;
			}
		}

		// Token: 0x17000784 RID: 1924
		// (get) Token: 0x0600242D RID: 9261 RVA: 0x0008D609 File Offset: 0x0008C609
		private bool AllowHttpRequestHeader
		{
			get
			{
				if (this.m_Type == WebHeaderCollectionType.Unknown)
				{
					this.m_Type = WebHeaderCollectionType.WebRequest;
				}
				return this.m_Type == WebHeaderCollectionType.WebRequest || this.m_Type == WebHeaderCollectionType.HttpWebRequest || this.m_Type == WebHeaderCollectionType.HttpListenerRequest;
			}
		}

		// Token: 0x17000785 RID: 1925
		// (get) Token: 0x0600242E RID: 9262 RVA: 0x0008D637 File Offset: 0x0008C637
		internal bool AllowHttpResponseHeader
		{
			get
			{
				if (this.m_Type == WebHeaderCollectionType.Unknown)
				{
					this.m_Type = WebHeaderCollectionType.WebResponse;
				}
				return this.m_Type == WebHeaderCollectionType.WebResponse || this.m_Type == WebHeaderCollectionType.HttpWebResponse || this.m_Type == WebHeaderCollectionType.HttpListenerResponse;
			}
		}

		// Token: 0x17000786 RID: 1926
		public string this[HttpRequestHeader header]
		{
			get
			{
				if (!this.AllowHttpRequestHeader)
				{
					throw new InvalidOperationException(SR.GetString("net_headers_req"));
				}
				return base[UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_HEADER_ID.ToString((int)header)];
			}
			set
			{
				if (!this.AllowHttpRequestHeader)
				{
					throw new InvalidOperationException(SR.GetString("net_headers_req"));
				}
				base[UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_HEADER_ID.ToString((int)header)] = value;
			}
		}

		// Token: 0x17000787 RID: 1927
		public string this[HttpResponseHeader header]
		{
			get
			{
				if (!this.AllowHttpResponseHeader)
				{
					throw new InvalidOperationException(SR.GetString("net_headers_rsp"));
				}
				if (this.m_CommonHeaders != null)
				{
					if (header == HttpResponseHeader.ProxyAuthenticate)
					{
						return this.m_CommonHeaders[9];
					}
					if (header == HttpResponseHeader.WwwAuthenticate)
					{
						return this.m_CommonHeaders[15];
					}
				}
				return base[UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.ToString((int)header)];
			}
			set
			{
				if (!this.AllowHttpResponseHeader)
				{
					throw new InvalidOperationException(SR.GetString("net_headers_rsp"));
				}
				if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
				{
					throw new ArgumentOutOfRangeException(SR.GetString("net_headers_toolong", new object[] { ushort.MaxValue }));
				}
				base[UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.ToString((int)header)] = value;
			}
		}

		// Token: 0x06002433 RID: 9267 RVA: 0x0008D780 File Offset: 0x0008C780
		public void Add(HttpRequestHeader header, string value)
		{
			if (!this.AllowHttpRequestHeader)
			{
				throw new InvalidOperationException(SR.GetString("net_headers_req"));
			}
			this.Add(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_HEADER_ID.ToString((int)header), value);
		}

		// Token: 0x06002434 RID: 9268 RVA: 0x0008D7A8 File Offset: 0x0008C7A8
		public void Add(HttpResponseHeader header, string value)
		{
			if (!this.AllowHttpResponseHeader)
			{
				throw new InvalidOperationException(SR.GetString("net_headers_rsp"));
			}
			if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_headers_toolong", new object[] { ushort.MaxValue }));
			}
			this.Add(UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.ToString((int)header), value);
		}

		// Token: 0x06002435 RID: 9269 RVA: 0x0008D818 File Offset: 0x0008C818
		public void Set(HttpRequestHeader header, string value)
		{
			if (!this.AllowHttpRequestHeader)
			{
				throw new InvalidOperationException(SR.GetString("net_headers_req"));
			}
			this.Set(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_HEADER_ID.ToString((int)header), value);
		}

		// Token: 0x06002436 RID: 9270 RVA: 0x0008D840 File Offset: 0x0008C840
		public void Set(HttpResponseHeader header, string value)
		{
			if (!this.AllowHttpResponseHeader)
			{
				throw new InvalidOperationException(SR.GetString("net_headers_rsp"));
			}
			if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_headers_toolong", new object[] { ushort.MaxValue }));
			}
			this.Set(UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.ToString((int)header), value);
		}

		// Token: 0x06002437 RID: 9271 RVA: 0x0008D8B0 File Offset: 0x0008C8B0
		internal void SetInternal(HttpResponseHeader header, string value)
		{
			if (!this.AllowHttpResponseHeader)
			{
				throw new InvalidOperationException(SR.GetString("net_headers_rsp"));
			}
			if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_headers_toolong", new object[] { ushort.MaxValue }));
			}
			this.SetInternal(UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.ToString((int)header), value);
		}

		// Token: 0x06002438 RID: 9272 RVA: 0x0008D920 File Offset: 0x0008C920
		public void Remove(HttpRequestHeader header)
		{
			if (!this.AllowHttpRequestHeader)
			{
				throw new InvalidOperationException(SR.GetString("net_headers_req"));
			}
			this.Remove(UnsafeNclNativeMethods.HttpApi.HTTP_REQUEST_HEADER_ID.ToString((int)header));
		}

		// Token: 0x06002439 RID: 9273 RVA: 0x0008D946 File Offset: 0x0008C946
		public void Remove(HttpResponseHeader header)
		{
			if (!this.AllowHttpResponseHeader)
			{
				throw new InvalidOperationException(SR.GetString("net_headers_rsp"));
			}
			this.Remove(UnsafeNclNativeMethods.HttpApi.HTTP_RESPONSE_HEADER_ID.ToString((int)header));
		}

		// Token: 0x0600243A RID: 9274 RVA: 0x0008D96C File Offset: 0x0008C96C
		protected void AddWithoutValidate(string headerName, string headerValue)
		{
			headerName = WebHeaderCollection.CheckBadChars(headerName, false);
			headerValue = WebHeaderCollection.CheckBadChars(headerValue, true);
			if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && headerValue != null && headerValue.Length > 65535)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_headers_toolong", new object[] { ushort.MaxValue }));
			}
			this.NormalizeCommonHeaders();
			base.InvalidateCachedArrays();
			this.InnerCollection.Add(headerName, headerValue);
		}

		// Token: 0x0600243B RID: 9275 RVA: 0x0008D9E4 File Offset: 0x0008C9E4
		internal void SetAddVerified(string name, string value)
		{
			if (WebHeaderCollection.HInfo[name].AllowMultiValues)
			{
				this.NormalizeCommonHeaders();
				base.InvalidateCachedArrays();
				this.InnerCollection.Add(name, value);
				return;
			}
			this.NormalizeCommonHeaders();
			base.InvalidateCachedArrays();
			this.InnerCollection.Set(name, value);
		}

		// Token: 0x0600243C RID: 9276 RVA: 0x0008DA36 File Offset: 0x0008CA36
		internal void AddInternal(string name, string value)
		{
			this.NormalizeCommonHeaders();
			base.InvalidateCachedArrays();
			this.InnerCollection.Add(name, value);
		}

		// Token: 0x0600243D RID: 9277 RVA: 0x0008DA51 File Offset: 0x0008CA51
		internal void ChangeInternal(string name, string value)
		{
			this.NormalizeCommonHeaders();
			base.InvalidateCachedArrays();
			this.InnerCollection.Set(name, value);
		}

		// Token: 0x0600243E RID: 9278 RVA: 0x0008DA6C File Offset: 0x0008CA6C
		internal void RemoveInternal(string name)
		{
			this.NormalizeCommonHeaders();
			if (this.m_InnerCollection != null)
			{
				base.InvalidateCachedArrays();
				this.m_InnerCollection.Remove(name);
			}
		}

		// Token: 0x0600243F RID: 9279 RVA: 0x0008DA8E File Offset: 0x0008CA8E
		internal void CheckUpdate(string name, string value)
		{
			value = WebHeaderCollection.CheckBadChars(value, true);
			this.ChangeInternal(name, value);
		}

		// Token: 0x06002440 RID: 9280 RVA: 0x0008DAA1 File Offset: 0x0008CAA1
		private void AddInternalNotCommon(string name, string value)
		{
			base.InvalidateCachedArrays();
			this.InnerCollection.Add(name, value);
		}

		// Token: 0x06002441 RID: 9281 RVA: 0x0008DAB8 File Offset: 0x0008CAB8
		internal static string CheckBadChars(string name, bool isHeaderValue)
		{
			if (name != null && name.Length != 0)
			{
				if (isHeaderValue)
				{
					name = name.Trim(WebHeaderCollection.HttpTrimCharacters);
					int num = 0;
					for (int i = 0; i < name.Length; i++)
					{
						char c = 'ÿ' & name[i];
						switch (num)
						{
						case 0:
							if (c == '\r')
							{
								num = 1;
							}
							else if (c == '\n')
							{
								num = 2;
							}
							else if (c == '\u007f' || (c < ' ' && c != '\t'))
							{
								throw new ArgumentException(SR.GetString("net_WebHeaderInvalidControlChars"), "value");
							}
							break;
						case 1:
							if (c != '\n')
							{
								throw new ArgumentException(SR.GetString("net_WebHeaderInvalidCRLFChars"), "value");
							}
							num = 2;
							break;
						case 2:
							if (c != ' ' && c != '\t')
							{
								throw new ArgumentException(SR.GetString("net_WebHeaderInvalidCRLFChars"), "value");
							}
							num = 0;
							break;
						}
					}
					if (num != 0)
					{
						throw new ArgumentException(SR.GetString("net_WebHeaderInvalidCRLFChars"), "value");
					}
				}
				else
				{
					if (name.IndexOfAny(ValidationHelper.InvalidParamChars) != -1)
					{
						throw new ArgumentException(SR.GetString("net_WebHeaderInvalidHeaderChars"), "name");
					}
					if (WebHeaderCollection.ContainsNonAsciiChars(name))
					{
						throw new ArgumentException(SR.GetString("net_WebHeaderInvalidNonAsciiChars"), "name");
					}
				}
				return name;
			}
			if (!isHeaderValue)
			{
				throw (name == null) ? new ArgumentNullException("name") : new ArgumentException(SR.GetString("net_emptystringcall", new object[] { "name" }), "name");
			}
			return string.Empty;
		}

		// Token: 0x06002442 RID: 9282 RVA: 0x0008DC30 File Offset: 0x0008CC30
		internal static bool IsValidToken(string token)
		{
			return token.Length > 0 && token.IndexOfAny(ValidationHelper.InvalidParamChars) == -1 && !WebHeaderCollection.ContainsNonAsciiChars(token);
		}

		// Token: 0x06002443 RID: 9283 RVA: 0x0008DC54 File Offset: 0x0008CC54
		internal static bool ContainsNonAsciiChars(string token)
		{
			for (int i = 0; i < token.Length; i++)
			{
				if (token[i] < ' ' || token[i] > '~')
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06002444 RID: 9284 RVA: 0x0008DC8C File Offset: 0x0008CC8C
		internal void ThrowOnRestrictedHeader(string headerName)
		{
			if (this.m_Type == WebHeaderCollectionType.HttpWebRequest)
			{
				if (WebHeaderCollection.HInfo[headerName].IsRequestRestricted)
				{
					throw new ArgumentException((!object.Equals(headerName, "Host")) ? SR.GetString("net_headerrestrict") : SR.GetString("net_headerrestrict_resp", new object[] { "Host" }), "name");
				}
			}
			else if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && WebHeaderCollection.HInfo[headerName].IsResponseRestricted)
			{
				throw new ArgumentException(SR.GetString("net_headerrestrict_resp", new object[] { headerName }), "name");
			}
		}

		// Token: 0x06002445 RID: 9285 RVA: 0x0008DD30 File Offset: 0x0008CD30
		public override void Add(string name, string value)
		{
			name = WebHeaderCollection.CheckBadChars(name, false);
			this.ThrowOnRestrictedHeader(name);
			value = WebHeaderCollection.CheckBadChars(value, true);
			if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_headers_toolong", new object[] { ushort.MaxValue }));
			}
			this.NormalizeCommonHeaders();
			base.InvalidateCachedArrays();
			this.InnerCollection.Add(name, value);
		}

		// Token: 0x06002446 RID: 9286 RVA: 0x0008DDB0 File Offset: 0x0008CDB0
		public void Add(string header)
		{
			if (ValidationHelper.IsBlankString(header))
			{
				throw new ArgumentNullException("header");
			}
			int num = header.IndexOf(':');
			if (num < 0)
			{
				throw new ArgumentException(SR.GetString("net_WebHeaderMissingColon"), "header");
			}
			string text = header.Substring(0, num);
			string text2 = header.Substring(num + 1);
			text = WebHeaderCollection.CheckBadChars(text, false);
			this.ThrowOnRestrictedHeader(text);
			text2 = WebHeaderCollection.CheckBadChars(text2, true);
			if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && text2 != null && text2.Length > 65535)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_headers_toolong", new object[] { ushort.MaxValue }));
			}
			this.NormalizeCommonHeaders();
			base.InvalidateCachedArrays();
			this.InnerCollection.Add(text, text2);
		}

		// Token: 0x06002447 RID: 9287 RVA: 0x0008DE74 File Offset: 0x0008CE74
		public override void Set(string name, string value)
		{
			if (ValidationHelper.IsBlankString(name))
			{
				throw new ArgumentNullException("name");
			}
			name = WebHeaderCollection.CheckBadChars(name, false);
			this.ThrowOnRestrictedHeader(name);
			value = WebHeaderCollection.CheckBadChars(value, true);
			if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_headers_toolong", new object[] { ushort.MaxValue }));
			}
			this.NormalizeCommonHeaders();
			base.InvalidateCachedArrays();
			this.InnerCollection.Set(name, value);
		}

		// Token: 0x06002448 RID: 9288 RVA: 0x0008DF04 File Offset: 0x0008CF04
		internal void SetInternal(string name, string value)
		{
			if (ValidationHelper.IsBlankString(name))
			{
				throw new ArgumentNullException("name");
			}
			name = WebHeaderCollection.CheckBadChars(name, false);
			value = WebHeaderCollection.CheckBadChars(value, true);
			if (this.m_Type == WebHeaderCollectionType.HttpListenerResponse && value != null && value.Length > 65535)
			{
				throw new ArgumentOutOfRangeException(SR.GetString("net_headers_toolong", new object[] { ushort.MaxValue }));
			}
			this.NormalizeCommonHeaders();
			base.InvalidateCachedArrays();
			this.InnerCollection.Set(name, value);
		}

		// Token: 0x06002449 RID: 9289 RVA: 0x0008DF90 File Offset: 0x0008CF90
		public override void Remove(string name)
		{
			if (ValidationHelper.IsBlankString(name))
			{
				throw new ArgumentNullException("name");
			}
			this.ThrowOnRestrictedHeader(name);
			name = WebHeaderCollection.CheckBadChars(name, false);
			this.NormalizeCommonHeaders();
			if (this.m_InnerCollection != null)
			{
				base.InvalidateCachedArrays();
				this.m_InnerCollection.Remove(name);
			}
		}

		// Token: 0x0600244A RID: 9290 RVA: 0x0008DFE0 File Offset: 0x0008CFE0
		public override string[] GetValues(string header)
		{
			this.NormalizeCommonHeaders();
			HeaderInfo headerInfo = WebHeaderCollection.HInfo[header];
			string[] values = this.InnerCollection.GetValues(header);
			if (headerInfo == null || values == null || !headerInfo.AllowMultiValues)
			{
				return values;
			}
			ArrayList arrayList = null;
			for (int i = 0; i < values.Length; i++)
			{
				string[] array = headerInfo.Parser(values[i]);
				if (arrayList == null)
				{
					if (array.Length > 1)
					{
						arrayList = new ArrayList(values);
						arrayList.RemoveRange(i, values.Length - i);
						arrayList.AddRange(array);
					}
				}
				else
				{
					arrayList.AddRange(array);
				}
			}
			if (arrayList != null)
			{
				string[] array2 = new string[arrayList.Count];
				arrayList.CopyTo(array2);
				return array2;
			}
			return values;
		}

		// Token: 0x0600244B RID: 9291 RVA: 0x0008E08C File Offset: 0x0008D08C
		public override string ToString()
		{
			return WebHeaderCollection.GetAsString(this, false, false);
		}

		// Token: 0x0600244C RID: 9292 RVA: 0x0008E0A3 File Offset: 0x0008D0A3
		internal string ToString(bool forTrace)
		{
			return WebHeaderCollection.GetAsString(this, false, true);
		}

		// Token: 0x0600244D RID: 9293 RVA: 0x0008E0B0 File Offset: 0x0008D0B0
		internal static string GetAsString(NameValueCollection cc, bool winInetCompat, bool forTrace)
		{
			if (cc == null || cc.Count == 0)
			{
				return "\r\n";
			}
			StringBuilder stringBuilder = new StringBuilder(30 * cc.Count);
			string text = cc[string.Empty];
			if (text != null)
			{
				stringBuilder.Append(text).Append("\r\n");
			}
			for (int i = 0; i < cc.Count; i++)
			{
				string key = cc.GetKey(i);
				string text2 = cc.Get(i);
				if (!ValidationHelper.IsBlankString(key))
				{
					stringBuilder.Append(key);
					if (winInetCompat)
					{
						stringBuilder.Append(':');
					}
					else
					{
						stringBuilder.Append(": ");
					}
					stringBuilder.Append(text2).Append("\r\n");
				}
			}
			if (!forTrace)
			{
				stringBuilder.Append("\r\n");
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600244E RID: 9294 RVA: 0x0008E174 File Offset: 0x0008D174
		public byte[] ToByteArray()
		{
			string text = this.ToString();
			return WebHeaderCollection.HeaderEncoding.GetBytes(text);
		}

		// Token: 0x0600244F RID: 9295 RVA: 0x0008E190 File Offset: 0x0008D190
		public static bool IsRestricted(string headerName)
		{
			return WebHeaderCollection.IsRestricted(headerName, false);
		}

		// Token: 0x06002450 RID: 9296 RVA: 0x0008E199 File Offset: 0x0008D199
		public static bool IsRestricted(string headerName, bool response)
		{
			if (!response)
			{
				return WebHeaderCollection.HInfo[WebHeaderCollection.CheckBadChars(headerName, false)].IsRequestRestricted;
			}
			return WebHeaderCollection.HInfo[WebHeaderCollection.CheckBadChars(headerName, false)].IsResponseRestricted;
		}

		// Token: 0x06002451 RID: 9297 RVA: 0x0008E1CB File Offset: 0x0008D1CB
		public WebHeaderCollection()
			: base(DBNull.Value)
		{
		}

		// Token: 0x06002452 RID: 9298 RVA: 0x0008E1D8 File Offset: 0x0008D1D8
		internal WebHeaderCollection(WebHeaderCollectionType type)
			: base(DBNull.Value)
		{
			this.m_Type = type;
			if (type == WebHeaderCollectionType.HttpWebResponse)
			{
				this.m_CommonHeaders = new string[WebHeaderCollection.s_CommonHeaderNames.Length - 1];
			}
		}

		// Token: 0x06002453 RID: 9299 RVA: 0x0008E204 File Offset: 0x0008D204
		internal WebHeaderCollection(NameValueCollection cc)
			: base(DBNull.Value)
		{
			this.m_InnerCollection = new NameValueCollection(cc.Count + 2, CaseInsensitiveAscii.StaticInstance);
			int count = cc.Count;
			for (int i = 0; i < count; i++)
			{
				string key = cc.GetKey(i);
				string[] values = cc.GetValues(i);
				if (values != null)
				{
					for (int j = 0; j < values.Length; j++)
					{
						this.InnerCollection.Add(key, values[j]);
					}
				}
				else
				{
					this.InnerCollection.Add(key, null);
				}
			}
		}

		// Token: 0x06002454 RID: 9300 RVA: 0x0008E28C File Offset: 0x0008D28C
		protected WebHeaderCollection(SerializationInfo serializationInfo, StreamingContext streamingContext)
			: base(DBNull.Value)
		{
			int @int = serializationInfo.GetInt32("Count");
			this.m_InnerCollection = new NameValueCollection(@int + 2, CaseInsensitiveAscii.StaticInstance);
			for (int i = 0; i < @int; i++)
			{
				string @string = serializationInfo.GetString(i.ToString(NumberFormatInfo.InvariantInfo));
				string string2 = serializationInfo.GetString((i + @int).ToString(NumberFormatInfo.InvariantInfo));
				this.InnerCollection.Add(@string, string2);
			}
		}

		// Token: 0x06002455 RID: 9301 RVA: 0x0008E307 File Offset: 0x0008D307
		public override void OnDeserialization(object sender)
		{
		}

		// Token: 0x06002456 RID: 9302 RVA: 0x0008E30C File Offset: 0x0008D30C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter)]
		public override void GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.NormalizeCommonHeaders();
			serializationInfo.AddValue("Count", this.Count);
			for (int i = 0; i < this.Count; i++)
			{
				serializationInfo.AddValue(i.ToString(NumberFormatInfo.InvariantInfo), this.GetKey(i));
				serializationInfo.AddValue((i + this.Count).ToString(NumberFormatInfo.InvariantInfo), this.Get(i));
			}
		}

		// Token: 0x06002457 RID: 9303 RVA: 0x0008E37C File Offset: 0x0008D37C
		internal unsafe DataParseStatus ParseHeaders(byte[] buffer, int size, ref int unparsed, ref int totalResponseHeadersLength, int maximumResponseHeadersLength, ref WebParseError parseError)
		{
			DataParseStatus dataParseStatus;
			try
			{
				fixed (byte* ptr = buffer)
				{
					if (buffer.Length < size)
					{
						dataParseStatus = DataParseStatus.NeedMoreData;
					}
					else
					{
						int num = -1;
						int num2 = -1;
						int num3 = -1;
						int i = unparsed;
						int num4 = totalResponseHeadersLength;
						WebParseErrorCode webParseErrorCode = WebParseErrorCode.Generic;
						for (;;)
						{
							string text = string.Empty;
							string text2 = string.Empty;
							bool flag = false;
							string text3 = null;
							if (this.Count == 0)
							{
								while (i < size)
								{
									char c = (char)ptr[i];
									if (c != ' ' && c != '\t')
									{
										break;
									}
									i++;
									if (maximumResponseHeadersLength >= 0 && ++num4 >= maximumResponseHeadersLength)
									{
										goto Block_7;
									}
								}
								if (i == size)
								{
									goto Block_8;
								}
							}
							int num5 = i;
							while (i < size)
							{
								char c = (char)ptr[i];
								if (c != ':' && c != '\n')
								{
									if (c > ' ')
									{
										num = i;
									}
									i++;
									if (maximumResponseHeadersLength >= 0 && ++num4 >= maximumResponseHeadersLength)
									{
										goto Block_13;
									}
								}
								else
								{
									if (c != ':')
									{
										break;
									}
									i++;
									if (maximumResponseHeadersLength >= 0 && ++num4 >= maximumResponseHeadersLength)
									{
										goto Block_16;
									}
									break;
								}
							}
							if (i == size)
							{
								goto Block_17;
							}
							int num6;
							for (;;)
							{
								num6 = ((this.Count == 0 && num < 0) ? 1 : 0);
								char c;
								while (i < size && num6 < 2)
								{
									c = (char)ptr[i];
									if (c > ' ')
									{
										break;
									}
									if (c == '\n')
									{
										num6++;
										if (num6 == 1)
										{
											if (i + 1 == size)
											{
												goto Block_22;
											}
											flag = ptr[i + 1] == 32 || ptr[i + 1] == 9;
										}
									}
									i++;
									if (maximumResponseHeadersLength >= 0 && ++num4 >= maximumResponseHeadersLength)
									{
										goto Block_25;
									}
								}
								if (num6 != 2 && (num6 != 1 || flag))
								{
									if (i == size)
									{
										goto Block_29;
									}
									num2 = i;
									while (i < size)
									{
										c = (char)ptr[i];
										if (c == '\n')
										{
											break;
										}
										if (c > ' ')
										{
											num3 = i;
										}
										i++;
										if (maximumResponseHeadersLength >= 0 && ++num4 >= maximumResponseHeadersLength)
										{
											goto Block_33;
										}
									}
									if (i == size)
									{
										goto Block_34;
									}
									num6 = 0;
									while (i < size && num6 < 2)
									{
										c = (char)ptr[i];
										if (c != '\r' && c != '\n')
										{
											break;
										}
										if (c == '\n')
										{
											num6++;
										}
										i++;
										if (maximumResponseHeadersLength >= 0 && ++num4 >= maximumResponseHeadersLength)
										{
											goto Block_38;
										}
									}
									if (i == size && num6 < 2)
									{
										goto Block_41;
									}
								}
								if (num2 >= 0 && num2 > num && num3 >= num2)
								{
									text2 = WebHeaderCollection.HeaderEncoding.GetString(ptr + num2, num3 - num2 + 1);
								}
								text3 = ((text3 == null) ? text2 : (text3 + " " + text2));
								if (i >= size || num6 != 1)
								{
									break;
								}
								c = (char)ptr[i];
								if (c != ' ' && c != '\t')
								{
									break;
								}
								i++;
								if (maximumResponseHeadersLength >= 0 && ++num4 >= maximumResponseHeadersLength)
								{
									goto Block_50;
								}
							}
							if (num5 >= 0 && num >= num5)
							{
								text = WebHeaderCollection.HeaderEncoding.GetString(ptr + num5, num - num5 + 1);
							}
							if (text.Length > 0)
							{
								this.AddInternal(text, text3);
							}
							totalResponseHeadersLength = num4;
							unparsed = i;
							if (num6 == 2)
							{
								goto Block_54;
							}
						}
						Block_7:
						DataParseStatus dataParseStatus2 = DataParseStatus.DataTooBig;
						goto IL_0316;
						Block_8:
						dataParseStatus2 = DataParseStatus.NeedMoreData;
						goto IL_0316;
						Block_13:
						dataParseStatus2 = DataParseStatus.DataTooBig;
						goto IL_0316;
						Block_16:
						dataParseStatus2 = DataParseStatus.DataTooBig;
						goto IL_0316;
						Block_17:
						dataParseStatus2 = DataParseStatus.NeedMoreData;
						goto IL_0316;
						Block_22:
						dataParseStatus2 = DataParseStatus.NeedMoreData;
						goto IL_0316;
						Block_25:
						dataParseStatus2 = DataParseStatus.DataTooBig;
						goto IL_0316;
						Block_29:
						dataParseStatus2 = DataParseStatus.NeedMoreData;
						goto IL_0316;
						Block_33:
						dataParseStatus2 = DataParseStatus.DataTooBig;
						goto IL_0316;
						Block_34:
						dataParseStatus2 = DataParseStatus.NeedMoreData;
						goto IL_0316;
						Block_38:
						dataParseStatus2 = DataParseStatus.DataTooBig;
						goto IL_0316;
						Block_41:
						dataParseStatus2 = DataParseStatus.NeedMoreData;
						goto IL_0316;
						Block_50:
						dataParseStatus2 = DataParseStatus.DataTooBig;
						goto IL_0316;
						Block_54:
						dataParseStatus2 = DataParseStatus.Done;
						IL_0316:
						if (dataParseStatus2 == DataParseStatus.Invalid)
						{
							parseError.Section = WebParseErrorSection.ResponseHeader;
							parseError.Code = webParseErrorCode;
						}
						dataParseStatus = dataParseStatus2;
					}
				}
			}
			finally
			{
				byte* ptr = null;
			}
			return dataParseStatus;
		}

		// Token: 0x06002458 RID: 9304 RVA: 0x0008E6E0 File Offset: 0x0008D6E0
		internal unsafe DataParseStatus ParseHeadersStrict(byte[] buffer, int size, ref int unparsed, ref int totalResponseHeadersLength, int maximumResponseHeadersLength, ref WebParseError parseError)
		{
			WebParseErrorCode webParseErrorCode = WebParseErrorCode.Generic;
			DataParseStatus dataParseStatus = DataParseStatus.Invalid;
			int num = unparsed;
			int num2 = ((maximumResponseHeadersLength <= 0) ? int.MaxValue : (maximumResponseHeadersLength - totalResponseHeadersLength + num));
			DataParseStatus dataParseStatus2 = DataParseStatus.DataTooBig;
			if (size < num2)
			{
				num2 = size;
				dataParseStatus2 = DataParseStatus.NeedMoreData;
			}
			if (num >= num2)
			{
				dataParseStatus = dataParseStatus2;
			}
			else
			{
				try
				{
					fixed (byte* ptr = buffer)
					{
						while (ptr[num] != 13)
						{
							int num3 = num;
							while (num < num2 && ((ptr[num] > 127) ? WebHeaderCollection.RfcChar.High : WebHeaderCollection.RfcCharMap[(int)ptr[num]]) == WebHeaderCollection.RfcChar.Reg)
							{
								num++;
							}
							if (num == num2)
							{
								dataParseStatus = dataParseStatus2;
								goto IL_042C;
							}
							if (num == num3)
							{
								dataParseStatus = DataParseStatus.Invalid;
								webParseErrorCode = WebParseErrorCode.InvalidHeaderName;
								goto IL_042C;
							}
							int num4 = num - 1;
							int num5 = 0;
							WebHeaderCollection.RfcChar rfcChar;
							while (num < num2 && (rfcChar = ((ptr[num] > 127) ? WebHeaderCollection.RfcChar.High : WebHeaderCollection.RfcCharMap[(int)ptr[num]])) != WebHeaderCollection.RfcChar.Colon)
							{
								switch (rfcChar)
								{
								case WebHeaderCollection.RfcChar.CR:
									if (num5 != 0)
									{
										goto IL_0122;
									}
									num5 = 1;
									break;
								case WebHeaderCollection.RfcChar.LF:
									if (num5 != 1)
									{
										goto IL_0122;
									}
									num5 = 2;
									break;
								case WebHeaderCollection.RfcChar.WS:
									if (num5 == 1)
									{
										goto IL_0122;
									}
									num5 = 0;
									break;
								default:
									goto IL_0122;
								}
								num++;
								continue;
								IL_0122:
								dataParseStatus = DataParseStatus.Invalid;
								webParseErrorCode = WebParseErrorCode.CrLfError;
								goto IL_042C;
							}
							if (num == num2)
							{
								dataParseStatus = dataParseStatus2;
								goto IL_042C;
							}
							if (num5 != 0)
							{
								dataParseStatus = DataParseStatus.Invalid;
								webParseErrorCode = WebParseErrorCode.IncompleteHeaderLine;
								goto IL_042C;
							}
							if (++num == num2)
							{
								dataParseStatus = dataParseStatus2;
								goto IL_042C;
							}
							int num6 = -1;
							int num7 = -1;
							StringBuilder stringBuilder = null;
							while (num < num2 && ((rfcChar = ((ptr[num] > 127) ? WebHeaderCollection.RfcChar.High : WebHeaderCollection.RfcCharMap[(int)ptr[num]])) == WebHeaderCollection.RfcChar.WS || num5 != 2))
							{
								switch (rfcChar)
								{
								case WebHeaderCollection.RfcChar.High:
								case WebHeaderCollection.RfcChar.Reg:
								case WebHeaderCollection.RfcChar.Colon:
								case WebHeaderCollection.RfcChar.Delim:
									if (num5 == 1)
									{
										goto IL_024A;
									}
									if (num5 == 3)
									{
										num5 = 0;
										if (num6 != -1)
										{
											string @string = WebHeaderCollection.HeaderEncoding.GetString(ptr + num6, num7 - num6 + 1);
											if (stringBuilder == null)
											{
												stringBuilder = new StringBuilder(@string, @string.Length * 5);
											}
											else
											{
												stringBuilder.Append(" ");
												stringBuilder.Append(@string);
											}
										}
										num6 = -1;
									}
									if (num6 == -1)
									{
										num6 = num;
									}
									num7 = num;
									break;
								case WebHeaderCollection.RfcChar.Ctl:
									goto IL_024A;
								case WebHeaderCollection.RfcChar.CR:
									if (num5 != 0)
									{
										goto IL_024A;
									}
									num5 = 1;
									break;
								case WebHeaderCollection.RfcChar.LF:
									if (num5 != 1)
									{
										goto IL_024A;
									}
									num5 = 2;
									break;
								case WebHeaderCollection.RfcChar.WS:
									if (num5 == 1)
									{
										goto IL_024A;
									}
									if (num5 == 2)
									{
										num5 = 3;
									}
									break;
								default:
									goto IL_024A;
								}
								num++;
								continue;
								IL_024A:
								dataParseStatus = DataParseStatus.Invalid;
								webParseErrorCode = WebParseErrorCode.CrLfError;
								goto IL_042C;
							}
							if (num == num2)
							{
								dataParseStatus = dataParseStatus2;
								goto IL_042C;
							}
							string text = ((num6 == -1) ? "" : WebHeaderCollection.HeaderEncoding.GetString(ptr + num6, num7 - num6 + 1));
							if (stringBuilder != null)
							{
								if (text.Length != 0)
								{
									stringBuilder.Append(" ");
									stringBuilder.Append(text);
								}
								text = stringBuilder.ToString();
							}
							string text2 = null;
							int num8 = num4 - num3 + 1;
							if (this.m_CommonHeaders != null)
							{
								int num9 = (int)WebHeaderCollection.s_CommonHeaderHints[(int)(ptr[num3] & 31)];
								if (num9 >= 0)
								{
									string text3;
									for (;;)
									{
										text3 = WebHeaderCollection.s_CommonHeaderNames[num9++];
										if (text3.Length < num8 || CaseInsensitiveAscii.AsciiToLower[(int)ptr[num3]] != CaseInsensitiveAscii.AsciiToLower[(int)text3[0]])
										{
											goto IL_03F8;
										}
										if (text3.Length <= num8)
										{
											byte* ptr2 = ptr + num3 + 1;
											int num10 = 1;
											while (num10 < text3.Length && ((char)(*(ptr2++)) == text3[num10] || CaseInsensitiveAscii.AsciiToLower[(int)(*(ptr2 - 1))] == CaseInsensitiveAscii.AsciiToLower[(int)text3[num10]]))
											{
												num10++;
											}
											if (num10 == text3.Length)
											{
												break;
											}
										}
									}
									this.m_NumCommonHeaders++;
									num9--;
									if (this.m_CommonHeaders[num9] == null)
									{
										this.m_CommonHeaders[num9] = text;
									}
									else
									{
										this.NormalizeCommonHeaders();
										this.AddInternalNotCommon(text3, text);
									}
									text2 = text3;
								}
							}
							IL_03F8:
							if (text2 == null)
							{
								text2 = WebHeaderCollection.HeaderEncoding.GetString(ptr + num3, num8);
								this.AddInternalNotCommon(text2, text);
							}
							totalResponseHeadersLength += num - unparsed;
							unparsed = num;
						}
						if (++num == num2)
						{
							dataParseStatus = dataParseStatus2;
						}
						else if (ptr[num++] == 10)
						{
							totalResponseHeadersLength += num - unparsed;
							unparsed = num;
							dataParseStatus = DataParseStatus.Done;
						}
						else
						{
							dataParseStatus = DataParseStatus.Invalid;
							webParseErrorCode = WebParseErrorCode.CrLfError;
						}
					}
				}
				finally
				{
					byte* ptr = null;
				}
			}
			IL_042C:
			if (dataParseStatus == DataParseStatus.Invalid)
			{
				parseError.Section = WebParseErrorSection.ResponseHeader;
				parseError.Code = webParseErrorCode;
			}
			return dataParseStatus;
		}

		// Token: 0x06002459 RID: 9305 RVA: 0x0008EB4C File Offset: 0x0008DB4C
		[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.SerializationFormatter, SerializationFormatter = true)]
		void ISerializable.GetObjectData(SerializationInfo serializationInfo, StreamingContext streamingContext)
		{
			this.GetObjectData(serializationInfo, streamingContext);
		}

		// Token: 0x0600245A RID: 9306 RVA: 0x0008EB58 File Offset: 0x0008DB58
		public override string Get(string name)
		{
			if (this.m_CommonHeaders != null && name != null && name.Length > 0 && name[0] < 'Ā')
			{
				int num = (int)WebHeaderCollection.s_CommonHeaderHints[(int)(name[0] & '\u001f')];
				if (num >= 0)
				{
					for (;;)
					{
						string text = WebHeaderCollection.s_CommonHeaderNames[num++];
						if (text.Length < name.Length || CaseInsensitiveAscii.AsciiToLower[(int)name[0]] != CaseInsensitiveAscii.AsciiToLower[(int)text[0]])
						{
							goto IL_00EF;
						}
						if (text.Length <= name.Length)
						{
							int num2 = 1;
							while (num2 < text.Length && (name[num2] == text[num2] || (name[num2] <= 'ÿ' && CaseInsensitiveAscii.AsciiToLower[(int)name[num2]] == CaseInsensitiveAscii.AsciiToLower[(int)text[num2]])))
							{
								num2++;
							}
							if (num2 == text.Length)
							{
								break;
							}
						}
					}
					return this.m_CommonHeaders[num - 1];
				}
			}
			IL_00EF:
			if (this.m_InnerCollection == null)
			{
				return null;
			}
			return this.m_InnerCollection.Get(name);
		}

		// Token: 0x0600245B RID: 9307 RVA: 0x0008EC6A File Offset: 0x0008DC6A
		public override IEnumerator GetEnumerator()
		{
			this.NormalizeCommonHeaders();
			return new NameObjectCollectionBase.NameObjectKeysEnumerator(this.InnerCollection);
		}

		// Token: 0x17000788 RID: 1928
		// (get) Token: 0x0600245C RID: 9308 RVA: 0x0008EC7D File Offset: 0x0008DC7D
		public override int Count
		{
			get
			{
				return ((this.m_InnerCollection == null) ? 0 : this.m_InnerCollection.Count) + this.m_NumCommonHeaders;
			}
		}

		// Token: 0x17000789 RID: 1929
		// (get) Token: 0x0600245D RID: 9309 RVA: 0x0008EC9C File Offset: 0x0008DC9C
		public override NameObjectCollectionBase.KeysCollection Keys
		{
			get
			{
				this.NormalizeCommonHeaders();
				return this.InnerCollection.Keys;
			}
		}

		// Token: 0x0600245E RID: 9310 RVA: 0x0008ECAF File Offset: 0x0008DCAF
		internal override bool InternalHasKeys()
		{
			this.NormalizeCommonHeaders();
			return this.m_InnerCollection != null && this.m_InnerCollection.HasKeys();
		}

		// Token: 0x0600245F RID: 9311 RVA: 0x0008ECCC File Offset: 0x0008DCCC
		public override string Get(int index)
		{
			this.NormalizeCommonHeaders();
			return this.InnerCollection.Get(index);
		}

		// Token: 0x06002460 RID: 9312 RVA: 0x0008ECE0 File Offset: 0x0008DCE0
		public override string[] GetValues(int index)
		{
			this.NormalizeCommonHeaders();
			return this.InnerCollection.GetValues(index);
		}

		// Token: 0x06002461 RID: 9313 RVA: 0x0008ECF4 File Offset: 0x0008DCF4
		public override string GetKey(int index)
		{
			this.NormalizeCommonHeaders();
			return this.InnerCollection.GetKey(index);
		}

		// Token: 0x1700078A RID: 1930
		// (get) Token: 0x06002462 RID: 9314 RVA: 0x0008ED08 File Offset: 0x0008DD08
		public override string[] AllKeys
		{
			get
			{
				this.NormalizeCommonHeaders();
				return this.InnerCollection.AllKeys;
			}
		}

		// Token: 0x06002463 RID: 9315 RVA: 0x0008ED1B File Offset: 0x0008DD1B
		public override void Clear()
		{
			this.m_CommonHeaders = null;
			this.m_NumCommonHeaders = 0;
			base.InvalidateCachedArrays();
			if (this.m_InnerCollection != null)
			{
				this.m_InnerCollection.Clear();
			}
		}

		// Token: 0x04002492 RID: 9362
		private const int ApproxAveHeaderLineSize = 30;

		// Token: 0x04002493 RID: 9363
		private const int ApproxHighAvgNumHeaders = 16;

		// Token: 0x04002494 RID: 9364
		private const int c_AcceptRanges = 0;

		// Token: 0x04002495 RID: 9365
		private const int c_ContentLength = 1;

		// Token: 0x04002496 RID: 9366
		private const int c_CacheControl = 2;

		// Token: 0x04002497 RID: 9367
		private const int c_ContentType = 3;

		// Token: 0x04002498 RID: 9368
		private const int c_Date = 4;

		// Token: 0x04002499 RID: 9369
		private const int c_Expires = 5;

		// Token: 0x0400249A RID: 9370
		private const int c_ETag = 6;

		// Token: 0x0400249B RID: 9371
		private const int c_LastModified = 7;

		// Token: 0x0400249C RID: 9372
		private const int c_Location = 8;

		// Token: 0x0400249D RID: 9373
		private const int c_ProxyAuthenticate = 9;

		// Token: 0x0400249E RID: 9374
		private const int c_P3P = 10;

		// Token: 0x0400249F RID: 9375
		private const int c_SetCookie2 = 11;

		// Token: 0x040024A0 RID: 9376
		private const int c_SetCookie = 12;

		// Token: 0x040024A1 RID: 9377
		private const int c_Server = 13;

		// Token: 0x040024A2 RID: 9378
		private const int c_Via = 14;

		// Token: 0x040024A3 RID: 9379
		private const int c_WwwAuthenticate = 15;

		// Token: 0x040024A4 RID: 9380
		private const int c_XAspNetVersion = 16;

		// Token: 0x040024A5 RID: 9381
		private const int c_XPoweredBy = 17;

		// Token: 0x040024A6 RID: 9382
		private static readonly HeaderInfoTable HInfo = new HeaderInfoTable();

		// Token: 0x040024A7 RID: 9383
		private string[] m_CommonHeaders;

		// Token: 0x040024A8 RID: 9384
		private int m_NumCommonHeaders;

		// Token: 0x040024A9 RID: 9385
		private static readonly string[] s_CommonHeaderNames = new string[]
		{
			"Accept-Ranges", "Content-Length", "Cache-Control", "Content-Type", "Date", "Expires", "ETag", "Last-Modified", "Location", "Proxy-Authenticate",
			"P3P", "Set-Cookie2", "Set-Cookie", "Server", "Via", "WWW-Authenticate", "X-AspNet-Version", "X-Powered-By", "["
		};

		// Token: 0x040024AA RID: 9386
		private static readonly sbyte[] s_CommonHeaderHints = new sbyte[]
		{
			-1, 0, -1, 1, 4, 5, -1, -1, -1, -1,
			-1, -1, 7, -1, -1, -1, 9, -1, -1, 11,
			-1, -1, 14, 15, 16, -1, -1, -1, -1, -1,
			-1, -1
		};

		// Token: 0x040024AB RID: 9387
		private NameValueCollection m_InnerCollection;

		// Token: 0x040024AC RID: 9388
		private WebHeaderCollectionType m_Type;

		// Token: 0x040024AD RID: 9389
		private static readonly char[] HttpTrimCharacters = new char[] { '\t', '\n', '\v', '\f', '\r', ' ' };

		// Token: 0x040024AE RID: 9390
		private static WebHeaderCollection.RfcChar[] RfcCharMap = new WebHeaderCollection.RfcChar[]
		{
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.WS,
			WebHeaderCollection.RfcChar.LF,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.CR,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.Ctl,
			WebHeaderCollection.RfcChar.WS,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Colon,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Delim,
			WebHeaderCollection.RfcChar.Reg,
			WebHeaderCollection.RfcChar.Ctl
		};

		// Token: 0x020004A4 RID: 1188
		internal static class HeaderEncoding
		{
			// Token: 0x06002465 RID: 9317 RVA: 0x0008F0F0 File Offset: 0x0008E0F0
			internal unsafe static string GetString(byte[] bytes, int byteIndex, int byteCount)
			{
				fixed (byte* ptr = bytes)
				{
					return WebHeaderCollection.HeaderEncoding.GetString(ptr + byteIndex, byteCount);
				}
			}

			// Token: 0x06002466 RID: 9318 RVA: 0x0008F124 File Offset: 0x0008E124
			internal unsafe static string GetString(byte* pBytes, int byteCount)
			{
				if (byteCount < 1)
				{
					return "";
				}
				string text = new string('\0', byteCount);
				fixed (char* ptr = text)
				{
					char* ptr2 = ptr;
					while (byteCount >= 8)
					{
						*ptr2 = (char)(*pBytes);
						ptr2[1] = (char)pBytes[1];
						ptr2[2] = (char)pBytes[2];
						ptr2[3] = (char)pBytes[3];
						ptr2[4] = (char)pBytes[4];
						ptr2[5] = (char)pBytes[5];
						ptr2[6] = (char)pBytes[6];
						ptr2[7] = (char)pBytes[7];
						ptr2 += 8;
						pBytes += 8;
						byteCount -= 8;
					}
					for (int i = 0; i < byteCount; i++)
					{
						ptr2[i] = (char)pBytes[i];
					}
				}
				return text;
			}

			// Token: 0x06002467 RID: 9319 RVA: 0x0008F1D4 File Offset: 0x0008E1D4
			internal static int GetByteCount(string myString)
			{
				return myString.Length;
			}

			// Token: 0x06002468 RID: 9320 RVA: 0x0008F1DC File Offset: 0x0008E1DC
			internal unsafe static void GetBytes(string myString, int charIndex, int charCount, byte[] bytes, int byteIndex)
			{
				if (myString.Length == 0)
				{
					return;
				}
				fixed (byte* ptr = bytes)
				{
					byte* ptr2 = ptr + byteIndex;
					int num = charIndex + charCount;
					while (charIndex < num)
					{
						*(ptr2++) = (byte)myString[charIndex++];
					}
				}
			}

			// Token: 0x06002469 RID: 9321 RVA: 0x0008F230 File Offset: 0x0008E230
			internal static byte[] GetBytes(string myString)
			{
				byte[] array = new byte[myString.Length];
				if (myString.Length != 0)
				{
					WebHeaderCollection.HeaderEncoding.GetBytes(myString, 0, myString.Length, array, 0);
				}
				return array;
			}
		}

		// Token: 0x020004A5 RID: 1189
		private enum RfcChar : byte
		{
			// Token: 0x040024B0 RID: 9392
			High,
			// Token: 0x040024B1 RID: 9393
			Reg,
			// Token: 0x040024B2 RID: 9394
			Ctl,
			// Token: 0x040024B3 RID: 9395
			CR,
			// Token: 0x040024B4 RID: 9396
			LF,
			// Token: 0x040024B5 RID: 9397
			WS,
			// Token: 0x040024B6 RID: 9398
			Colon,
			// Token: 0x040024B7 RID: 9399
			Delim
		}
	}
}
