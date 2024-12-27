using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000413 RID: 1043
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IResourceUrlGenerator
	{
		// Token: 0x060032B3 RID: 12979
		string GetResourceUrl(Type type, string resourceName);
	}
}
