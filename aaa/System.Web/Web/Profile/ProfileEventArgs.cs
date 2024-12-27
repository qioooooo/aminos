using System;
using System.Security.Permissions;

namespace System.Web.Profile
{
	// Token: 0x02000307 RID: 775
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class ProfileEventArgs : EventArgs
	{
		// Token: 0x170007FD RID: 2045
		// (get) Token: 0x0600264F RID: 9807 RVA: 0x000A4A00 File Offset: 0x000A3A00
		public HttpContext Context
		{
			get
			{
				return this._Context;
			}
		}

		// Token: 0x170007FE RID: 2046
		// (get) Token: 0x06002650 RID: 9808 RVA: 0x000A4A08 File Offset: 0x000A3A08
		// (set) Token: 0x06002651 RID: 9809 RVA: 0x000A4A10 File Offset: 0x000A3A10
		public ProfileBase Profile
		{
			get
			{
				return this._Profile;
			}
			set
			{
				this._Profile = value;
			}
		}

		// Token: 0x06002652 RID: 9810 RVA: 0x000A4A19 File Offset: 0x000A3A19
		public ProfileEventArgs(HttpContext context)
		{
			this._Context = context;
		}

		// Token: 0x04001DB6 RID: 7606
		private HttpContext _Context;

		// Token: 0x04001DB7 RID: 7607
		private ProfileBase _Profile;
	}
}
