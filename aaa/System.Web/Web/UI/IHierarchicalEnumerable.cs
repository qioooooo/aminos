using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020000CF RID: 207
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IHierarchicalEnumerable : IEnumerable
	{
		// Token: 0x06000956 RID: 2390
		IHierarchyData GetHierarchyData(object enumeratedItem);
	}
}
