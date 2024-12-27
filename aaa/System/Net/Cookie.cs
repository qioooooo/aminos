using System;
using System.Collections;
using System.Globalization;
using System.Runtime.Serialization;

namespace System.Net
{
	// Token: 0x0200038B RID: 907
	[Serializable]
	public sealed class Cookie
	{
		// Token: 0x06001C30 RID: 7216 RVA: 0x0006A318 File Offset: 0x00069318
		public Cookie()
		{
		}

		// Token: 0x06001C31 RID: 7217 RVA: 0x0006A3AC File Offset: 0x000693AC
		public Cookie(string name, string value)
		{
			this.Name = name;
			this.m_value = value;
		}

		// Token: 0x06001C32 RID: 7218 RVA: 0x0006A44C File Offset: 0x0006944C
		public Cookie(string name, string value, string path)
			: this(name, value)
		{
			this.Path = path;
		}

		// Token: 0x06001C33 RID: 7219 RVA: 0x0006A45D File Offset: 0x0006945D
		public Cookie(string name, string value, string path, string domain)
			: this(name, value, path)
		{
			this.Domain = domain;
		}

		// Token: 0x1700056C RID: 1388
		// (get) Token: 0x06001C34 RID: 7220 RVA: 0x0006A470 File Offset: 0x00069470
		// (set) Token: 0x06001C35 RID: 7221 RVA: 0x0006A478 File Offset: 0x00069478
		public string Comment
		{
			get
			{
				return this.m_comment;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this.m_comment = value;
			}
		}

		// Token: 0x1700056D RID: 1389
		// (get) Token: 0x06001C36 RID: 7222 RVA: 0x0006A48B File Offset: 0x0006948B
		// (set) Token: 0x06001C37 RID: 7223 RVA: 0x0006A493 File Offset: 0x00069493
		public Uri CommentUri
		{
			get
			{
				return this.m_commentUri;
			}
			set
			{
				this.m_commentUri = value;
			}
		}

		// Token: 0x1700056E RID: 1390
		// (get) Token: 0x06001C38 RID: 7224 RVA: 0x0006A49C File Offset: 0x0006949C
		// (set) Token: 0x06001C39 RID: 7225 RVA: 0x0006A4A4 File Offset: 0x000694A4
		public bool HttpOnly
		{
			get
			{
				return this.m_httpOnly;
			}
			set
			{
				this.m_httpOnly = value;
			}
		}

		// Token: 0x1700056F RID: 1391
		// (get) Token: 0x06001C3A RID: 7226 RVA: 0x0006A4AD File Offset: 0x000694AD
		// (set) Token: 0x06001C3B RID: 7227 RVA: 0x0006A4B5 File Offset: 0x000694B5
		public bool Discard
		{
			get
			{
				return this.m_discard;
			}
			set
			{
				this.m_discard = value;
			}
		}

		// Token: 0x17000570 RID: 1392
		// (get) Token: 0x06001C3C RID: 7228 RVA: 0x0006A4BE File Offset: 0x000694BE
		// (set) Token: 0x06001C3D RID: 7229 RVA: 0x0006A4C6 File Offset: 0x000694C6
		public string Domain
		{
			get
			{
				return this.m_domain;
			}
			set
			{
				this.m_domain = ((value == null) ? string.Empty : value);
				this.m_domain_implicit = false;
				this.m_domainKey = string.Empty;
			}
		}

		// Token: 0x17000571 RID: 1393
		// (get) Token: 0x06001C3E RID: 7230 RVA: 0x0006A4EC File Offset: 0x000694EC
		private string _Domain
		{
			get
			{
				if (!this.Plain && !this.m_domain_implicit && this.m_domain.Length != 0)
				{
					return "$Domain=" + (this.IsQuotedDomain ? "\"" : string.Empty) + this.m_domain + (this.IsQuotedDomain ? "\"" : string.Empty);
				}
				return string.Empty;
			}
		}

		// Token: 0x17000572 RID: 1394
		// (get) Token: 0x06001C3F RID: 7231 RVA: 0x0006A554 File Offset: 0x00069554
		// (set) Token: 0x06001C40 RID: 7232 RVA: 0x0006A57A File Offset: 0x0006957A
		public bool Expired
		{
			get
			{
				return this.m_expires <= DateTime.Now && this.m_expires != DateTime.MinValue;
			}
			set
			{
				if (value)
				{
					this.m_expires = DateTime.Now;
				}
			}
		}

