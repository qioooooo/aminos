using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200041A RID: 1050
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IUserControlTypeResolutionService
	{
		// Token: 0x060032CC RID: 13004
		Type GetType(string tagPrefix, string tagName);
	}
}
