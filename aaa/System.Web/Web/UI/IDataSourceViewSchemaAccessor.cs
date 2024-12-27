using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x0200040A RID: 1034
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IDataSourceViewSchemaAccessor
	{
		// Token: 0x17000B2A RID: 2858
		// (get) Token: 0x0600329E RID: 12958
		// (set) Token: 0x0600329F RID: 12959
		object DataSourceViewSchema { get; set; }
	}
}
