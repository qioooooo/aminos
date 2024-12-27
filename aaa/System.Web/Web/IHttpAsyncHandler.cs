using System;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x02000017 RID: 23
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IHttpAsyncHandler : IHttpHandler
	{
		// Token: 0x0600004F RID: 79
		IAsyncResult BeginProcessRequest(HttpContext context, AsyncCallback cb, object extraData);

		// Token: 0x06000050 RID: 80
		void EndProcessRequest(IAsyncResult result);
	}
}
