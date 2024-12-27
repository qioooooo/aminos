using System;
using System.Security.Permissions;

namespace System.Web.Profile
{
	// Token: 0x02000309 RID: 777
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public class ProfileInfo
	{
		// Token: 0x06002657 RID: 9815 RVA: 0x000A4A28 File Offset: 0x000A3A28
		public ProfileInfo(string username, bool isAnonymous, DateTime lastActivityDate, DateTime lastUpdatedDate, int size)
		{
			if (username != null)
			{
				username = username.Trim();
			}
			this._UserName = username;
			if (lastActivityDate.Kind == DateTimeKind.Local)
			{
				lastActivityDate = lastActivityDate.ToUniversalTime();
			}
			this._LastActivityDate = lastActivityDate;
			if (lastUpdatedDate.Kind == DateTimeKind.Local)
			{
				lastUpdatedDate = lastUpdatedDate.ToUniversalTime();
			}
			this._LastUpdatedDate = lastUpdatedDate;
			this._IsAnonymous = isAnonymous;
			this._Size = size;
		}

		// Token: 0x06002658 RID: 9816 RVA: 0x000A4A91 File Offset: 0x000A3A91
		protected ProfileInfo()
		{
		}

		// Token: 0x170007FF RID: 2047
		// (get) Token: 0x06002659 RID: 9817 RVA: 0x000A4A99 File Offset: 0x000A3A99
		public virtual string UserName
		{
			get
			{
				return this._UserName;
			}
		}

		// Token: 0x17000800 RID: 2048
		// (get) Token: 0x0600265A RID: 9818 RVA: 0x000A4AA1 File Offset: 0x000A3AA1
		public virtual DateTime LastActivityDate
		{
			get
			{
				return this._LastActivityDate.ToLocalTime();
			}
		}

		// Token: 0x17000801 RID: 2049
		// (get) Token: 0x0600265B RID: 9819 RVA: 0x000A4AAE File Offset: 0x000A3AAE
		public virtual DateTime LastUpdatedDate
		{
			get
			{
				return this._LastUpdatedDate.ToLocalTime();
			}
		}

		// Token: 0x17000802 RID: 2050
		// (get) Token: 0x0600265C RID: 9820 RVA: 0x000A4ABB File Offset: 0x000A3ABB
		public virtual bool IsAnonymous
		{
			get
			{
				return this._IsAnonymous;
			}
		}

		// Token: 0x17000803 RID: 2051
		// (get) Token: 0x0600265D RID: 9821 RVA: 0x000A4AC3 File Offset: 0x000A3AC3
		public virtual int Size
		{
			get
			{
				return this._Size;
			}
		}

		// Token: 0x04001DB8 RID: 7608
		private string _UserName;

		// Token: 0x04001DB9 RID: 7609
		private DateTime _LastActivityDate;

		// Token: 0x04001DBA RID: 7610
		private DateTime _LastUpdatedDate;

		// Token: 0x04001DBB RID: 7611
		private bool _IsAnonymous;

		// Token: 0x04001DBC RID: 7612
		private int _Size;
	}
}
