using System;
using System.Security.Permissions;

namespace System.Web.Profile
{
	// Token: 0x0200030E RID: 782
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProfileMigrateEventArgs : EventArgs
	{
		// Token: 0x1700080D RID: 2061
		// (get) Token: 0x06002696 RID: 9878 RVA: 0x000A585C File Offset: 0x000A485C
		public HttpContext Context
		{
			get
			{
				return this._Context;
			}
		}

		// Token: 0x1700080E RID: 2062
		// (get) Token: 0x06002697 RID: 9879 RVA: 0x000A5864 File Offset: 0x000A4864
		public string AnonymousID
		{
			get
			{
				return this._AnonymousId;
			}
		}

		// Token: 0x06002698 RID: 9880 RVA: 0x000A586C File Offset: 0x000A486C
		public ProfileMigrateEventArgs(HttpContext context, string anonymousId)
		{
			this._Context = context;
			this._AnonymousId = anonymousId;
		}

		// Token: 0x04001DCF RID: 7631
		private HttpContext _Context;

		// Token: 0x04001DD0 RID: 7632
		private string _AnonymousId;
	}
}
