using System;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x02000016 RID: 22
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IHttpHandler
	{
		// Token: 0x0600004D RID: 77
		void ProcessRequest(HttpContext context);

		// Token: 0x1700000F RID: 15
		// (get) Token: 0x0600004E RID: 78
		bool IsReusable { get; }
	}
}
