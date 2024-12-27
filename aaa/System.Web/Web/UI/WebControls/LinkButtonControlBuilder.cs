using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005C7 RID: 1479
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class LinkButtonControlBuilder : ControlBuilder
	{
		// Token: 0x0600481F RID: 18463 RVA: 0x00126AF6 File Offset: 0x00125AF6
		public override bool AllowWhitespaceLiterals()
		{
			return false;
		}
	}
}
