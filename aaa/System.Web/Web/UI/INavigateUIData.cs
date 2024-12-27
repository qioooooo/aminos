using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x020000CD RID: 205
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface INavigateUIData
	{
		// Token: 0x170002DF RID: 735
		// (get) Token: 0x06000917 RID: 2327
		string Description { get; }

		// Token: 0x170002E0 RID: 736
		// (get) Token: 0x06000918 RID: 2328
		string Name { get; }

		// Token: 0x170002E1 RID: 737
		// (get) Token: 0x06000919 RID: 2329
		string NavigateUrl { get; }

		// Token: 0x170002E2 RID: 738
		// (get) Token: 0x0600091A RID: 2330
		string Value { get; }
	}
}
