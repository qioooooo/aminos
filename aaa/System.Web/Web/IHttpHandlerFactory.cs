using System;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x020000A1 RID: 161
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IHttpHandlerFactory
	{
		// Token: 0x06000844 RID: 2116
		IHttpHandler GetHandler(HttpContext context, string requestType, string url, string pathTranslated);

		// Token: 0x06000845 RID: 2117
		void ReleaseHandler(IHttpHandler handler);
	}
}
