using System;
using System.Collections.Specialized;
using System.Security.Permissions;
using System.Text;
using System.Web.Configuration;

namespace System.Web
{
	// Token: 0x02000066 RID: 102
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HttpCookie
	{
		// Token: 0x0600045E RID: 1118 RVA: 0x00013246 File Offset: 0x00012246
		internal HttpCookie()
		{
			this._changed = true;
		}

		// Token: 0x0600045F RID: 1119 RVA: 0x00013260 File Offset: 0x00012260
		public HttpCookie(string name)
		{
			this._name = name;
			this.SetDefaultsFromConfig();
			this._changed = true;
		}

		// Token: 0x06000460 RID: 1120 RVA: 0x00013287 File Offset: 0x00012287
		public HttpCookie(string name, string value)
		{
			this._name = name;
			this._stringValue = value;
			this.SetDefaultsFromConfig();
			this._changed = true;
		}

		// Token: 0x06000461 RID: 1121 RVA: 0x000132B8 File Offset: 0x000122B8
		private void SetDefaultsFromConfig()
		{
			HttpCookiesSection httpCookies = RuntimeConfig.GetConfig().HttpCookies;
			this._secure = httpCookies.RequireSSL;
			this._httpOnly = httpCookies.HttpOnlyCookies;
			if (httpCookies.Domain != null && httpCookies.Domain.Length > 0)
			{
				this._domain = httpCookies.Domain;
			}
		}

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x06000462 RID: 1122 RVA: 0x0001330A File Offset: 0x0001230A
		// (set) Token: 0x06000463 RID: 1123 RVA: 0x00013312 File Offset: 0x00012312
		internal bool Changed
		{
			get
			{
				return this._changed;
			}
			set
			{
				this._changed = value;
			}
		}

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x06000464 RID: 1124 RVA: 0x0001331B File Offset: 0x0001231B
		// (set) Token: 0x06000465 RID: 1125 RVA: 0x00013323 File Offset: 0x00012323
		internal bool Added
		{
			get
			{
				return this._added;
			}
			set
			{
				this._added = value;
			}
		}

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x06000466 RID: 1126 RVA: 0x0001332C File Offset: 0x0001232C
		// (set) Token: 0x06000467 RID: 1127 RVA: 0x00013334 File Offset: 0x00012334
		public string Name
		{
			get
			{
				return this._name;
			}
			set
			{
				this._name = value;
				this._changed = true;
			}
		}

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x06000468 RID: 1128 RVA: 0x00013344 File Offset: 0x00012344
		// (set) Token: 0x06000469 RID: 1129 RVA: 0x0001334C File Offset: 0x0001234C
		public string Path
		{
			get
			{
				return this._path;
			}
			set
			{
				this._path = value;
				this._changed = true;
			}
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x0600046A RID: 1130 RVA: 0x0001335C File Offset: 0x0001235C
		// (set) Token: 0x0600046B RID: 1131 RVA: 0x00013364 File Offset: 0x00012364
		public bool Secure
		{
			get
			{
				return this._secure;
			}
			set
			{
				this._secure = value;
				this._changed = true;
			}
		}

		// Token: 0x170001BF RID: 447
		// (get) Token: 0x0600046C RID: 1132 RVA: 0x00013374 File Offset: 0x00012374
		// (set) Token: 0x0600046D RID: 1133 RVA: 0x0001337C File Offset: 0x0001237C
		public bool HttpOnly
		{
			get
			{
				return this._httpOnly;
			}
			set
			{
				this._httpOnly = value;
				this._changed = true;
			}
		}

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x0600046E RID: 1134 RVA: 0x0001338C File Offset: 0x0001238C
		// (set) Token: 0x0600046F RID: 1135 RVA: 0x00013394 File Offset: 0x00012394
		public string Domain
		{
			get
			{
				return this._domain;
			}
			set
			{
				this._domain = value;
				this._changed = true;
			}
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x06000470 RID: 1136 RVA: 0x000133A4 File Offset: 0x000123A4
		// (set) Token: 0x06000471 RID: 1137 RVA: 0x000133BA File Offset: 0x000123BA
		public DateTime Expires
		{
			get
			{
				if (!this._expirationSet)
				{
					return DateTime.MinValue;
				}
				return this._expires;
			}
			set
			{
				this._expires = value;
				this._expirationSet = true;
				this._changed = true;
			}
		}

		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x06000472 RID: 1138 RVA: 0x000133D1 File Offset: 0x000123D1
		// (set) Token: 0x06000473 RID: 1139 RVA: 0x000133EE File Offset: 0x000123EE
		public string Value
		{
			get
			{
				if (this._multiValue != null)
				{
					return this._multiValue.ToString(false);
				}
				return this._stringValue;
			}
			set
			{
				if (this._multiValue != null)
				{
					this._multiValue.Reset();
					this._multiValue.Add(null, value);
				}
				else
				{
					this._stringValue = value;
				}
				this._changed = true;
			}
		}

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x06000474 RID: 1140 RVA: 0x00013420 File Offset: 0x00012420
		public bool HasKeys
		{
			get
			{
				return this.Values.HasKeys();
			}
		}

		// Token: 0x06000475 RID: 1141 RVA: 0x00013430 File Offset: 0x00012430
		private bool SupportsHttpOnly(HttpContext context)
		{
			if (context != null && context.Request != null)
			{
				HttpBrowserCapabilities browser = context.Request.Browser;
				return browser != null && (browser.Type != "IE5" || browser.Platform != "MacPPC");
			}
			return false;
		}

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x06000476 RID: 1142 RVA: 0x00013480 File Offset: 0x00012480
		public NameValueCollection Values
		{
			get
			{
				if (this._multiValue == null)
				{
					this._multiValue = new HttpValueCollection();
					if (this._stringValue != null)
					{
						if (this._stringValue.IndexOf('&') >= 0 || this._stringValue.IndexOf('=') >= 0)
						{
							this._multiValue.FillFromString(this._stringValue);
						}
						else
						{
							this._multiValue.Add(null, this._stringValue);
						}
						this._stringValue = null;
					}
				}
				this._changed = true;
				return this._multiValue;
			}
		}

		// Token: 0x170001C5 RID: 453
		public string this[string key]
		{
			get
			{
				return this.Values[key];
			}
			set
			{
				this.Values[key] = value;
				this._changed = true;
			}
		}

		// Token: 0x06000479 RID: 1145 RVA: 0x00013528 File Offset: 0x00012528
		internal HttpResponseHeader GetSetCookieHeader(HttpContext context)
		{
			StringBuilder stringBuilder = new StringBuilder();
			if (!string.IsNullOrEmpty(this._name))
			{
				stringBuilder.Append(this._name);
				stringBuilder.Append('=');
			}
			if (this._multiValue != null)
			{
				stringBuilder.Append(this._multiValue.ToString(false));
			}
			else if (this._stringValue != null)
			{
				stringBuilder.Append(this._stringValue);
			}
			if (!string.IsNullOrEmpty(this._domain))
			{
				stringBuilder.Append("; domain=");
				stringBuilder.Append(this._domain);
			}
			if (this._expirationSet && this._expires != DateTime.MinValue)
			{
				stringBuilder.Append("; expires=");
				stringBuilder.Append(HttpUtility.FormatHttpCookieDateTime(this._expires));
			}
			if (!string.IsNullOrEmpty(this._path))
			{
				stringBuilder.Append("; path=");
				stringBuilder.Append(this._path);
			}
			if (this._secure)
			{
				stringBuilder.Append("; secure");
			}
			if (this._httpOnly && this.SupportsHttpOnly(context))
			{
				stringBuilder.Append("; HttpOnly");
			}
			return new HttpResponseHeader(27, stringBuilder.ToString());
		}

		// Token: 0x04001020 RID: 4128
		private string _name;

		// Token: 0x04001021 RID: 4129
		private string _path = "/";

		// Token: 0x04001022 RID: 4130
		private bool _secure;

		// Token: 0x04001023 RID: 4131
		private bool _httpOnly;

		// Token: 0x04001024 RID: 4132
		private string _domain;

		// Token: 0x04001025 RID: 4133
		private bool _expirationSet;

		// Token: 0x04001026 RID: 4134
		private DateTime _expires;

		// Token: 0x04001027 RID: 4135
		private string _stringValue;

		// Token: 0x04001028 RID: 4136
		private HttpValueCollection _multiValue;

		// Token: 0x04001029 RID: 4137
		private bool _changed;

		// Token: 0x0400102A RID: 4138
		private bool _added;
	}
}
