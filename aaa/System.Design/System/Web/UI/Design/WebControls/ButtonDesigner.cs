using System;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x020003F4 RID: 1012
	[SupportsPreviewControl(true)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ButtonDesigner : ControlDesigner
	{
		// Token: 0x0600255F RID: 9567 RVA: 0x000C84E4 File Offset: 0x000C74E4
		public override string GetDesignTimeHtml()
		{
			Button button = (Button)base.ViewControl;
			string text = button.Text;
			bool flag = text.Trim().Length == 0;
			if (flag)
			{
				button.Text = "[" + button.ID + "]";
			}
			string designTimeHtml = base.GetDesignTimeHtml();
			if (flag)
			{
				button.Text = text;
			}
			return designTimeHtml;
		}
	}
}
