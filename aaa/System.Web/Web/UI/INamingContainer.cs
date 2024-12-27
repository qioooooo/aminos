using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003E9 RID: 1001
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface INamingContainer
	{
	}
}
