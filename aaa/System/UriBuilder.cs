using System;
using System.Globalization;
using System.Net;
using System.Text;
using System.Threading;

namespace System
{
	// Token: 0x0200035E RID: 862
	public class UriBuilder
	{
		// Token: 0x06001B6B RID: 7019 RVA: 0x00066D70 File Offset: 0x00065D70
		public UriBuilder()
		{
		}

		// Token: 0x06001B6C RID: 7020 RVA: 0x00066DEC File Offset: 0x00065DEC
		public UriBuilder(string uri)
		{
			Uri uri2 = new Uri(uri, UriKind.RelativeOrAbsolute);
			if (uri2.IsAbsoluteUri)
			{
				this.Init(uri2);
				return;
			}
			uri = Uri.UriSchemeHttp + Uri.SchemeDelimiter + uri;
			this.Init(new Uri(uri));
		}

		// Token: 0x06001B6D RID: 7021 RVA: 0x00066E9C File Offset: 0x00065E9C
		public UriBuilder(Uri uri)
		{
			this.Init(uri);
		}

		// Token: 0x06001B6E RID: 7022 RVA: 0x00066F1C File Offset: 0x00065F1C
		private void Init(Uri uri)
		{
			this.m_fragment = uri.Fragment;
			this.m_query = uri.Query;
			this.m_host = uri.Host;
			this.m_path = uri.AbsolutePath;
			this.m_port = uri.Port;
			this.m_scheme = uri.Scheme;
			this.m_schemeDelimiter = (uri.HasAuthority ? Uri.SchemeDelimiter : ":");
			string userInfo = uri.UserInfo;
			if (!ValidationHelper.IsBlankString(userInfo))
			{
				int num = userInfo.IndexOf(':');
				if (num != -1)
				{
					this.m_password = userInfo.Substring(num + 1);
					this.m_username = userInfo.Substring(0, num);
				}
				else
				{
					this.m_username = userInfo;
				}
			}
			this.SetFieldsFromUri(uri);
		}

		// Token: 0x06001B6F RID: 7023 RVA: 0x00066FD4 File Offset: 0x00065FD4
		public UriBuilder(string schemeName, string hostName)
		{
			this.Scheme = schemeName;
			this.Host = hostName;
		}

		// Token: 0x06001B70 RID: 7024 RVA: 0x0006705B File Offset: 0x0006605B
		public UriBuilder(string scheme, string host, int portNumber)
			: this(scheme, host)
		{
			this.Port = portNumber;
		}

		// Token: 0x06001B71 RID: 7025 RVA: 0x0006706C File Offset: 0x0006606C
		public UriBuilder(string scheme, string host, int port, string pathValue)
			: this(scheme, host, port)
		{
			this.Path = pathValue;
		}

		// Token: 0x06001B72 RID: 7026 RVA: 0x00067080 File Offset: 0x00066080
		public UriBuilder(string scheme, string host, int port, string path, string extraValue)
			: this(scheme, host, port, path)
		{
			try
			{
				this.Extra = extraValue;
			}
			catch (Exception ex)
			{
				if (ex is ThreadAbortException || ex is StackOverflowException || ex is OutOfMemoryException)
				{
					throw;
				}
				throw new ArgumentException("extraValue");
			}
		}

		// Token: 0x17000548 RID: 1352
		// (set) Token: 0x06001B73 RID: 7027 RVA: 0x000670D8 File Offset: 0x000660D8
		private string Extra
		{
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (value.Length <= 0)
				{
					this.Fragment = string.Empty;
					this.Query = string.Empty;
					return;
				}
				if (value[0] == '#')
				{
					this.Fragment = value.Substring(1);
					return;
				}
				if (value[0] == '?')
				{
					int num = value.IndexOf('#');
					if (num == -1)
					{
						num = value.Length;
					}
					else
					{
						this.Fragment = value.Substring(num + 1);
					}
					this.Query = value.Substring(1, num - 1);
					return;
				}
				throw new ArgumentException("value");
			}
		}

		// Token: 0x17000549 RID: 1353
		// (get) Token: 0x06001B74 RID: 7028 RVA: 0x00067173 File Offset: 0x00066173
		// (set) Token: 0x06001B75 RID: 7029 RVA: 0x0006717B File Offset: 0x0006617B
		public string Fragment
		{
			get
			{
				return this.m_fragment;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (value.Length > 0)
				{
					value = '#' + value;
				}
				this.m_fragment = value;
				this.m_changed = true;
			}
		}

