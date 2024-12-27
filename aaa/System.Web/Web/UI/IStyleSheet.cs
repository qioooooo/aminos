using System;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI
{
	// Token: 0x02000416 RID: 1046
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IStyleSheet
	{
		// Token: 0x060032C3 RID: 12995
		void CreateStyleRule(Style style, IUrlResolutionService urlResolver, string selector);

		// Token: 0x060032C4 RID: 12996
		void RegisterStyle(Style style, IUrlResolutionService urlResolver);
	}
}
