using System;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020003F0 RID: 1008
	[SupportsPreviewControl(true)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class PreviewControlDesigner : ControlDesigner
	{
		// Token: 0x170006F5 RID: 1781
		// (get) Token: 0x06002544 RID: 9540 RVA: 0x000C8116 File Offset: 0x000C7116
		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}
	}
}