		// Token: 0x1700054A RID: 1354
		// (get) Token: 0x06001B76 RID: 7030 RVA: 0x000671AD File Offset: 0x000661AD
		// (set) Token: 0x06001B77 RID: 7031 RVA: 0x000671B8 File Offset: 0x000661B8
		public string Host
		{
			get
			{
				return this.m_host;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this.m_host = value;
				if (this.m_host.IndexOf(':') >= 0 && this.m_host[0] != '[')
				{
					this.m_host = "[" + this.m_host + "]";
				}
				this.m_changed = true;
			}
		}

		// Token: 0x1700054B RID: 1355
		// (get) Token: 0x06001B78 RID: 7032 RVA: 0x00067218 File Offset: 0x00066218
		// (set) Token: 0x06001B79 RID: 7033 RVA: 0x00067220 File Offset: 0x00066220
		public string Password
		{
			get
			{
				return this.m_password;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this.m_password = value;
			}
		}

		// Token: 0x1700054C RID: 1356
		// (get) Token: 0x06001B7A RID: 7034 RVA: 0x00067233 File Offset: 0x00066233
		// (set) Token: 0x06001B7B RID: 7035 RVA: 0x0006723B File Offset: 0x0006623B
		public string Path
		{
			get
			{
				return this.m_path;
			}
			set
			{
				if (value == null || value.Length == 0)
				{
					value = "/";
				}
				this.m_path = Uri.InternalEscapeString(this.ConvertSlashes(value));
				this.m_changed = true;
			}
		}

		// Token: 0x1700054D RID: 1357
		// (get) Token: 0x06001B7C RID: 7036 RVA: 0x00067268 File Offset: 0x00066268
		// (set) Token: 0x06001B7D RID: 7037 RVA: 0x00067270 File Offset: 0x00066270
		public int Port
		{
			get
			{
				return this.m_port;
			}
			set
			{
				if (value < -1 || value > 65535)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				this.m_port = value;
				this.m_changed = true;
			}
		}

		// Token: 0x1700054E RID: 1358
		// (get) Token: 0x06001B7E RID: 7038 RVA: 0x00067297 File Offset: 0x00066297
		// (set) Token: 0x06001B7F RID: 7039 RVA: 0x0006729F File Offset: 0x0006629F
		public string Query
		{
			get
			{
				return this.m_query;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				if (value.Length > 0)
				{
					value = '?' + value;
				}
				this.m_query = value;
				this.m_changed = true;
			}
		}

		// Token: 0x1700054F RID: 1359
		// (get) Token: 0x06001B80 RID: 7040 RVA: 0x000672D1 File Offset: 0x000662D1
		// (set) Token: 0x06001B81 RID: 7041 RVA: 0x000672DC File Offset: 0x000662DC
		public string Scheme
		{
			get
			{
				return this.m_scheme;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				int num = value.IndexOf(':');
				if (num != -1)
				{
					value = value.Substring(0, num);
				}
				if (value.Length != 0)
				{
					if (!Uri.CheckSchemeName(value))
					{
						throw new ArgumentException("value");
					}
					value = value.ToLower(CultureInfo.InvariantCulture);
				}
				this.m_scheme = value;
				this.m_changed = true;
			}
		}

		// Token: 0x17000550 RID: 1360
		// (get) Token: 0x06001B82 RID: 7042 RVA: 0x00067340 File Offset: 0x00066340
		public Uri Uri
		{
			get
			{
				if (this.m_changed)
				{
					this.m_uri = new Uri(this.ToString());
					this.SetFieldsFromUri(this.m_uri);
					this.m_changed = false;
				}
				return this.m_uri;
			}
		}

		// Token: 0x17000551 RID: 1361
		// (get) Token: 0x06001B83 RID: 7043 RVA: 0x00067374 File Offset: 0x00066374
		// (set) Token: 0x06001B84 RID: 7044 RVA: 0x0006737C File Offset: 0x0006637C
		public string UserName
		{
			get
			{
				return this.m_username;
			}
			set
			{
				if (value == null)
				{
					value = string.Empty;
				}
				this.m_username = value;
			}
		}

