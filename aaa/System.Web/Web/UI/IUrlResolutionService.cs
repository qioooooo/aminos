using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003B7 RID: 951
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IUrlResolutionService
	{
		// Token: 0x06002E46 RID: 11846
		string ResolveClientUrl(string relativeUrl);
	}
}
