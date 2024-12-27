using System;

namespace System.Web
{
	// Token: 0x020000A2 RID: 162
	internal interface IHttpHandlerFactory2 : IHttpHandlerFactory
	{
		// Token: 0x06000846 RID: 2118
		IHttpHandler GetHandler(HttpContext context, string requestType, VirtualPath virtualPath, string physicalPath);
	}
}
