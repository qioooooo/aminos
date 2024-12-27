using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005B8 RID: 1464
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HyperLinkControlBuilder : ControlBuilder
	{
		// Token: 0x06004790 RID: 18320 RVA: 0x00124759 File Offset: 0x00123759
		public override bool AllowWhitespaceLiterals()
		{
			return false;
		}
	}
}