		// Token: 0x17000573 RID: 1395
		// (get) Token: 0x06001C41 RID: 7233 RVA: 0x0006A58A File Offset: 0x0006958A
		// (set) Token: 0x06001C42 RID: 7234 RVA: 0x0006A592 File Offset: 0x00069592
		public DateTime Expires
		{
			get
			{
				return this.m_expires;
			}
			set
			{
				this.m_expires = value;
			}
		}

		// Token: 0x17000574 RID: 1396
		// (get) Token: 0x06001C43 RID: 7235 RVA: 0x0006A59B File Offset: 0x0006959B
		// (set) Token: 0x06001C44 RID: 7236 RVA: 0x0006A5A4 File Offset: 0x000695A4
		public string Name
		{
			get
			{
				return this.m_name;
			}
			set
			{
				if (ValidationHelper.IsBlankString(value) || !this.InternalSetName(value))
				{
					throw new CookieException(SR.GetString("net_cookie_attribute", new object[]
					{
						"Name",
						(value == null) ? "<null>" : value
					}));
				}
			}
		}

		// Token: 0x06001C45 RID: 7237 RVA: 0x0006A5F0 File Offset: 0x000695F0
		internal bool InternalSetName(string value)
		{
			if (ValidationHelper.IsBlankString(value) || value[0] == '$' || value.IndexOfAny(Cookie.Reserved2Name) != -1)
			{
				this.m_name = string.Empty;
				return false;
			}
			this.m_name = value;
			return true;
		}

		// Token: 0x17000575 RID: 1397
		// (get) Token: 0x06001C46 RID: 7238 RVA: 0x0006A628 File Offset: 0x00069628
		// (set) Token: 0x06001C47 RID: 7239 RVA: 0x0006A630 File Offset: 0x00069630
		public string Path
		{
			get
			{
				return this.m_path;
			}
			set
			{
				this.m_path = ((value == null) ? string.Empty : value);
				this.m_path_implicit = false;
			}
		}

		// Token: 0x17000576 RID: 1398
		// (get) Token: 0x06001C48 RID: 7240 RVA: 0x0006A64A File Offset: 0x0006964A
		private string _Path
		{
			get
			{
				if (!this.Plain && !this.m_path_implicit && this.m_path.Length != 0)
				{
					return "$Path=" + this.m_path;
				}
				return string.Empty;
			}
		}

		// Token: 0x17000577 RID: 1399
		// (get) Token: 0x06001C49 RID: 7241 RVA: 0x0006A67F File Offset: 0x0006967F
		internal bool Plain
		{
			get
			{
				return this.Variant == CookieVariant.Plain;
			}
		}

