using System;
using System.Security.Permissions;
using System.Web.Util;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x02000700 RID: 1792
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[Serializable]
	public sealed class UserPersonalizationStateInfo : PersonalizationStateInfo
	{
		// Token: 0x06005771 RID: 22385 RVA: 0x00160F04 File Offset: 0x0015FF04
		public UserPersonalizationStateInfo(string path, DateTime lastUpdatedDate, int size, string username, DateTime lastActivityDate)
			: base(path, lastUpdatedDate, size)
		{
			this._username = StringUtil.CheckAndTrimString(username, "username");
			this._lastActivityDate = lastActivityDate.ToUniversalTime();
		}

		// Token: 0x1700168C RID: 5772
		// (get) Token: 0x06005772 RID: 22386 RVA: 0x00160F2E File Offset: 0x0015FF2E
		public string Username
		{
			get
			{
				return this._username;
			}
		}

		// Token: 0x1700168D RID: 5773
		// (get) Token: 0x06005773 RID: 22387 RVA: 0x00160F36 File Offset: 0x0015FF36
		public DateTime LastActivityDate
		{
			get
			{
				return this._lastActivityDate.ToLocalTime();
			}
		}

		// Token: 0x04002F9D RID: 12189
		private string _username;

		// Token: 0x04002F9E RID: 12190
		private DateTime _lastActivityDate;
	}
}
