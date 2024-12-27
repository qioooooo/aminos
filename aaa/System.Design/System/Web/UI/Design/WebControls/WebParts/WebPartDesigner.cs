using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Web.UI.WebControls.WebParts;

namespace System.Web.UI.Design.WebControls.WebParts
{
	// Token: 0x02000543 RID: 1347
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class WebPartDesigner : PartDesigner
	{
		// Token: 0x06002F71 RID: 12145 RVA: 0x0010E6BB File Offset: 0x0010D6BB
		public override void Initialize(IComponent component)
		{
			ControlDesigner.VerifyInitializeArgument(component, typeof(WebPart));
			base.Initialize(component);
		}
	}
}
