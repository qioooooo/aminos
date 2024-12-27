using System;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x020000CB RID: 203
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class SiteMapResolveEventArgs : EventArgs
	{
		// Token: 0x0600090E RID: 2318 RVA: 0x00028E6B File Offset: 0x00027E6B
		public SiteMapResolveEventArgs(HttpContext context, SiteMapProvider provider)
		{
			this._context = context;
			this._provider = provider;
		}

		// Token: 0x170002D9 RID: 729
		// (get) Token: 0x0600090F RID: 2319 RVA: 0x00028E81 File Offset: 0x00027E81
		public SiteMapProvider Provider
		{
			get
			{
				return this._provider;
			}
		}

		// Token: 0x170002DA RID: 730
		// (get) Token: 0x06000910 RID: 2320 RVA: 0x00028E89 File Offset: 0x00027E89
		public HttpContext Context
		{
			get
			{
				return this._context;
			}
		}

		// Token: 0x04001232 RID: 4658
		private HttpContext _context;

		// Token: 0x04001233 RID: 4659
		private SiteMapProvider _provider;
	}
}
