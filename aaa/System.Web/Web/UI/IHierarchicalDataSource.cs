using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020003F9 RID: 1017
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IHierarchicalDataSource
	{
		// Token: 0x14000042 RID: 66
		// (add) Token: 0x0600323A RID: 12858
		// (remove) Token: 0x0600323B RID: 12859
		event EventHandler DataSourceChanged;

		// Token: 0x0600323C RID: 12860
		HierarchicalDataSourceView GetHierarchicalView(string viewPath);
	}
}
