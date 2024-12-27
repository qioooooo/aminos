using System;
using System.Security.Permissions;

namespace System.Web.Configuration
{
	// Token: 0x020001F6 RID: 502
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HttpConfigurationContext
	{
		// Token: 0x17000543 RID: 1347
		// (get) Token: 0x06001B63 RID: 7011 RVA: 0x0007F15D File Offset: 0x0007E15D
		public string VirtualPath
		{
			get
			{
				return this.vpath;
			}
		}

		// Token: 0x06001B64 RID: 7012 RVA: 0x0007F165 File Offset: 0x0007E165
		internal HttpConfigurationContext(string vpath)
		{
			this.vpath = vpath;
		}

		// Token: 0x04001855 RID: 6229
		private string vpath;
	}
}
