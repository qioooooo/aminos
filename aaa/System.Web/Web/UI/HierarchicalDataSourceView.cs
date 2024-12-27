using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003FB RID: 1019
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public abstract class HierarchicalDataSourceView
	{
		// Token: 0x06003252 RID: 12882
		public abstract IHierarchicalEnumerable Select();
	}
}
