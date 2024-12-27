using System;
using System.Globalization;
using System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace System.Security.Util
{
	// Token: 0x02000474 RID: 1140
	[Serializable]
	internal sealed class URLString : SiteString
	{
		// Token: 0x06002DD2 RID: 11730 RVA: 0x0009A956 File Offset: 0x00099956
		[OnDeserialized]
		public void OnDeserialized(StreamingContext ctx)
		{
			if (this.m_urlOriginal == null)
			{
				this.m_parseDeferred = false;
				this.m_parsedOriginal = false;
				this.m_userpass = "";
				this.m_urlOriginal = this.m_fullurl;
				this.m_fullurl = null;
			}
		}

		// Token: 0x06002DD3 RID: 11731 RVA: 0x0009A98C File Offset: 0x0009998C
		[OnSerializing]
		private void OnSerializing(StreamingContext ctx)
		{
			if ((ctx.State & ~(StreamingContextStates.Clone | StreamingContextStates.CrossAppDomain)) != (StreamingContextStates)0)
			{
				this.DoDeferredParse();
				this.m_fullurl = this.m_urlOriginal;
			}
		}

		// Token: 0x06002DD4 RID: 11732 RVA: 0x0009A9AF File Offset: 0x000999AF
		[OnSerialized]
		private void OnSerialized(StreamingContext ctx)
		{
			if ((ctx.State & ~(StreamingContextStates.Clone | StreamingContextStates.CrossAppDomain)) != (StreamingContextStates)0)
			{
				this.m_fullurl = null;
			}
		}

		// Token: 0x06002DD5 RID: 11733 RVA: 0x0009A9C8 File Offset: 0x000999C8
		public URLString()
		{
			this.m_protocol = "";
			this.m_userpass = "";
			this.m_siteString = new SiteString();
			this.m_port = -1;
			this.m_localSite = null;
			this.m_directory = new DirectoryString();
			this.m_parseDeferred = false;
		}

		// Token: 0x06002DD6 RID: 11734 RVA: 0x0009AA1C File Offset: 0x00099A1C
		private void DoDeferredParse()
		{
			if (this.m_parseDeferred)
			{
				this.ParseString(this.m_urlOriginal, this.m_parsedOriginal);
				this.m_parseDeferred = false;
			}
		}

		// Token: 0x06002DD7 RID: 11735 RVA: 0x0009AA3F File Offset: 0x00099A3F
		public URLString(string url)
			: this(url, false, false)
		{
		}

		// Token: 0x06002DD8 RID: 11736 RVA: 0x0009AA4A File Offset: 0x00099A4A
		public URLString(string url, bool parsed)
			: this(url, parsed, false)
		{
		}

		// Token: 0x06002DD9 RID: 11737 RVA: 0x0009AA55 File Offset: 0x00099A55
		internal URLString(string url, bool parsed, bool doDeferredParsing)
		{
			this.m_port = -1;
			this.m_userpass = "";
			this.DoFastChecks(url);
			this.m_urlOriginal = url;
			this.m_parsedOriginal = parsed;
			this.m_parseDeferred = true;
			if (doDeferredParsing)
			{
				this.DoDeferredParse();
			}
		}

		// Token: 0x06002DDA RID: 11738 RVA: 0x0009AA94 File Offset: 0x00099A94
		private string UnescapeURL(string url)
		{
			StringBuilder stringBuilder = new StringBuilder(url.Length);
			int num = 0;
			int num2 = -1;
			int num3 = -1;
			num2 = url.IndexOf('[', num);
			if (num2 != -1)
			{
				num3 = url.IndexOf(']', num2);
			}
			for (;;)
			{
				int num4 = url.IndexOf('%', num);
				if (num4 == -1)
				{
					break;
				}
				if (num4 > num2 && num4 < num3)
				{
					stringBuilder = stringBuilder.Append(url, num, num3 - num + 1);
					num = num3 + 1;
				}
				else
				{
					if (url.Length - num4 < 2)
					{
						goto Block_5;
					}
					if (url[num4 + 1] == 'u' || url[num4 + 1] == 'U')
					{
						if (url.Length - num4 < 6)
						{
							goto Block_7;
						}
						try
						{
							char c = (char)((Hex.ConvertHexDigit(url[num4 + 2]) << 12) | (Hex.ConvertHexDigit(url[num4 + 3]) << 8) | (Hex.ConvertHexDigit(url[num4 + 4]) << 4) | Hex.ConvertHexDigit(url[num4 + 5]));
							stringBuilder = stringBuilder.Append(url, num, num4 - num);
							stringBuilder = stringBuilder.Append(c);
						}
						catch (ArgumentException)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_InvalidUrl"));
						}
						num = num4 + 6;
					}
					else
					{
						if (url.Length - num4 < 3)
						{
							goto Block_9;
						}
						try
						{
							char c2 = (char)((Hex.ConvertHexDigit(url[num4 + 1]) << 4) | Hex.ConvertHexDigit(url[num4 + 2]));
							stringBuilder = stringBuilder.Append(url, num, num4 - num);
							stringBuilder = stringBuilder.Append(c2);
						}
						catch (ArgumentException)
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_InvalidUrl"));
						}
						num = num4 + 3;
					}
				}
			}
			stringBuilder = stringBuilder.Append(url, num, url.Length - num);
			return stringBuilder.ToString();
			Block_5:
			throw new ArgumentException(Environment.GetResourceString("Argument_InvalidUrl"));
			Block_7:
			throw new ArgumentException(Environment.GetResourceString("Argument_InvalidUrl"));
			Block_9:
			throw new ArgumentException(Environment.GetResourceString("Argument_InvalidUrl"));
		}

		// Token: 0x06002DDB RID: 11739 RVA: 0x0009AC6C File Offset: 0x00099C6C
		private string ParseProtocol(string url)
		{
			int num = url.IndexOf(':');
			if (num == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidUrl"));
			}
			string text;
			if (num == -1)
			{
				this.m_protocol = "file";
				text = url;
			}
			else
			{
				if (url.Length <= num + 1)
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidUrl"));
				}
				if (num == "file".Length && string.Compare(url, 0, "file", 0, num, StringComparison.OrdinalIgnoreCase) == 0)
				{
					this.m_protocol = "file";
					text = url.Substring(num + 1);
				}
				else if (url[num + 1] != '\\')
				{
					if (url.Length <= num + 2 || url[num + 1] != '/' || url[num + 2] != '/')
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_InvalidUrl"));
					}
					this.m_protocol = url.Substring(0, num);
					for (int i = 0; i < this.m_protocol.Length; i++)
					{
						char c = this.m_protocol[i];
						if ((c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && (c < '0' || c > '9') && c != '+' && c != '.' && c != '-')
						{
							throw new ArgumentException(Environment.GetResourceString("Argument_InvalidUrl"));
						}
					}
					text = url.Substring(num + 3);
				}
				else
				{
					this.m_protocol = "file";
					text = url;
				}
			}
			return text;
		}

		// Token: 0x06002DDC RID: 11740 RVA: 0x0009ADD8 File Offset: 0x00099DD8
		private string ParsePort(string url)
		{
			char[] array = new char[] { ':', '/' };
			int num = 0;
			int num2 = url.IndexOf('@');
			if (num2 != -1 && url.IndexOf('/', 0, num2) == -1)
			{
				this.m_userpass = url.Substring(0, num2);
				num = num2 + 1;
			}
			int num3 = -1;
			int num4 = url.IndexOf('[', num);
			if (num4 != -1)
			{
				num3 = url.IndexOf(']', num4);
			}
			int num5;
			if (num3 != -1)
			{
				num5 = url.IndexOfAny(array, num3);
			}
			else
			{
				num5 = url.IndexOfAny(array, num);
			}
			string text;
			if (num5 != -1 && url[num5] == ':')
			{
				if (url[num5 + 1] < '0' || url[num5 + 1] > '9')
				{
					throw new ArgumentException(Environment.GetResourceString("Argument_InvalidUrl"));
				}
				int num6 = url.IndexOf('/', num);
				if (num6 == -1)
				{
					this.m_port = int.Parse(url.Substring(num5 + 1), CultureInfo.InvariantCulture);
					if (this.m_port < 0)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_InvalidUrl"));
					}
					text = url.Substring(num, num5 - num);
				}
				else
				{
					if (num6 <= num5)
					{
						throw new ArgumentException(Environment.GetResourceString("Argument_InvalidUrl"));
					}
					this.m_port = int.Parse(url.Substring(num5 + 1, num6 - num5 - 1), CultureInfo.InvariantCulture);
					text = url.Substring(num, num5 - num) + url.Substring(num6);
				}
			}
			else
			{
				text = url.Substring(num);
			}
			return text;
		}

		// Token: 0x06002DDD RID: 11741 RVA: 0x0009AF64 File Offset: 0x00099F64
		internal static string PreProcessForExtendedPathRemoval(string url, bool isFileUrl)
		{
			StringBuilder stringBuilder = new StringBuilder(url);
			int num = 0;
			int num2 = 0;
			if (url.Length - num >= 4 && (string.Compare(url, num, "//?/", 0, 4, StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(url, num, "//./", 0, 4, StringComparison.OrdinalIgnoreCase) == 0))
			{
				stringBuilder.Remove(num2, 4);
				num += 4;
			}
			else
			{
				if (isFileUrl)
				{
					while (url[num] == '/')
					{
						num++;
						num2++;
					}
				}
				if (url.Length - num >= 4 && (string.Compare(url, num, "\\\\?\\", 0, 4, StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(url, num, "\\\\?/", 0, 4, StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(url, num, "\\\\.\\", 0, 4, StringComparison.OrdinalIgnoreCase) == 0 || string.Compare(url, num, "\\\\./", 0, 4, StringComparison.OrdinalIgnoreCase) == 0))
				{
					stringBuilder.Remove(num2, 4);
					num += 4;
				}
			}
			if (isFileUrl)
			{
				stringBuilder.Replace('\\', '/');
				int num3 = 0;
				while (num3 < stringBuilder.Length && stringBuilder[num3] == '/')
				{
					num3++;
				}
				stringBuilder.Remove(0, num3);
			}
			if (stringBuilder.Length >= 260)
			{
				throw new PathTooLongException(Environment.GetResourceString("IO.PathTooLong"));
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06002DDE RID: 11742 RVA: 0x0009B082 File Offset: 0x0009A082
		private string PreProcessURL(string url, bool isFileURL)
		{
			if (isFileURL)
			{
				url = URLString.PreProcessForExtendedPathRemoval(url, true);
			}
			else
			{
				url = url.Replace('\\', '/');
			}
			return url;
		}

		// Token: 0x06002DDF RID: 11743 RVA: 0x0009B0A0 File Offset: 0x0009A0A0
		private void ParseFileURL(string url)
		{
			int num = url.IndexOf('/');
			if (num != -1 && ((num == 2 && url[num - 1] != ':' && url[num - 1] != '|') || num != 2) && num != url.Length - 1)
			{
				int num2 = url.IndexOf('/', num + 1);
				if (num2 != -1)
				{
					num = num2;
				}
				else
				{
					num = -1;
				}
			}
			string text;
			if (num == -1)
			{
				text = url;
			}
			else
			{
				text = url.Substring(0, num);
			}
			if (text.Length == 0)
			{
				throw new ArgumentException(Environment.GetResourceString("Argument_InvalidUrl"));
			}
			bool flag;
			int i;
			if (text[0] == '\\' && text[1] == '\\')
			{
				flag = true;
				i = 2;
			}
			else
			{
				i = 0;
				flag = false;
			}
			bool flag2 = true;
			while (i < text.Length)
			{
				char c = text[i];
				if ((c < 'A' || c > 'Z') && (c < 'a' || c > 'z') && (c < '0' || c > '9') && c != '-' && c != '/' && c != ':' && c != '|' && c != '.' && c != '*' && c != '$' && (!flag || c != ' '))
				{
					flag2 = false;
					break;
				}
				i++;
			}
			if (flag2)
			{
				text = string.SmallCharToUpper(text);
			}
			else
			{
				text = text.ToUpper(CultureInfo.InvariantCulture);
			}
			this.m_localSite = new LocalSiteString(text);
			if (num == -1)
			{
				if (text[text.Length - 1] == '*')
				{
					this.m_directory = new DirectoryString("*", false);
				}
				else
				{
					this.m_directory = new DirectoryString();
				}
			}
			else
			{
				string text2 = url.Substring(num + 1);
				if (text2.Length == 0)
				{
					this.m_directory = new DirectoryString();
				}
				else
				{
					this.m_directory = new DirectoryString(text2, true);
				}
			}
			this.m_siteString = null;
		}

		// Token: 0x06002DE0 RID: 11744 RVA: 0x0009B25C File Offset: 0x0009A25C
		private void ParseNonFileURL(string url)
		{
			int num = url.IndexOf('/');
			if (num == -1)
			{
				this.m_localSite = null;
				this.m_siteString = new SiteString(url);
				this.m_directory = new DirectoryString();
				return;
			}
			string text = url.Substring(0, num);
			this.m_localSite = null;
			this.m_siteString = new SiteString(text);
			string text2 = url.Substring(num + 1);
			if (text2.Length == 0)
			{
				this.m_directory = new DirectoryString();
				return;
			}
			this.m_directory = new DirectoryString(text2, false);
		}

		// Token: 0x06002DE1 RID: 11745 RVA: 0x0009B2DE File Offset: 0x0009A2DE
		private void DoFastChecks(string url)
		{
			if (url == null)
			{
				throw new ArgumentNullException("url");
			}
			if (url.Length == 0)
			{
				throw new FormatException(Environment.GetResourceString("Format_StringZeroLength"));
			}
		}

		// Token: 0x06002DE2 RID: 11746 RVA: 0x0009B308 File Offset: 0x0009A308
		private void ParseString(string url, bool parsed)
		{
			if (!parsed)
			{
				url = this.UnescapeURL(url);
			}
			string text = this.ParseProtocol(url);
			bool flag = string.Compare(this.m_protocol, "file", StringComparison.OrdinalIgnoreCase) == 0;
			text = this.PreProcessURL(text, flag);
			if (flag)
			{
				this.ParseFileURL(text);
				return;
			}
			text = this.ParsePort(text);
			this.ParseNonFileURL(text);
		}

		// Token: 0x17000835 RID: 2101
		// (get) Token: 0x06002DE3 RID: 11747 RVA: 0x0009B361 File Offset: 0x0009A361
		public string Scheme
		{
			get
			{
				this.DoDeferredParse();
				return this.m_protocol;
			}
		}

		// Token: 0x17000836 RID: 2102
		// (get) Token: 0x06002DE4 RID: 11748 RVA: 0x0009B36F File Offset: 0x0009A36F
		public string Host
		{
			get
			{
				this.DoDeferredParse();
				if (this.m_siteString != null)
				{
					return this.m_siteString.ToString();
				}
				return this.m_localSite.ToString();
			}
		}

		// Token: 0x17000837 RID: 2103
		// (get) Token: 0x06002DE5 RID: 11749 RVA: 0x0009B396 File Offset: 0x0009A396
		public string Port
		{
			get
			{
				this.DoDeferredParse();
				if (this.m_port == -1)
				{
					return null;
				}
				return this.m_port.ToString(CultureInfo.InvariantCulture);
			}
		}

		// Token: 0x17000838 RID: 2104
		// (get) Token: 0x06002DE6 RID: 11750 RVA: 0x0009B3B9 File Offset: 0x0009A3B9
		public string Directory
		{
			get
			{
				this.DoDeferredParse();
				return this.m_directory.ToString();
			}
		}

		// Token: 0x06002DE7 RID: 11751 RVA: 0x0009B3CC File Offset: 0x0009A3CC
		public string GetFileName()
		{
			this.DoDeferredParse();
			if (string.Compare(this.m_protocol, "file", StringComparison.OrdinalIgnoreCase) != 0)
			{
				return null;
			}
			string text = this.Directory.Replace('/', '\\');
			string text2 = this.Host.Replace('/', '\\');
			int num = text2.IndexOf('\\');
			if (num == -1)
			{
				if (text2.Length != 2 || (text2[1] != ':' && text2[1] != '|'))
				{
					text2 = "\\\\" + text2;
				}
			}
			else if (num != 2 || (num == 2 && text2[1] != ':' && text2[1] != '|'))
			{
				text2 = "\\\\" + text2;
			}
			return text2 + "\\" + text;
		}

		// Token: 0x06002DE8 RID: 11752 RVA: 0x0009B488 File Offset: 0x0009A488
		public string GetDirectoryName()
		{
			this.DoDeferredParse();
			if (string.Compare(this.m_protocol, "file", StringComparison.OrdinalIgnoreCase) != 0)
			{
				return null;
			}
			string text = this.Directory.Replace('/', '\\');
			int num = 0;
			for (int i = text.Length; i > 0; i--)
			{
				if (text[i - 1] == '\\')
				{
					num = i;
					break;
				}
			}
			string text2 = this.Host.Replace('/', '\\');
			int num2 = text2.IndexOf('\\');
			if (num2 == -1)
			{
				if (text2.Length != 2 || (text2[1] != ':' && text2[1] != '|'))
				{
					text2 = "\\\\" + text2;
				}
			}
			else if (num2 > 2 || (num2 == 2 && text2[1] != ':' && text2[1] != '|'))
			{
				text2 = "\\\\" + text2;
			}
			text2 += "\\";
			if (num > 0)
			{
				text2 += text.Substring(0, num);
			}
			return text2;
		}

		// Token: 0x06002DE9 RID: 11753 RVA: 0x0009B57C File Offset: 0x0009A57C
		public override SiteString Copy()
		{
			return new URLString(this.m_urlOriginal, this.m_parsedOriginal);
		}

		// Token: 0x06002DEA RID: 11754 RVA: 0x0009B590 File Offset: 0x0009A590
		public override bool IsSubsetOf(SiteString site)
		{
			if (site == null)
			{
				return false;
			}
			URLString urlstring = site as URLString;
			if (urlstring == null)
			{
				return false;
			}
			this.DoDeferredParse();
			urlstring.DoDeferredParse();
			URLString urlstring2 = this.SpecialNormalizeUrl();
			URLString urlstring3 = urlstring.SpecialNormalizeUrl();
			if (string.Compare(urlstring2.m_protocol, urlstring3.m_protocol, StringComparison.OrdinalIgnoreCase) != 0 || !urlstring2.m_directory.IsSubsetOf(urlstring3.m_directory))
			{
				return false;
			}
			if (urlstring2.m_localSite != null)
			{
				return urlstring2.m_localSite.IsSubsetOf(urlstring3.m_localSite);
			}
			return urlstring2.m_port == urlstring3.m_port && urlstring3.m_siteString != null && urlstring2.m_siteString.IsSubsetOf(urlstring3.m_siteString);
		}

		// Token: 0x06002DEB RID: 11755 RVA: 0x0009B636 File Offset: 0x0009A636
		public override string ToString()
		{
			return this.m_urlOriginal;
		}

		// Token: 0x06002DEC RID: 11756 RVA: 0x0009B63E File Offset: 0x0009A63E
		public override bool Equals(object o)
		{
			this.DoDeferredParse();
			return o != null && o is URLString && this.Equals((URLString)o);
		}

		// Token: 0x06002DED RID: 11757 RVA: 0x0009B660 File Offset: 0x0009A660
		public override int GetHashCode()
		{
			this.DoDeferredParse();
			TextInfo textInfo = CultureInfo.InvariantCulture.TextInfo;
			int num = 0;
			if (this.m_protocol != null)
			{
				num = textInfo.GetCaseInsensitiveHashCode(this.m_protocol);
			}
			if (this.m_localSite != null)
			{
				num ^= this.m_localSite.GetHashCode();
			}
			else
			{
				num ^= this.m_siteString.GetHashCode();
			}
			return num ^ this.m_directory.GetHashCode();
		}

		// Token: 0x06002DEE RID: 11758 RVA: 0x0009B6CA File Offset: 0x0009A6CA
		public bool Equals(URLString url)
		{
			return URLString.CompareUrls(this, url);
		}

		// Token: 0x06002DEF RID: 11759 RVA: 0x0009B6D4 File Offset: 0x0009A6D4
		public static bool CompareUrls(URLString url1, URLString url2)
		{
			if (url1 == null && url2 == null)
			{
				return true;
			}
			if (url1 == null || url2 == null)
			{
				return false;
			}
			url1.DoDeferredParse();
			url2.DoDeferredParse();
			URLString urlstring = url1.SpecialNormalizeUrl();
			URLString urlstring2 = url2.SpecialNormalizeUrl();
			if (string.Compare(urlstring.m_protocol, urlstring2.m_protocol, StringComparison.OrdinalIgnoreCase) != 0)
			{
				return false;
			}
			if (string.Compare(urlstring.m_protocol, "file", StringComparison.OrdinalIgnoreCase) == 0)
			{
				if (!urlstring.m_localSite.IsSubsetOf(urlstring2.m_localSite) || !urlstring2.m_localSite.IsSubsetOf(urlstring.m_localSite))
				{
					return false;
				}
			}
			else
			{
				if (string.Compare(urlstring.m_userpass, urlstring2.m_userpass, StringComparison.Ordinal) != 0)
				{
					return false;
				}
				if (!urlstring.m_siteString.IsSubsetOf(urlstring2.m_siteString) || !urlstring2.m_siteString.IsSubsetOf(urlstring.m_siteString))
				{
					return false;
				}
				if (url1.m_port != url2.m_port)
				{
					return false;
				}
			}
			return urlstring.m_directory.IsSubsetOf(urlstring2.m_directory) && urlstring2.m_directory.IsSubsetOf(urlstring.m_directory);
		}

		// Token: 0x06002DF0 RID: 11760 RVA: 0x0009B7D4 File Offset: 0x0009A7D4
		internal string NormalizeUrl()
		{
			StringBuilder stringBuilder = new StringBuilder();
			this.DoDeferredParse();
			if (string.Compare(this.m_protocol, "file", StringComparison.OrdinalIgnoreCase) == 0)
			{
				stringBuilder = stringBuilder.AppendFormat("FILE:///{0}/{1}", this.m_localSite.ToString(), this.m_directory.ToString());
			}
			else
			{
				stringBuilder = stringBuilder.AppendFormat("{0}://{1}{2}", this.m_protocol, this.m_userpass, this.m_siteString.ToString());
				if (this.m_port != -1)
				{
					stringBuilder = stringBuilder.AppendFormat("{0}", this.m_port);
				}
				stringBuilder = stringBuilder.AppendFormat("/{0}", this.m_directory.ToString());
			}
			return stringBuilder.ToString().ToUpper(CultureInfo.InvariantCulture);
		}

		// Token: 0x06002DF1 RID: 11761 RVA: 0x0009B890 File Offset: 0x0009A890
		internal URLString SpecialNormalizeUrl()
		{
			this.DoDeferredParse();
			if (string.Compare(this.m_protocol, "file", StringComparison.OrdinalIgnoreCase) != 0)
			{
				return this;
			}
			string text = this.m_localSite.ToString();
			if (text.Length != 2 || (text[1] != '|' && text[1] != ':'))
			{
				return this;
			}
			string text2 = URLString._GetDeviceName(text);
			if (text2 == null)
			{
				return this;
			}
			if (text2.IndexOf("://", StringComparison.Ordinal) != -1)
			{
				URLString urlstring = new URLString(text2 + "/" + this.m_directory.ToString());
				urlstring.DoDeferredParse();
				return urlstring;
			}
			URLString urlstring2 = new URLString("file://" + text2 + "/" + this.m_directory.ToString());
			urlstring2.DoDeferredParse();
			return urlstring2;
		}

		// Token: 0x06002DF2 RID: 11762
		[MethodImpl(MethodImplOptions.InternalCall)]
		private static extern string _GetDeviceName(string driveLetter);

		// Token: 0x04001769 RID: 5993
		private const string m_defaultProtocol = "file";

		// Token: 0x0400176A RID: 5994
		private string m_protocol;

		// Token: 0x0400176B RID: 5995
		[OptionalField(VersionAdded = 2)]
		private string m_userpass;

		// Token: 0x0400176C RID: 5996
		private SiteString m_siteString;

		// Token: 0x0400176D RID: 5997
		private int m_port;

		// Token: 0x0400176E RID: 5998
		private LocalSiteString m_localSite;

		// Token: 0x0400176F RID: 5999
		private DirectoryString m_directory;

		// Token: 0x04001770 RID: 6000
		[OptionalField(VersionAdded = 2)]
		private bool m_parseDeferred;

		// Token: 0x04001771 RID: 6001
		[OptionalField(VersionAdded = 2)]
		private string m_urlOriginal;

		// Token: 0x04001772 RID: 6002
		[OptionalField(VersionAdded = 2)]
		private bool m_parsedOriginal;

		// Token: 0x04001773 RID: 6003
		private string m_fullurl;
	}
}
