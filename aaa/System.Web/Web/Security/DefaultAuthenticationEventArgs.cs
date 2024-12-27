using System;
using System.Security.Permissions;

namespace System.Web.Security
{
	// Token: 0x0200033D RID: 829
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class DefaultAuthenticationEventArgs : EventArgs
	{
		// Token: 0x17000889 RID: 2185
		// (get) Token: 0x0600286D RID: 10349 RVA: 0x000B1D6F File Offset: 0x000B0D6F
		public HttpContext Context
		{
			get
			{
				return this._Context;
			}
		}

		// Token: 0x0600286E RID: 10350 RVA: 0x000B1D77 File Offset: 0x000B0D77
		public DefaultAuthenticationEventArgs(HttpContext context)
		{
			this._Context = context;
		}

		// Token: 0x04001EB1 RID: 7857
		private HttpContext _Context;
	}
}
