using System;
using System.Security.Permissions;

namespace System.Web.Security
{
	// Token: 0x02000338 RID: 824
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class FormsAuthenticationTicket
	{
		// Token: 0x1700087B RID: 2171
		// (get) Token: 0x06002852 RID: 10322 RVA: 0x000B1846 File Offset: 0x000B0846
		public int Version
		{
			get
			{
				return this._Version;
			}
		}

		// Token: 0x1700087C RID: 2172
		// (get) Token: 0x06002853 RID: 10323 RVA: 0x000B184E File Offset: 0x000B084E
		public string Name
		{
			get
			{
				return this._Name;
			}
		}

		// Token: 0x1700087D RID: 2173
		// (get) Token: 0x06002854 RID: 10324 RVA: 0x000B1856 File Offset: 0x000B0856
		public DateTime Expiration
		{
			get
			{
				return this._Expiration;
			}
		}

		// Token: 0x1700087E RID: 2174
		// (get) Token: 0x06002855 RID: 10325 RVA: 0x000B185E File Offset: 0x000B085E
		public DateTime IssueDate
		{
			get
			{
				return this._IssueDate;
			}
		}

		// Token: 0x1700087F RID: 2175
		// (get) Token: 0x06002856 RID: 10326 RVA: 0x000B1866 File Offset: 0x000B0866
		public bool IsPersistent
		{
			get
			{
				return this._IsPersistent;
			}
		}

		// Token: 0x17000880 RID: 2176
		// (get) Token: 0x06002857 RID: 10327 RVA: 0x000B186E File Offset: 0x000B086E
		public bool Expired
		{
			get
			{
				return this.ExpirationUtc < DateTime.UtcNow;
			}
		}

		// Token: 0x17000881 RID: 2177
		// (get) Token: 0x06002858 RID: 10328 RVA: 0x000B1880 File Offset: 0x000B0880
		public string UserData
		{
			get
			{
				return this._UserData;
			}
		}

		// Token: 0x17000882 RID: 2178
		// (get) Token: 0x06002859 RID: 10329 RVA: 0x000B1888 File Offset: 0x000B0888
		public string CookiePath
		{
			get
			{
				return this._CookiePath;
			}
		}

		// Token: 0x17000883 RID: 2179
		// (get) Token: 0x0600285A RID: 10330 RVA: 0x000B1890 File Offset: 0x000B0890
		internal DateTime ExpirationUtc
		{
			get
			{
				if (!this._ExpirationUtcHasValue)
				{
					return this.Expiration.ToUniversalTime();
				}
				return this._ExpirationUtc;
			}
		}

		// Token: 0x17000884 RID: 2180
		// (get) Token: 0x0600285B RID: 10331 RVA: 0x000B18BC File Offset: 0x000B08BC
		internal DateTime IssueDateUtc
		{
			get
			{
				if (!this._IssueDateUtcHasValue)
				{
					return this.IssueDate.ToUniversalTime();
				}
				return this._IssueDateUtc;
			}
		}

		// Token: 0x0600285C RID: 10332 RVA: 0x000B18E6 File Offset: 0x000B08E6
		public FormsAuthenticationTicket(int version, string name, DateTime issueDate, DateTime expiration, bool isPersistent, string userData)
		{
			this._Version = version;
			this._Name = name;
			this._Expiration = expiration;
			this._IssueDate = issueDate;
			this._IsPersistent = isPersistent;
			this._UserData = userData;
			this._CookiePath = FormsAuthentication.FormsCookiePath;
		}

		// Token: 0x0600285D RID: 10333 RVA: 0x000B1926 File Offset: 0x000B0926
		public FormsAuthenticationTicket(int version, string name, DateTime issueDate, DateTime expiration, bool isPersistent, string userData, string cookiePath)
		{
			this._Version = version;
			this._Name = name;
			this._Expiration = expiration;
			this._IssueDate = issueDate;
			this._IsPersistent = isPersistent;
			this._UserData = userData;
			this._CookiePath = cookiePath;
		}

		// Token: 0x0600285E RID: 10334 RVA: 0x000B1964 File Offset: 0x000B0964
		public FormsAuthenticationTicket(string name, bool isPersistent, int timeout)
		{
			this._Version = 2;
			this._Name = name;
			this._IssueDateUtcHasValue = true;
			this._IssueDateUtc = DateTime.UtcNow;
			this._IssueDate = DateTime.Now;
			this._IsPersistent = isPersistent;
			this._UserData = "";
			this._ExpirationUtcHasValue = true;
			this._ExpirationUtc = this._IssueDateUtc.AddMinutes((double)timeout);
			this._Expiration = this._IssueDate.AddMinutes((double)timeout);
			this._CookiePath = FormsAuthentication.FormsCookiePath;
		}

		// Token: 0x0600285F RID: 10335 RVA: 0x000B19EC File Offset: 0x000B09EC
		internal static FormsAuthenticationTicket FromUtc(int version, string name, DateTime issueDateUtc, DateTime expirationUtc, bool isPersistent, string userData, string cookiePath)
		{
			return new FormsAuthenticationTicket(version, name, issueDateUtc.ToLocalTime(), expirationUtc.ToLocalTime(), isPersistent, userData, cookiePath)
			{
				_IssueDateUtcHasValue = true,
				_IssueDateUtc = issueDateUtc,
				_ExpirationUtcHasValue = true,
				_ExpirationUtc = expirationUtc
			};
		}

		// Token: 0x04001EA4 RID: 7844
		private int _Version;

		// Token: 0x04001EA5 RID: 7845
		private string _Name;

		// Token: 0x04001EA6 RID: 7846
		private DateTime _Expiration;

		// Token: 0x04001EA7 RID: 7847
		private DateTime _IssueDate;

		// Token: 0x04001EA8 RID: 7848
		private bool _IsPersistent;

		// Token: 0x04001EA9 RID: 7849
		private string _UserData;

		// Token: 0x04001EAA RID: 7850
		private string _CookiePath;

		// Token: 0x04001EAB RID: 7851
		[NonSerialized]
		private bool _ExpirationUtcHasValue;

		// Token: 0x04001EAC RID: 7852
		[NonSerialized]
		private DateTime _ExpirationUtc;

		// Token: 0x04001EAD RID: 7853
		[NonSerialized]
		private bool _IssueDateUtcHasValue;

		// Token: 0x04001EAE RID: 7854
		[NonSerialized]
		private DateTime _IssueDateUtc;
	}
}
