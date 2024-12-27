using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005C2 RID: 1474
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class LabelControlBuilder : ControlBuilder
	{
		// Token: 0x0600480B RID: 18443 RVA: 0x00126814 File Offset: 0x00125814
		public override bool AllowWhitespaceLiterals()
		{
			return false;
		}
	}
}
