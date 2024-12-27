using System;
using System.Collections;
using System.Globalization;
using System.Net.NetworkInformation;
using System.Threading;

namespace System.Net
{
	// Token: 0x02000395 RID: 917
	[Serializable]
	public class CookieContainer
	{
		// Token: 0x06001C95 RID: 7317 RVA: 0x0006C478 File Offset: 0x0006B478
		public CookieContainer()
		{
			string domainName = IPGlobalProperties.InternalGetIPGlobalProperties().DomainName;
			if (domainName != null && domainName.Length > 1)
			{
				this.m_fqdnMyDomain = '.' + domainName;
			}
		}

		// Token: 0x06001C96 RID: 7318 RVA: 0x0006C4E9 File Offset: 0x0006B4E9
		public CookieContainer(int capacity)
			: this()
		{
			if (capacity <= 0)
			{
				throw new ArgumentException(SR.GetString("net_toosmall"), "Capacity");
			}
			this.m_maxCookies = capacity;
		}

		// Token: 0x06001C97 RID: 7319 RVA: 0x0006C514 File Offset: 0x0006B514
		public CookieContainer(int capacity, int perDomainCapacity, int maxCookieSize)
			: this(capacity)
		{
			if (perDomainCapacity != 2147483647 && (perDomainCapacity <= 0 || perDomainCapacity > capacity))
			{
				throw new ArgumentOutOfRangeException("perDomainCapacity", SR.GetString("net_cookie_capacity_range", new object[] { "PerDomainCapacity", 0, capacity }));
			}
			this.m_maxCookiesPerDomain = perDomainCapacity;
			if (maxCookieSize <= 0)
			{
				throw new ArgumentException(SR.GetString("net_toosmall"), "MaxCookieSize");
			}
			this.m_maxCookieSize = maxCookieSize;
		}

		// Token: 0x17000593 RID: 1427
		// (get) Token: 0x06001C98 RID: 7320 RVA: 0x0006C596 File Offset: 0x0006B596
		// (set) Token: 0x06001C99 RID: 7321 RVA: 0x0006C5A0 File Offset: 0x0006B5A0
		public int Capacity
		{
			get
			{
				return this.m_maxCookies;
			}
			set
			{
				if (value <= 0 || (value < this.m_maxCookiesPerDomain && this.m_maxCookiesPerDomain != 2147483647))
				{
					throw new ArgumentOutOfRangeException("value", SR.GetString("net_cookie_capacity_range", new object[] { "Capacity", 0, this.m_maxCookiesPerDomain }));
				}
				if (value < this.m_maxCookies)
				{
					this.m_maxCookies = value;
					this.AgeCookies(null);
				}
				this.m_maxCookies = value;
			}
		}

		// Token: 0x17000594 RID: 1428
		// (get) Token: 0x06001C9A RID: 7322 RVA: 0x0006C622 File Offset: 0x0006B622
		public int Count
		{
			get
			{
				return this.m_count;
			}
		}

