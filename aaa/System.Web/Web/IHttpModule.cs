using System;
using System.Security.Permissions;

namespace System.Web
{
	// Token: 0x020000A3 RID: 163
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IHttpModule
	{
		// Token: 0x06000847 RID: 2119
		void Init(HttpApplication context);

		// Token: 0x06000848 RID: 2120
		void Dispose();
	}
}
