using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000053 RID: 83
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IFilterResolutionService
	{
		// Token: 0x0600029E RID: 670
		bool EvaluateFilter(string filterName);

		// Token: 0x0600029F RID: 671
		int CompareFilters(string filter1, string filter2);
	}
}