		// Token: 0x17000595 RID: 1429
		// (get) Token: 0x06001C9B RID: 7323 RVA: 0x0006C62A File Offset: 0x0006B62A
		// (set) Token: 0x06001C9C RID: 7324 RVA: 0x0006C632 File Offset: 0x0006B632
		public int MaxCookieSize
		{
			get
			{
				return this.m_maxCookieSize;
			}
			set
			{
				if (value <= 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_maxCookieSize = value;
			}
		}

		// Token: 0x17000596 RID: 1430
		// (get) Token: 0x06001C9D RID: 7325 RVA: 0x0006C64A File Offset: 0x0006B64A
		// (set) Token: 0x06001C9E RID: 7326 RVA: 0x0006C654 File Offset: 0x0006B654
		public int PerDomainCapacity
		{
			get
			{
				return this.m_maxCookiesPerDomain;
			}
			set
			{
				if (value <= 0 || (value > this.m_maxCookies && value != 2147483647))
				{
					throw new ArgumentOutOfRangeException("value");
				}
				if (value < this.m_maxCookiesPerDomain)
				{
					this.m_maxCookiesPerDomain = value;
					this.AgeCookies(null);
				}
				this.m_maxCookiesPerDomain = value;
			}
		}

		// Token: 0x06001C9F RID: 7327 RVA: 0x0006C6A0 File Offset: 0x0006B6A0
		public void Add(Cookie cookie)
		{
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			if (cookie.Domain.Length == 0)
			{
				throw new ArgumentException(SR.GetString("net_emptystringcall"), "cookie.Domain");
			}
			Cookie cookie2 = new Cookie(cookie.Name, cookie.Value);
			cookie2.Version = cookie.Version;
			string text = (cookie.Secure ? Uri.UriSchemeHttps : Uri.UriSchemeHttp) + Uri.SchemeDelimiter;
			if (cookie.Domain[0] == '.')
			{
				text += "0";
				cookie2.Domain = cookie.Domain;
			}
			text += cookie.Domain;
			if (cookie.PortList != null)
			{
				cookie2.Port = cookie.Port;
				text = text + ":" + cookie.PortList[0];
			}
			cookie2.Path = ((cookie.Path.Length == 0) ? "/" : cookie.Path);
			text += cookie.Path;
			Uri uri;
			if (!Uri.TryCreate(text, UriKind.Absolute, out uri))
			{
				throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Domain", cookie.Domain }));
			}
			cookie2.VerifySetDefaults(CookieVariant.Unknown, uri, this.IsLocal(uri.Host), this.m_fqdnMyDomain, true, true);
			this.Add(cookie2, true);
		}

		// Token: 0x06001CA0 RID: 7328 RVA: 0x0006C804 File Offset: 0x0006B804
		private void AddRemoveDomain(string key, PathList value)
		{
			lock (this)
			{
				if (value == null)
				{
					this.m_domainTable.Remove(key);
				}
				else
				{
					this.m_domainTable[key] = value;
				}
			}
		}

		// Token: 0x06001CA1 RID: 7329 RVA: 0x0006C850 File Offset: 0x0006B850
		internal void Add(Cookie cookie, bool throwOnError)
		{
			if (cookie.Value.Length <= this.m_maxCookieSize)
			{
				try
				{
					PathList pathList = (PathList)this.m_domainTable[cookie.DomainKey];
					if (pathList == null)
					{
						pathList = new PathList();
						this.AddRemoveDomain(cookie.DomainKey, pathList);
					}
					int cookiesCount = pathList.GetCookiesCount();
					CookieCollection cookieCollection = (CookieCollection)pathList[cookie.Path];
					if (cookieCollection == null)
					{
						cookieCollection = new CookieCollection();
						pathList[cookie.Path] = cookieCollection;
					}
					if (cookie.Expired)
					{
						lock (cookieCollection)
						{
							int num = cookieCollection.IndexOf(cookie);
							if (num != -1)
							{
								cookieCollection.RemoveAt(num);
								this.m_count--;
							}
							goto IL_0142;
						}
					}
					if (cookiesCount < this.m_maxCookiesPerDomain || this.AgeCookies(cookie.DomainKey))
					{
						if (this.m_count < this.m_maxCookies || this.AgeCookies(null))
						{
							lock (cookieCollection)
							{
								this.m_count += cookieCollection.InternalAdd(cookie, true);
							}
						}
					}
					IL_0142:;
				}
				catch (Exception ex)
				{
					if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
					{
						throw;
					}
					if (throwOnError)
					{
						throw new CookieException(SR.GetString("net_container_add_cookie"), ex);
					}
				}
				catch
				{
					if (throwOnError)
					{
						throw new CookieException(SR.GetString("net_container_add_cookie"), new Exception(SR.GetString("net_nonClsCompliantException")));
					}
				}
				return;
			}
			if (throwOnError)
			{
				throw new CookieException(SR.GetString("net_cookie_size", new object[]
				{
					cookie.ToString(),
					this.m_maxCookieSize
				}));
			}
		}

		// Token: 0x06001CA2 RID: 7330 RVA: 0x0006CA30 File Offset: 0x0006BA30
		private bool AgeCookies(string domain)
		{
			if (this.m_maxCookies == 0 || this.m_maxCookiesPerDomain == 0)
			{
				this.m_domainTable = new Hashtable();
				this.m_count = 0;
				return false;
			}
			int num = 0;
			DateTime dateTime = DateTime.MaxValue;
			CookieCollection cookieCollection = null;
			int num2 = 0;
			int num3 = 0;
			float num4 = 1f;
			if (this.m_count > this.m_maxCookies)
			{
				num4 = (float)this.m_maxCookies / (float)this.m_count;
			}
			foreach (object obj in this.m_domainTable)
			{
				DictionaryEntry dictionaryEntry = (DictionaryEntry)obj;
				PathList pathList;
				if (domain == null)
				{
					string text = (string)dictionaryEntry.Key;
					pathList = (PathList)dictionaryEntry.Value;
				}
				else
				{
					pathList = (PathList)this.m_domainTable[domain];
				}
				num2 = 0;
				foreach (object obj2 in pathList.Values)
				{
					CookieCollection cookieCollection2 = (CookieCollection)obj2;
					num3 = this.ExpireCollection(cookieCollection2);
					num += num3;
					this.m_count -= num3;
					num2 += cookieCollection2.Count;
					DateTime dateTime2;
					if (cookieCollection2.Count > 0 && (dateTime2 = cookieCollection2.TimeStamp(CookieCollection.Stamp.Check)) < dateTime)
					{
						cookieCollection = cookieCollection2;
						dateTime = dateTime2;
					}
				}
				int num5 = Math.Min((int)((float)num2 * num4), Math.Min(this.m_maxCookiesPerDomain, this.m_maxCookies) - 1);
				if (num2 > num5)
				{
					Array array = Array.CreateInstance(typeof(CookieCollection), pathList.Count);
					Array array2 = Array.CreateInstance(typeof(DateTime), pathList.Count);
					foreach (object obj3 in pathList.Values)
					{
						CookieCollection cookieCollection3 = (CookieCollection)obj3;
						array2.SetValue(cookieCollection3.TimeStamp(CookieCollection.Stamp.Check), num3);
						array.SetValue(cookieCollection3, num3);
						num3++;
					}
					Array.Sort(array2, array);
					num3 = 0;
					for (int i = 0; i < pathList.Count; i++)
					{
						CookieCollection cookieCollection4 = (CookieCollection)array.GetValue(i);
						lock (cookieCollection4)
						{
							while (num2 > num5 && cookieCollection4.Count > 0)
							{
								cookieCollection4.RemoveAt(0);
								num2--;
								this.m_count--;
								num++;
							}
						}
						if (num2 <= num5)
						{
							break;
						}
					}
					if (num2 > num5 && domain != null)
					{
						return false;
					}
				}
				if (domain != null)
				{
					return true;
				}
			}
			if (num != 0)
			{
				return true;
			}
			if (dateTime == DateTime.MaxValue)
			{
				return false;
			}
			lock (cookieCollection)
			{
				while (this.m_count >= this.m_maxCookies && cookieCollection.Count > 0)
				{
					cookieCollection.RemoveAt(0);
					this.m_count--;
				}
			}
			return true;
		}

		// Token: 0x06001CA3 RID: 7331 RVA: 0x0006CDCC File Offset: 0x0006BDCC
		private int ExpireCollection(CookieCollection cc)
		{
			int count = cc.Count;
			int i = count - 1;
			DateTime now = DateTime.Now;
			lock (cc)
			{
				while (i >= 0)
				{
					Cookie cookie = cc[i];
					if (cookie.Expires <= now && cookie.Expires != DateTime.MinValue)
					{
						cc.RemoveAt(i);
					}
					i--;
				}
			}
			return count - cc.Count;
		}

		// Token: 0x06001CA4 RID: 7332 RVA: 0x0006CE50 File Offset: 0x0006BE50
		public void Add(CookieCollection cookies)
		{
			if (cookies == null)
			{
				throw new ArgumentNullException("cookies");
			}
			foreach (object obj in cookies)
			{
				Cookie cookie = (Cookie)obj;
				this.Add(cookie);
			}
		}

		// Token: 0x06001CA5 RID: 7333 RVA: 0x0006CEB4 File Offset: 0x0006BEB4
		internal bool IsLocal(string host)
		{
			int num = host.IndexOf('.');
			if (num == -1)
			{
				return true;
			}
			if (host == "127.0.0.1")
			{
				return true;
			}
			if (string.Compare(this.m_fqdnMyDomain, 0, host, num, this.m_fqdnMyDomain.Length, StringComparison.OrdinalIgnoreCase) == 0)
			{
				return true;
			}
			string[] array = host.Split(new char[] { '.' });
			if (array != null && array.Length == 4 && array[0] == "127")
			{
				int i = 1;
				while (i < 4)
				{
					switch (array[i].Length)
					{
					case 1:
						break;
					case 2:
						goto IL_00B0;
					case 3:
						if (array[i][2] >= '0' && array[i][2] <= '9')
						{
							goto IL_00B0;
						}
						goto IL_00EC;
					default:
						goto IL_00EC;
					}
					IL_00CA:
					if (array[i][0] >= '0' && array[i][0] <= '9')
					{
						i++;
						continue;
					}
					break;
					IL_00B0:
					if (array[i][1] >= '0' && array[i][1] <= '9')
					{
						goto IL_00CA;
					}
					break;
				}
				IL_00EC:
				if (i == 4)
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06001CA6 RID: 7334 RVA: 0x0006CFB4 File Offset: 0x0006BFB4
		public void Add(Uri uri, Cookie cookie)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (cookie == null)
			{
				throw new ArgumentNullException("cookie");
			}
			cookie.VerifySetDefaults(CookieVariant.Unknown, uri, this.IsLocal(uri.Host), this.m_fqdnMyDomain, true, true);
			this.Add(cookie, true);
		}

		// Token: 0x06001CA7 RID: 7335 RVA: 0x0006D008 File Offset: 0x0006C008
		public void Add(Uri uri, CookieCollection cookies)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (cookies == null)
			{
				throw new ArgumentNullException("cookies");
			}
			bool flag = this.IsLocal(uri.Host);
			foreach (object obj in cookies)
			{
				Cookie cookie = (Cookie)obj;
				cookie.VerifySetDefaults(CookieVariant.Unknown, uri, flag, this.m_fqdnMyDomain, true, true);
				this.Add(cookie, true);
			}
		}

		// Token: 0x06001CA8 RID: 7336 RVA: 0x0006D0A0 File Offset: 0x0006C0A0
		internal CookieCollection CookieCutter(Uri uri, string headerName, string setCookieHeader, bool isThrow)
		{
			CookieCollection cookieCollection = new CookieCollection();
			CookieVariant cookieVariant = CookieVariant.Unknown;
			if (headerName == null)
			{
				cookieVariant = CookieVariant.Rfc2109;
			}
			else
			{
				for (int i = 0; i < CookieContainer.HeaderInfo.Length; i++)
				{
					if (string.Compare(headerName, CookieContainer.HeaderInfo[i].Name, StringComparison.OrdinalIgnoreCase) == 0)
					{
						cookieVariant = CookieContainer.HeaderInfo[i].Variant;
					}
				}
			}
			bool flag = this.IsLocal(uri.Host);
			try
			{
				CookieParser cookieParser = new CookieParser(setCookieHeader);
				for (;;)
				{
					Cookie cookie = cookieParser.Get();
					if (cookie == null)
					{
						goto IL_00B0;
					}
					if (ValidationHelper.IsBlankString(cookie.Name))
					{
						if (isThrow)
						{
							break;
						}
					}
					else if (cookie.VerifySetDefaults(cookieVariant, uri, flag, this.m_fqdnMyDomain, true, isThrow))
					{
						cookieCollection.InternalAdd(cookie, true);
					}
				}
				throw new CookieException(SR.GetString("net_cookie_format"));
				IL_00B0:;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				if (isThrow)
				{
					throw new CookieException(SR.GetString("net_cookie_parse_header", new object[] { uri.AbsoluteUri }), ex);
				}
			}
			catch
			{
				if (isThrow)
				{
					throw new CookieException(SR.GetString("net_cookie_parse_header", new object[] { uri.AbsoluteUri }), new Exception(SR.GetString("net_nonClsCompliantException")));
				}
			}
			foreach (object obj in cookieCollection)
			{
				Cookie cookie2 = (Cookie)obj;
				this.Add(cookie2, isThrow);
			}
			return cookieCollection;
		}

		// Token: 0x06001CA9 RID: 7337 RVA: 0x0006D254 File Offset: 0x0006C254
		public CookieCollection GetCookies(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			return this.InternalGetCookies(uri);
		}

		// Token: 0x06001CAA RID: 7338 RVA: 0x0006D274 File Offset: 0x0006C274
		internal CookieCollection InternalGetCookies(Uri uri)
		{
			bool flag = uri.Scheme == Uri.UriSchemeHttps;
			int port = uri.Port;
			CookieCollection cookieCollection = new CookieCollection();
			ArrayList arrayList = new ArrayList();
			int num = 0;
			string host = uri.Host;
			int num2 = host.IndexOf('.');
			if (num2 == -1)
			{
				arrayList.Add(host);
				if (this.m_fqdnMyDomain != null && this.m_fqdnMyDomain.Length != 0)
				{
					arrayList.Add(host + this.m_fqdnMyDomain);
					arrayList.Add(this.m_fqdnMyDomain);
					num = 3;
				}
				else
				{
					num = 1;
				}
			}
			else
			{
				arrayList.Add(host);
				arrayList.Add(host.Substring(num2));
				num = 2;
				if (host.Length > 2)
				{
					int num3 = host.LastIndexOf('.', host.Length - 2);
					if (num3 > 0)
					{
						num3 = host.LastIndexOf('.', num3 - 1);
					}
					if (num3 != -1)
					{
						while (num2 < num3 && (num2 = host.IndexOf('.', num2 + 1)) != -1)
						{
							arrayList.Add(host.Substring(num2));
						}
					}
				}
			}
			foreach (object obj in arrayList)
			{
				string text = (string)obj;
				bool flag2 = false;
				bool flag3 = false;
				PathList pathList = (PathList)this.m_domainTable[text];
				num--;
				if (pathList != null)
				{
					foreach (object obj2 in pathList)
					{
						DictionaryEntry dictionaryEntry = (DictionaryEntry)obj2;
						string text2 = (string)dictionaryEntry.Key;
						if (uri.AbsolutePath.StartsWith(CookieParser.CheckQuoted(text2)))
						{
							flag2 = true;
							CookieCollection cookieCollection2 = (CookieCollection)dictionaryEntry.Value;
							cookieCollection2.TimeStamp(CookieCollection.Stamp.Set);
							this.MergeUpdateCollections(cookieCollection, cookieCollection2, port, flag, num < 0);
							if (text2 == "/")
							{
								flag3 = true;
							}
						}
						else if (flag2)
						{
							break;
						}
					}
					if (!flag3)
					{
						CookieCollection cookieCollection3 = (CookieCollection)pathList["/"];
						if (cookieCollection3 != null)
						{
							cookieCollection3.TimeStamp(CookieCollection.Stamp.Set);
							this.MergeUpdateCollections(cookieCollection, cookieCollection3, port, flag, num < 0);
						}
					}
					if (pathList.Count == 0)
					{
						this.AddRemoveDomain(text, null);
					}
				}
			}
			return cookieCollection;
		}

		// Token: 0x06001CAB RID: 7339 RVA: 0x0006D508 File Offset: 0x0006C508
		private void MergeUpdateCollections(CookieCollection destination, CookieCollection source, int port, bool isSecure, bool isPlainOnly)
		{
			lock (source)
			{
				for (int i = 0; i < source.Count; i++)
				{
					bool flag = false;
					Cookie cookie = source[i];
					if (cookie.Expired)
					{
						source.RemoveAt(i);
						this.m_count--;
						i--;
					}
					else
					{
						if (!isPlainOnly || cookie.Variant == CookieVariant.Plain)
						{
							if (cookie.PortList != null)
							{
								foreach (int num in cookie.PortList)
								{
									if (num == port)
									{
										flag = true;
										break;
									}
								}
							}
							else
							{
								flag = true;
							}
						}
						if (cookie.Secure && !isSecure)
						{
							flag = false;
						}
						if (flag)
						{
							destination.InternalAdd(cookie, false);
						}
					}
				}
			}
		}

		// Token: 0x06001CAC RID: 7340 RVA: 0x0006D5D8 File Offset: 0x0006C5D8
		public string GetCookieHeader(Uri uri)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			string text;
			return this.GetCookieHeader(uri, out text);
		}