		// Token: 0x06001B85 RID: 7045 RVA: 0x00067390 File Offset: 0x00066390
		private string ConvertSlashes(string path)
		{
			StringBuilder stringBuilder = new StringBuilder(path.Length);
			foreach (char c in path)
			{
				if (c == '\\')
				{
					c = '/';
				}
				stringBuilder.Append(c);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06001B86 RID: 7046 RVA: 0x000673D8 File Offset: 0x000663D8
		public override bool Equals(object rparam)
		{
			return rparam != null && this.Uri.Equals(rparam.ToString());
		}

		// Token: 0x06001B87 RID: 7047 RVA: 0x000673F0 File Offset: 0x000663F0
		public override int GetHashCode()
		{
			return this.Uri.GetHashCode();
		}

		// Token: 0x06001B88 RID: 7048 RVA: 0x00067400 File Offset: 0x00066400
		private void SetFieldsFromUri(Uri uri)
		{
			this.m_fragment = uri.Fragment;
			this.m_query = uri.Query;
			this.m_host = uri.Host;
			this.m_path = uri.AbsolutePath;
			this.m_port = uri.Port;
			this.m_scheme = uri.Scheme;
			this.m_schemeDelimiter = (uri.HasAuthority ? Uri.SchemeDelimiter : ":");
			string userInfo = uri.UserInfo;
			if (userInfo.Length > 0)
			{
				int num = userInfo.IndexOf(':');
				if (num != -1)
				{
					this.m_password = userInfo.Substring(num + 1);
					this.m_username = userInfo.Substring(0, num);
					return;
				}
				this.m_username = userInfo;
			}
		}

		// Token: 0x06001B89 RID: 7049 RVA: 0x000674B4 File Offset: 0x000664B4
		public override string ToString()
		{
			if (this.m_username.Length == 0 && this.m_password.Length > 0)
			{
				throw new UriFormatException(SR.GetString("net_uri_BadUserPassword"));
			}
			if (this.m_scheme.Length != 0)
			{
				UriParser syntax = UriParser.GetSyntax(this.m_scheme);
				if (syntax != null)
				{
					this.m_schemeDelimiter = ((syntax.InFact(UriSyntaxFlags.MustHaveAuthority) || (this.m_host.Length != 0 && syntax.NotAny(UriSyntaxFlags.MailToLikeUri) && syntax.InFact(UriSyntaxFlags.OptionalAuthority))) ? Uri.SchemeDelimiter : ":");
				}
				else
				{
					this.m_schemeDelimiter = ((this.m_host.Length != 0) ? Uri.SchemeDelimiter : ":");
				}
			}
			string text = ((this.m_scheme.Length != 0) ? (this.m_scheme + this.m_schemeDelimiter) : string.Empty);
			return string.Concat(new string[]
			{
				text,
				this.m_username,
				(this.m_password.Length > 0) ? (":" + this.m_password) : string.Empty,
				(this.m_username.Length > 0) ? "@" : string.Empty,
				this.m_host,
				(this.m_port != -1 && this.m_host.Length > 0) ? (":" + this.m_port) : string.Empty,
				(this.m_host.Length > 0 && this.m_path.Length != 0 && this.m_path[0] != '/') ? "/" : string.Empty,
				this.m_path,
				this.m_query,
				this.m_fragment
			});
		}

		// Token: 0x04001C15 RID: 7189
		private bool m_changed = true;

		// Token: 0x04001C16 RID: 7190
		private string m_fragment = string.Empty;

		// Token: 0x04001C17 RID: 7191
		private string m_host = "localhost";

		// Token: 0x04001C18 RID: 7192
		private string m_password = string.Empty;

		// Token: 0x04001C19 RID: 7193
		private string m_path = "/";

		// Token: 0x04001C1A RID: 7194
		private int m_port = -1;

		// Token: 0x04001C1B RID: 7195
		private string m_query = string.Empty;

		// Token: 0x04001C1C RID: 7196
		private string m_scheme = "http";

		// Token: 0x04001C1D RID: 7197
		private string m_schemeDelimiter = Uri.SchemeDelimiter;

		// Token: 0x04001C1E RID: 7198
		private Uri m_uri;

		// Token: 0x04001C1F RID: 7199
		private string m_username = string.Empty;
	}
}