		// Token: 0x06001C4A RID: 7242 RVA: 0x0006A68C File Offset: 0x0006968C
		internal bool VerifySetDefaults(CookieVariant variant, Uri uri, bool isLocalDomain, string localDomain, bool set_default, bool isThrow)
		{
			string host = uri.Host;
			int port = uri.Port;
			string absolutePath = uri.AbsolutePath;
			bool flag = true;
			if (set_default)
			{
				if (this.Version == 0)
				{
					variant = CookieVariant.Plain;
				}
				else if (this.Version == 1 && variant == CookieVariant.Unknown)
				{
					variant = CookieVariant.Rfc2109;
				}
				this.m_cookieVariant = variant;
			}
			if (this.m_name == null || this.m_name.Length == 0 || this.m_name[0] == '$' || this.m_name.IndexOfAny(Cookie.Reserved2Name) != -1)
			{
				if (isThrow)
				{
					throw new CookieException(SR.GetString("net_cookie_attribute", new object[]
					{
						"Name",
						(this.m_name == null) ? "<null>" : this.m_name
					}));
				}
				return false;
			}
			else if (this.m_value == null || ((this.m_value.Length <= 2 || this.m_value[0] != '"' || this.m_value[this.m_value.Length - 1] != '"') && this.m_value.IndexOfAny(Cookie.Reserved2Value) != -1))
			{
				if (isThrow)
				{
					throw new CookieException(SR.GetString("net_cookie_attribute", new object[]
					{
						"Value",
						(this.m_value == null) ? "<null>" : this.m_value
					}));
				}
				return false;
			}
			else if (this.Comment != null && (this.Comment.Length <= 2 || this.Comment[0] != '"' || this.Comment[this.Comment.Length - 1] != '"') && this.Comment.IndexOfAny(Cookie.Reserved2Value) != -1)
			{
				if (isThrow)
				{
					throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Comment", this.Comment }));
				}
				return false;
			}
			else
			{
				if (this.Path == null || (this.Path.Length > 2 && this.Path[0] == '"' && this.Path[this.Path.Length - 1] == '"') || this.Path.IndexOfAny(Cookie.Reserved2Value) == -1)
				{
					if (set_default && this.m_domain_implicit)
					{
						this.m_domain = host;
					}
					else
					{
						if (!this.m_domain_implicit)
						{
							string text = this.m_domain;
							if (!Cookie.DomainCharsTest(text))
							{
								if (isThrow)
								{
									throw new CookieException(SR.GetString("net_cookie_attribute", new object[]
									{
										"Domain",
										(text == null) ? "<null>" : text
									}));
								}
								return false;
							}
							else
							{
								if (text[0] != '.')
								{
									if (variant != CookieVariant.Rfc2965 && variant != CookieVariant.Plain)
									{
										if (isThrow)
										{
											throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Domain", this.m_domain }));
										}
										return false;
									}
									else
									{
										text = '.' + text;
									}
								}
								int num = host.IndexOf('.');
								bool flag2 = false;
								if (isLocalDomain && string.Compare(localDomain, text, StringComparison.OrdinalIgnoreCase) == 0)
								{
									flag = true;
								}
								else if (text.Length < 4 || (!(flag2 = string.Compare(text, ".local", StringComparison.OrdinalIgnoreCase) == 0) && text.IndexOf('.', 1, text.Length - 2) == -1))
								{
									flag = false;
								}
								else if (flag2 && isLocalDomain)
								{
									flag = true;
								}
								else if (flag2 && !isLocalDomain)
								{
									flag = false;
								}
								else if (variant == CookieVariant.Plain)
								{
									if ((host.Length + 1 != text.Length || string.Compare(host, 0, text, 1, host.Length, StringComparison.OrdinalIgnoreCase) != 0) && (host.Length <= text.Length || string.Compare(host, host.Length - text.Length, text, 0, text.Length, StringComparison.OrdinalIgnoreCase) != 0))
									{
										flag = false;
									}
								}
								else if (!flag2 && (num == -1 || text.Length != host.Length - num || string.Compare(host, num, text, 0, text.Length, StringComparison.OrdinalIgnoreCase) != 0))
								{
									flag = false;
								}
								if (flag)
								{
									if (flag2)
									{
										this.m_domainKey = localDomain.ToLower(CultureInfo.InvariantCulture);
									}
									else
									{
										this.m_domainKey = text.ToLower(CultureInfo.InvariantCulture);
									}
								}
							}
						}
						else if (string.Compare(host, this.m_domain, StringComparison.OrdinalIgnoreCase) != 0)
						{
							flag = false;
						}
						if (!flag)
						{
							if (isThrow)
							{
								throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Domain", this.m_domain }));
							}
							return false;
						}
					}
					if (set_default && this.m_path_implicit)
					{
						switch (this.m_cookieVariant)
						{
						case CookieVariant.Plain:
							this.m_path = absolutePath;
							goto IL_0553;
						case CookieVariant.Rfc2109:
							this.m_path = absolutePath.Substring(0, absolutePath.LastIndexOf('/'));
							goto IL_0553;
						}
						this.m_path = absolutePath.Substring(0, absolutePath.LastIndexOf('/') + 1);
					}
					else if (!absolutePath.StartsWith(CookieParser.CheckQuoted(this.m_path)))
					{
						if (isThrow)
						{
							throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Path", this.m_path }));
						}
						return false;
					}
					IL_0553:
					if (set_default && !this.m_port_implicit && this.m_port.Length == 0)
					{
						this.m_port_list = new int[] { port };
					}
					if (!this.m_port_implicit)
					{
						flag = false;
						foreach (int num2 in this.m_port_list)
						{
							if (num2 == port)
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							if (isThrow)
							{
								throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Port", this.m_port }));
							}
							return false;
						}
					}
					return true;
				}
				if (isThrow)
				{
					throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Path", this.Path }));
				}
				return false;
			}
		}

