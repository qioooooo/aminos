using System;
using System.Collections;
using System.Collections.Specialized;
using System.Runtime.Serialization;
using System.Text;
using System.Web.Util;

namespace System.Web
{
	// Token: 0x02000073 RID: 115
	[Serializable]
	internal class HttpValueCollection : NameValueCollection
	{
		// Token: 0x060004F0 RID: 1264 RVA: 0x000144F2 File Offset: 0x000134F2
		internal HttpValueCollection()
			: base(StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x060004F1 RID: 1265 RVA: 0x000144FF File Offset: 0x000134FF
		internal HttpValueCollection(string str, bool readOnly, bool urlencoded, Encoding encoding)
			: base(StringComparer.OrdinalIgnoreCase)
		{
			if (!string.IsNullOrEmpty(str))
			{
				this.FillFromString(str, urlencoded, encoding);
			}
			base.IsReadOnly = readOnly;
		}

		// Token: 0x060004F2 RID: 1266 RVA: 0x00014525 File Offset: 0x00013525
		internal HttpValueCollection(int capacity)
			: base(capacity, StringComparer.OrdinalIgnoreCase)
		{
		}

		// Token: 0x060004F3 RID: 1267 RVA: 0x00014533 File Offset: 0x00013533
		protected HttpValueCollection(SerializationInfo info, StreamingContext context)
			: base(info, context)
		{
		}

		// Token: 0x060004F4 RID: 1268 RVA: 0x0001453D File Offset: 0x0001353D
		internal void MakeReadOnly()
		{
			base.IsReadOnly = true;
		}

		// Token: 0x060004F5 RID: 1269 RVA: 0x00014546 File Offset: 0x00013546
		internal void MakeReadWrite()
		{
			base.IsReadOnly = false;
		}

		// Token: 0x060004F6 RID: 1270 RVA: 0x0001454F File Offset: 0x0001354F
		internal void FillFromString(string s)
		{
			this.FillFromString(s, false, null);
		}

		// Token: 0x060004F7 RID: 1271 RVA: 0x0001455C File Offset: 0x0001355C
		internal void FillFromString(string s, bool urlencoded, Encoding encoding)
		{
			int num = ((s != null) ? s.Length : 0);
			for (int i = 0; i < num; i++)
			{
				this.ThrowIfMaxHttpCollectionKeysExceeded();
				int num2 = i;
				int num3 = -1;
				while (i < num)
				{
					char c = s[i];
					if (c == '=')
					{
						if (num3 < 0)
						{
							num3 = i;
						}
					}
					else if (c == '&')
					{
						break;
					}
					i++;
				}
				string text = null;
				string text2;
				if (num3 >= 0)
				{
					text = s.Substring(num2, num3 - num2);
					text2 = s.Substring(num3 + 1, i - num3 - 1);
				}
				else
				{
					text2 = s.Substring(num2, i - num2);
				}
				if (urlencoded)
				{
					base.Add(HttpUtility.UrlDecode(text, encoding), HttpUtility.UrlDecode(text2, encoding));
				}
				else
				{
					base.Add(text, text2);
				}
				if (i == num - 1 && s[i] == '&')
				{
					base.Add(null, string.Empty);
				}
			}
		}

		// Token: 0x060004F8 RID: 1272 RVA: 0x00014630 File Offset: 0x00013630
		internal void FillFromEncodedBytes(byte[] bytes, Encoding encoding)
		{
			int num = ((bytes != null) ? bytes.Length : 0);
			for (int i = 0; i < num; i++)
			{
				this.ThrowIfMaxHttpCollectionKeysExceeded();
				int num2 = i;
				int num3 = -1;
				while (i < num)
				{
					byte b = bytes[i];
					if (b == 61)
					{
						if (num3 < 0)
						{
							num3 = i;
						}
					}
					else if (b == 38)
					{
						break;
					}
					i++;
				}
				string text;
				string text2;
				if (num3 >= 0)
				{
					text = HttpUtility.UrlDecode(bytes, num2, num3 - num2, encoding);
					text2 = HttpUtility.UrlDecode(bytes, num3 + 1, i - num3 - 1, encoding);
				}
				else
				{
					text = null;
					text2 = HttpUtility.UrlDecode(bytes, num2, i - num2, encoding);
				}
				base.Add(text, text2);
				if (i == num - 1 && bytes[i] == 38)
				{
					base.Add(null, string.Empty);
				}
			}
		}

		// Token: 0x060004F9 RID: 1273 RVA: 0x000146E0 File Offset: 0x000136E0
		internal void Add(HttpCookieCollection c)
		{
			int count = c.Count;
			for (int i = 0; i < count; i++)
			{
				this.ThrowIfMaxHttpCollectionKeysExceeded();
				HttpCookie httpCookie = c.Get(i);
				base.Add(httpCookie.Name, httpCookie.Value);
			}
		}

		// Token: 0x060004FA RID: 1274 RVA: 0x00014720 File Offset: 0x00013720
		private void ThrowIfMaxHttpCollectionKeysExceeded()
		{
			if (this.Count >= AppSettings.MaxHttpCollectionKeys)
			{
				throw new InvalidOperationException();
			}
		}

		// Token: 0x060004FB RID: 1275 RVA: 0x00014735 File Offset: 0x00013735
		internal void Reset()
		{
			base.Clear();
		}

		// Token: 0x060004FC RID: 1276 RVA: 0x0001473D File Offset: 0x0001373D
		public override string ToString()
		{
			return this.ToString(true);
		}

		// Token: 0x060004FD RID: 1277 RVA: 0x00014746 File Offset: 0x00013746
		internal virtual string ToString(bool urlencoded)
		{
			return this.ToString(urlencoded, null);
		}

		// Token: 0x060004FE RID: 1278 RVA: 0x00014750 File Offset: 0x00013750
		internal virtual string ToString(bool urlencoded, IDictionary excludeKeys)
		{
			int count = this.Count;
			if (count == 0)
			{
				return string.Empty;
			}
			StringBuilder stringBuilder = new StringBuilder();
			bool flag = excludeKeys != null && excludeKeys["__VIEWSTATE"] != null;
			for (int i = 0; i < count; i++)
			{
				string text = this.GetKey(i);
				if ((!flag || text == null || !text.StartsWith("__VIEWSTATE", StringComparison.Ordinal)) && (excludeKeys == null || text == null || excludeKeys[text] == null))
				{
					if (urlencoded)
					{
						text = HttpUtility.UrlEncodeUnicode(text);
					}
					string text2 = ((!string.IsNullOrEmpty(text)) ? (text + "=") : string.Empty);
					ArrayList arrayList = (ArrayList)base.BaseGet(i);
					int num = ((arrayList != null) ? arrayList.Count : 0);
					if (stringBuilder.Length > 0)
					{
						stringBuilder.Append('&');
					}
					if (num == 1)
					{
						stringBuilder.Append(text2);
						string text3 = (string)arrayList[0];
						if (urlencoded)
						{
							text3 = HttpUtility.UrlEncodeUnicode(text3);
						}
						stringBuilder.Append(text3);
					}
					else if (num == 0)
					{
						stringBuilder.Append(text2);
					}
					else
					{
						for (int j = 0; j < num; j++)
						{
							if (j > 0)
							{
								stringBuilder.Append('&');
							}
							stringBuilder.Append(text2);
							string text3 = (string)arrayList[j];
							if (urlencoded)
							{
								text3 = HttpUtility.UrlEncodeUnicode(text3);
							}
							stringBuilder.Append(text3);
						}
					}
				}
			}
			return stringBuilder.ToString();
		}
	}
}
