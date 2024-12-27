using System;
using System.Security.Permissions;

namespace System.Web.Security
{
	// Token: 0x0200034F RID: 847
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class RoleManagerEventArgs : EventArgs
	{
		// Token: 0x170008B9 RID: 2233
		// (get) Token: 0x06002920 RID: 10528 RVA: 0x000B4254 File Offset: 0x000B3254
		// (set) Token: 0x06002921 RID: 10529 RVA: 0x000B425C File Offset: 0x000B325C
		public bool RolesPopulated
		{
			get
			{
				return this._RolesPopulated;
			}
			set
			{
				this._RolesPopulated = value;
			}
		}

		// Token: 0x170008BA RID: 2234
		// (get) Token: 0x06002922 RID: 10530 RVA: 0x000B4265 File Offset: 0x000B3265
		public HttpContext Context
		{
			get
			{
				return this._Context;
			}
		}

		// Token: 0x06002923 RID: 10531 RVA: 0x000B426D File Offset: 0x000B326D
		public RoleManagerEventArgs(HttpContext context)
		{
			this._Context = context;
		}

		// Token: 0x04001EE5 RID: 7909
		private HttpContext _Context;

		// Token: 0x04001EE6 RID: 7910
		private bool _RolesPopulated;
	}
}