		// Token: 0x06001C4B RID: 7243 RVA: 0x0006AC88 File Offset: 0x00069C88
		private static bool DomainCharsTest(string name)
		{
			if (name == null || name.Length == 0)
			{
				return false;
			}
			foreach (char c in name)
			{
				if ((c < '0' || c > '9') && c != '.' && c != '-' && (c < 'a' || c > 'z') && (c < 'A' || c > 'Z') && c != '_')
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x17000578 RID: 1400
		// (get) Token: 0x06001C4C RID: 7244 RVA: 0x0006ACEB File Offset: 0x00069CEB
		// (set) Token: 0x06001C4D RID: 7245 RVA: 0x0006ACF4 File Offset: 0x00069CF4
		public string Port
		{
			get
			{
				return this.m_port;
			}
			set
			{
				this.m_port_implicit = false;
				if (value == null || value.Length == 0)
				{
					this.m_port = string.Empty;
					return;
				}
				this.m_port = value;
				if (value[0] != '"' || value[value.Length - 1] != '"')
				{
					throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Port", this.m_port }));
				}
				string[] array = value.Split(Cookie.PortSplitDelimiters);
				this.m_port_list = new int[array.Length];
				for (int i = 0; i < array.Length; i++)
				{
					this.m_port_list[i] = -1;
					if (array[i].Length != 0 && !int.TryParse(array[i], out this.m_port_list[i]))
					{
						throw new CookieException(SR.GetString("net_cookie_attribute", new object[] { "Port", this.m_port }));
					}
				}
				this.m_version = 1;
				this.m_cookieVariant = CookieVariant.Rfc2965;
			}
		}

		// Token: 0x17000579 RID: 1401
		// (get) Token: 0x06001C4E RID: 7246 RVA: 0x0006ADF4 File Offset: 0x00069DF4
		internal int[] PortList
		{
			get
			{
				return this.m_port_list;
			}
		}

		// Token: 0x1700057A RID: 1402
		// (get) Token: 0x06001C4F RID: 7247 RVA: 0x0006ADFC File Offset: 0x00069DFC
		private string _Port
		{
			get
			{
				if (!this.m_port_implicit)
				{
					return "$Port" + ((this.m_port.Length == 0) ? string.Empty : ("=" + this.m_port));
				}
				return string.Empty;
			}
		}

		// Token: 0x1700057B RID: 1403
		// (get) Token: 0x06001C50 RID: 7248 RVA: 0x0006AE3A File Offset: 0x00069E3A
		// (set) Token: 0x06001C51 RID: 7249 RVA: 0x0006AE42 File Offset: 0x00069E42
		public bool Secure
		{
			get
			{
				return this.m_secure;
			}
			set
			{
				this.m_secure = value;
			}
		}

		// Token: 0x1700057C RID: 1404
		// (get) Token: 0x06001C52 RID: 7250 RVA: 0x0006AE4B File Offset: 0x00069E4B
		public DateTime TimeStamp
		{
			get
			{
				return this.m_timeStamp;
			}
		}

		// Token: 0x1700057D RID: 1405
		// (get) Token: 0x06001C53 RID: 7251 RVA: 0x0006AE53 File Offset: 0x00069E53
		// (set) Token: 0x06001C54 RID: 7252 RVA: 0x0006AE5B File Offset: 0x00069E5B
		public string Value
		{
			get
			{
				return this.m_value;
			}
			set
			{
				this.m_value = ((value == null) ? string.Empty : value);
			}
		}

		// Token: 0x1700057E RID: 1406
		// (get) Token: 0x06001C55 RID: 7253 RVA: 0x0006AE6E File Offset: 0x00069E6E
		// (set) Token: 0x06001C56 RID: 7254 RVA: 0x0006AE76 File Offset: 0x00069E76
		internal CookieVariant Variant
		{
			get
			{
				return this.m_cookieVariant;
			}
			set
			{
				this.m_cookieVariant = value;
			}
		}

		// Token: 0x1700057F RID: 1407
		// (get) Token: 0x06001C57 RID: 7255 RVA: 0x0006AE7F File Offset: 0x00069E7F
		internal string DomainKey
		{
			get
			{
				if (!this.m_domain_implicit)
				{
					return this.m_domainKey;
				}
				return this.Domain;
			}
		}

		// Token: 0x17000580 RID: 1408
		// (get) Token: 0x06001C58 RID: 7256 RVA: 0x0006AE96 File Offset: 0x00069E96
		// (set) Token: 0x06001C59 RID: 7257 RVA: 0x0006AE9E File Offset: 0x00069E9E
		public int Version
		{
			get
			{
				return this.m_version;
			}
			set
			{
				if (value < 0)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_version = value;
				if (value > 0 && this.m_cookieVariant < CookieVariant.Rfc2109)
				{
					this.m_cookieVariant = CookieVariant.Rfc2109;
				}
			}
		}

		// Token: 0x17000581 RID: 1409
		// (get) Token: 0x06001C5A RID: 7258 RVA: 0x0006AECC File Offset: 0x00069ECC
		private string _Version
		{
			get
			{
				if (this.Version != 0)
				{
					return "$Version=" + (this.IsQuotedVersion ? "\"" : string.Empty) + this.m_version.ToString(NumberFormatInfo.InvariantInfo) + (this.IsQuotedVersion ? "\"" : string.Empty);
				}
				return string.Empty;
			}
		}

		// Token: 0x06001C5B RID: 7259 RVA: 0x0006AF29 File Offset: 0x00069F29
		internal static IComparer GetComparer()
		{
			return Cookie.staticComparer;
		}

		// Token: 0x06001C5C RID: 7260 RVA: 0x0006AF30 File Offset: 0x00069F30
		public override bool Equals(object comparand)
		{
			if (!(comparand is Cookie))
			{
				return false;
			}
			Cookie cookie = (Cookie)comparand;
			return string.Compare(this.Name, cookie.Name, StringComparison.OrdinalIgnoreCase) == 0 && string.Compare(this.Value, cookie.Value, StringComparison.Ordinal) == 0 && string.Compare(this.Path, cookie.Path, StringComparison.Ordinal) == 0 && string.Compare(this.Domain, cookie.Domain, StringComparison.OrdinalIgnoreCase) == 0 && this.Version == cookie.Version;
		}

		// Token: 0x06001C5D RID: 7261 RVA: 0x0006AFB0 File Offset: 0x00069FB0
		public override int GetHashCode()
		{
			return string.Concat(new object[] { this.Name, "=", this.Value, ";", this.Path, "; ", this.Domain, "; ", this.Version }).GetHashCode();
		}

		// Token: 0x06001C5E RID: 7262 RVA: 0x0006B024 File Offset: 0x0006A024
		public override string ToString()
		{
			string domain = this._Domain;
			string path = this._Path;
			string port = this._Port;
			string version = this._Version;
			string text = string.Concat(new string[]
			{
				(version.Length == 0) ? string.Empty : (version + "; "),
				this.Name,
				"=",
				this.Value,
				(path.Length == 0) ? string.Empty : ("; " + path),
				(domain.Length == 0) ? string.Empty : ("; " + domain),
				(port.Length == 0) ? string.Empty : ("; " + port)
			});
			if (text == "=")
			{
				return string.Empty;
			}
			return text;
		}

		// Token: 0x06001C5F RID: 7263 RVA: 0x0006B10C File Offset: 0x0006A10C
		internal string ToServerString()
		{
			string text = this.Name + "=" + this.Value;
			if (this.m_comment != null && this.m_comment.Length > 0)
			{
				text = text + "; Comment=" + this.m_comment;
			}
			if (this.m_commentUri != null)
			{
				text = text + "; CommentURL=\"" + this.m_commentUri.ToString() + "\"";
			}
			if (this.m_discard)
			{
				text += "; Discard";
			}
			if (!this.Plain && !this.m_domain_implicit && this.m_domain != null && this.m_domain.Length > 0)
			{
				text = text + "; Domain=" + this.m_domain;
			}
			int seconds = (this.Expires - DateTime.UtcNow).Seconds;
			if (seconds > 0)
			{
				text = text + "; Max-Age=" + seconds.ToString(NumberFormatInfo.InvariantInfo);
			}
			if (!this.Plain && !this.m_path_implicit && this.m_path != null && this.m_path.Length > 0)
			{
				text = text + "; Path=" + this.m_path;
			}
			if (!this.Plain && !this.m_port_implicit && this.m_port != null && this.m_port.Length > 0)
			{
				text = text + "; Port=" + this.m_port;
			}
			if (this.m_version > 0)
			{
				text = text + "; Version=" + this.m_version.ToString(NumberFormatInfo.InvariantInfo);
			}
			if (!(text == "="))
			{
				return text;
			}
			return null;
		}

		// Token: 0x04001CCE RID: 7374
		internal const int MaxSupportedVersion = 1;

		// Token: 0x04001CCF RID: 7375
		internal const string CommentAttributeName = "Comment";

		// Token: 0x04001CD0 RID: 7376
		internal const string CommentUrlAttributeName = "CommentURL";

		// Token: 0x04001CD1 RID: 7377
		internal const string DiscardAttributeName = "Discard";

		// Token: 0x04001CD2 RID: 7378
		internal const string DomainAttributeName = "Domain";

		// Token: 0x04001CD3 RID: 7379
		internal const string ExpiresAttributeName = "Expires";

		// Token: 0x04001CD4 RID: 7380
		internal const string MaxAgeAttributeName = "Max-Age";

		// Token: 0x04001CD5 RID: 7381
		internal const string PathAttributeName = "Path";

		// Token: 0x04001CD6 RID: 7382
		internal const string PortAttributeName = "Port";

		// Token: 0x04001CD7 RID: 7383
		internal const string SecureAttributeName = "Secure";

		// Token: 0x04001CD8 RID: 7384
		internal const string VersionAttributeName = "Version";

		// Token: 0x04001CD9 RID: 7385
		internal const string HttpOnlyAttributeName = "HttpOnly";

		// Token: 0x04001CDA RID: 7386
		internal const string SeparatorLiteral = "; ";

		// Token: 0x04001CDB RID: 7387
		internal const string EqualsLiteral = "=";

		// Token: 0x04001CDC RID: 7388
		internal const string QuotesLiteral = "\"";

		// Token: 0x04001CDD RID: 7389
		internal const string SpecialAttributeLiteral = "$";

		// Token: 0x04001CDE RID: 7390
		internal static readonly char[] PortSplitDelimiters = new char[] { ' ', ',', '"' };

		// Token: 0x04001CDF RID: 7391
		internal static readonly char[] Reserved2Name = new char[] { ' ', '\t', '\r', '\n', '=', ';', ',' };

		// Token: 0x04001CE0 RID: 7392
		internal static readonly char[] Reserved2Value = new char[] { ';', ',' };

		// Token: 0x04001CE1 RID: 7393
		private static Comparer staticComparer = new Comparer();

		// Token: 0x04001CE2 RID: 7394
		private string m_comment = string.Empty;

		// Token: 0x04001CE3 RID: 7395
		private Uri m_commentUri;

		// Token: 0x04001CE4 RID: 7396
		private CookieVariant m_cookieVariant = CookieVariant.Plain;

		// Token: 0x04001CE5 RID: 7397
		private bool m_discard;

		// Token: 0x04001CE6 RID: 7398
		private string m_domain = string.Empty;

		// Token: 0x04001CE7 RID: 7399
		private bool m_domain_implicit = true;

		// Token: 0x04001CE8 RID: 7400
		private DateTime m_expires = DateTime.MinValue;

		// Token: 0x04001CE9 RID: 7401
		private string m_name = string.Empty;

		// Token: 0x04001CEA RID: 7402
		private string m_path = string.Empty;

		// Token: 0x04001CEB RID: 7403
		private bool m_path_implicit = true;

		// Token: 0x04001CEC RID: 7404
		private string m_port = string.Empty;

		// Token: 0x04001CED RID: 7405
		private bool m_port_implicit = true;

		// Token: 0x04001CEE RID: 7406
		private int[] m_port_list;

		// Token: 0x04001CEF RID: 7407
		private bool m_secure;

		// Token: 0x04001CF0 RID: 7408
		[OptionalField]
		private bool m_httpOnly;

		// Token: 0x04001CF1 RID: 7409
		private DateTime m_timeStamp = DateTime.Now;

		// Token: 0x04001CF2 RID: 7410
		private string m_value = string.Empty;

		// Token: 0x04001CF3 RID: 7411
		private int m_version;

		// Token: 0x04001CF4 RID: 7412
		private string m_domainKey = string.Empty;

		// Token: 0x04001CF5 RID: 7413
		internal bool IsQuotedVersion;

		// Token: 0x04001CF6 RID: 7414
		internal bool IsQuotedDomain;
	}
}
