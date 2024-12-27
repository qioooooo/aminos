using System;
using System.Security.Permissions;
using System.Web.Configuration;

namespace System.Web
{
	// Token: 0x02000055 RID: 85
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HttpBrowserCapabilities : HttpCapabilitiesBase
	{
	}
}
