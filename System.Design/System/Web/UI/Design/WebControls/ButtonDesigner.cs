using System;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SupportsPreviewControl(true)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ButtonDesigner : ControlDesigner
	{
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
