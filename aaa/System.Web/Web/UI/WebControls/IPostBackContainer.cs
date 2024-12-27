using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x02000556 RID: 1366
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IPostBackContainer
	{
		// Token: 0x06004334 RID: 17204
		PostBackOptions GetPostBackOptions(IButtonControl buttonControl);
	}
}
