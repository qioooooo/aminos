using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.Adapters
{
	// Token: 0x02000699 RID: 1689
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HideDisabledControlAdapter : WebControlAdapter
	{
		// Token: 0x060052AC RID: 21164 RVA: 0x0014D5CA File Offset: 0x0014C5CA
		protected internal override void Render(HtmlTextWriter writer)
		{
			if (!base.Control.Enabled)
			{
				return;
			}
			base.Control.Render(writer);
		}
	}
}
