using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000419 RID: 1049
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IUserControlDesignerAccessor
	{
		// Token: 0x17000B2F RID: 2863
		// (get) Token: 0x060032C8 RID: 13000
		// (set) Token: 0x060032C9 RID: 13001
		string InnerText { get; set; }

		// Token: 0x17000B30 RID: 2864
		// (get) Token: 0x060032CA RID: 13002
		// (set) Token: 0x060032CB RID: 13003
		string TagName { get; set; }
	}
}
