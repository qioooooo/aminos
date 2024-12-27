using System;
using System.Collections;
using System.Globalization;
using System.Text;
using System.Text.RegularExpressions;

namespace System.Net
{
	// Token: 0x0200050A RID: 1290
	internal abstract class WebProxyDataBuilder
	{
		// Token: 0x06002801 RID: 10241 RVA: 0x000A4F1C File Offset: 0x000A3F1C
		public WebProxyData Build()
		{
			this.m_Result = new WebProxyData();
			this.BuildInternal();
			return this.m_Result;
		}

		// Token: 0x06002802 RID: 10242
		protected abstract void BuildInternal();

		// Token: 0x06002803 RID: 10243 RVA: 0x000A4F38 File Offset: 0x000A3F38
		protected void SetProxyAndBypassList(string addressString, string bypassListString)
		{
			Uri uri = null;
			Hashtable hashtable = null;
			if (addressString != null)
			{
				uri = WebProxyDataBuilder.ParseProxyUri(addressString, true);
				if (uri == null)
				{
					hashtable = WebProxyDataBuilder.ParseProtocolProxies(addressString);
				}
				if ((uri != null || hashtable != null) && bypassListString != null)
				{
					bool flag = false;
					this.m_Result.bypassList = WebProxyDataBuilder.ParseBypassList(bypassListString, out flag);
					this.m_Result.bypassOnLocal = flag;
				}
			}
			if (hashtable != null)
			{
				uri = hashtable["http"] as Uri;
			}
			this.m_Result.proxyAddress = uri;
		}

		// Token: 0x06002804 RID: 10244 RVA: 0x000A4FB4 File Offset: 0x000A3FB4
		protected void SetAutoProxyUrl(string autoConfigUrl)
		{
			if (!string.IsNullOrEmpty(autoConfigUrl))
			{
				Uri uri = null;
				if (Uri.TryCreate(autoConfigUrl, UriKind.Absolute, out uri))
				{
					this.m_Result.scriptLocation = uri;
				}
			}
		}

		// Token: 0x06002805 RID: 10245 RVA: 0x000A4FE2 File Offset: 0x000A3FE2
		protected void SetAutoDetectSettings(bool value)
		{
			this.m_Result.automaticallyDetectSettings = value;
		}

		// Token: 0x06002806 RID: 10246 RVA: 0x000A4FF0 File Offset: 0x000A3FF0
		private static Uri ParseProxyUri(string proxyString, bool validate)
		{
			if (validate)
			{
				if (proxyString.Length == 0)
				{
					return null;
				}
				if (proxyString.IndexOf('=') != -1)
				{
					return null;
				}
			}
			if (proxyString.IndexOf("://") == -1)
			{
				proxyString = "http://" + proxyString;
			}
			try
			{
				return new Uri(proxyString);
			}
			catch (UriFormatException ex)
			{
				if (Logging.On)
				{
					Logging.PrintError(Logging.Web, ex.Message);
				}
			}
			return null;
		}

		// Token: 0x06002807 RID: 10247 RVA: 0x000A5068 File Offset: 0x000A4068
		private static Hashtable ParseProtocolProxies(string proxyListString)
		{
			if (proxyListString.Length == 0)
			{
				return null;
			}
			string[] array = proxyListString.Split(WebProxyDataBuilder.s_AddressListSplitChars);
			bool flag = true;
			string text = null;
			Hashtable hashtable = new Hashtable(CaseInsensitiveAscii.StaticInstance);
			foreach (string text2 in array)
			{
				string text3 = text2.Trim().ToLower(CultureInfo.InvariantCulture);
				if (flag)
				{
					text = text3;
				}
				else
				{
					hashtable[text] = WebProxyDataBuilder.ParseProxyUri(text3, false);
				}
				flag = !flag;
			}
			if (hashtable.Count == 0)
			{
				return null;
			}
			return hashtable;
		}

		// Token: 0x06002808 RID: 10248 RVA: 0x000A50F4 File Offset: 0x000A40F4
		private static string BypassStringEscape(string rawString)
		{
			Regex regex = new Regex("^(?<scheme>.*://)?(?<host>[^:]*)(?<port>:[0-9]{1,5})?$", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant);
			Match match = regex.Match(rawString);
			string text;
			string text2;
			string text3;
			if (match.Success)
			{
				text = match.Groups["scheme"].Value;
				text2 = match.Groups["host"].Value;
				text3 = match.Groups["port"].Value;
			}
			else
			{
				text = string.Empty;
				text2 = rawString;
				text3 = string.Empty;
			}
			text = WebProxyDataBuilder.ConvertRegexReservedChars(text);
			text2 = WebProxyDataBuilder.ConvertRegexReservedChars(text2);
			text3 = WebProxyDataBuilder.ConvertRegexReservedChars(text3);
			if (text == string.Empty)
			{
				text = "(?:.*://)?";
			}
			if (text3 == string.Empty)
			{
				text3 = "(?::[0-9]{1,5})?";
			}
			return string.Concat(new string[] { "^", text, text2, text3, "$" });
		}

		// Token: 0x06002809 RID: 10249 RVA: 0x000A51E8 File Offset: 0x000A41E8
		private static string ConvertRegexReservedChars(string rawString)
		{
			if (rawString.Length == 0)
			{
				return rawString;
			}
			StringBuilder stringBuilder = new StringBuilder();
			foreach (char c in rawString)
			{
				if ("#$()+.?[\\^{|".IndexOf(c) != -1)
				{
					stringBuilder.Append('\\');
				}
				else if (c == '*')
				{
					stringBuilder.Append('.');
				}
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600280A RID: 10250 RVA: 0x000A5258 File Offset: 0x000A4258
		private static ArrayList ParseBypassList(string bypassListString, out bool bypassOnLocal)
		{
			string[] array = bypassListString.Split(WebProxyDataBuilder.s_BypassListDelimiter);
			bypassOnLocal = false;
			if (array.Length == 0)
			{
				return null;
			}
			ArrayList arrayList = null;
			foreach (string text in array)
			{
				if (text != null)
				{
					string text2 = text.Trim();
					if (text2.Length > 0)
					{
						if (string.Compare(text2, "<local>", StringComparison.OrdinalIgnoreCase) == 0)
						{
							bypassOnLocal = true;
						}
						else
						{
							text2 = WebProxyDataBuilder.BypassStringEscape(text2);
							if (arrayList == null)
							{
								arrayList = new ArrayList();
							}
							if (!arrayList.Contains(text2))
							{
								arrayList.Add(text2);
							}
						}
					}
				}
			}
			return arrayList;
		}

		// Token: 0x0400274B RID: 10059
		private const string regexReserved = "#$()+.?[\\^{|";

		// Token: 0x0400274C RID: 10060
		private static readonly char[] s_AddressListSplitChars = new char[] { ';', '=' };

		// Token: 0x0400274D RID: 10061
		private static readonly char[] s_BypassListDelimiter = new char[] { ';' };

		// Token: 0x0400274E RID: 10062
		private WebProxyData m_Result;
	}
}
