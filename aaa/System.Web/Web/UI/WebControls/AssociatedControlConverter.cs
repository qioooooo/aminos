using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020004BF RID: 1215
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class AssociatedControlConverter : ControlIDConverter
	{
		// Token: 0x060039D9 RID: 14809 RVA: 0x000F4D5D File Offset: 0x000F3D5D
		protected override bool FilterControl(Control control)
		{
			return control is WebControl;
		}
	}
}