		// Token: 0x06001CAD RID: 7341 RVA: 0x0006D604 File Offset: 0x0006C604
		internal string GetCookieHeader(Uri uri, out string optCookie2)
		{
			CookieCollection cookieCollection = this.InternalGetCookies(uri);
			string text = string.Empty;
			string text2 = string.Empty;
			foreach (object obj in cookieCollection)
			{
				Cookie cookie = (Cookie)obj;
				text = text + text2 + cookie.ToString();
				text2 = "; ";
			}
			optCookie2 = (cookieCollection.IsOtherVersionSeen ? ("$Version=" + 1.ToString(NumberFormatInfo.InvariantInfo)) : string.Empty);
			return text;
		}

		// Token: 0x06001CAE RID: 7342 RVA: 0x0006D6AC File Offset: 0x0006C6AC
		public void SetCookies(Uri uri, string cookieHeader)
		{
			if (uri == null)
			{
				throw new ArgumentNullException("uri");
			}
			if (cookieHeader == null)
			{
				throw new ArgumentNullException("cookieHeader");
			}
			this.CookieCutter(uri, null, cookieHeader, true);
		}

		// Token: 0x04001D2C RID: 7468
		public const int DefaultCookieLimit = 300;

		// Token: 0x04001D2D RID: 7469
		public const int DefaultPerDomainCookieLimit = 20;

		// Token: 0x04001D2E RID: 7470
		public const int DefaultCookieLengthLimit = 4096;

		// Token: 0x04001D2F RID: 7471
		private static readonly HeaderVariantInfo[] HeaderInfo = new HeaderVariantInfo[]
		{
			new HeaderVariantInfo("Set-Cookie", CookieVariant.Rfc2109),
			new HeaderVariantInfo("Set-Cookie2", CookieVariant.Rfc2965)
		};

		// Token: 0x04001D30 RID: 7472
		private Hashtable m_domainTable = new Hashtable();

		// Token: 0x04001D31 RID: 7473
		private int m_maxCookieSize = 4096;

		// Token: 0x04001D32 RID: 7474
		private int m_maxCookies = 300;

		// Token: 0x04001D33 RID: 7475
		private int m_maxCookiesPerDomain = 20;

		// Token: 0x04001D34 RID: 7476
		private int m_count;

		// Token: 0x04001D35 RID: 7477
		private string m_fqdnMyDomain = string.Empty;
	}
}
