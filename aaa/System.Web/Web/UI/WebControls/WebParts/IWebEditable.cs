using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls.WebParts
{
	// Token: 0x020006C1 RID: 1729
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public interface IWebEditable
	{
		// Token: 0x170015C2 RID: 5570
		// (get) Token: 0x060054ED RID: 21741
		object WebBrowsableObject { get; }

		// Token: 0x060054EE RID: 21742
		EditorPartCollection CreateEditorParts();
	}
}
