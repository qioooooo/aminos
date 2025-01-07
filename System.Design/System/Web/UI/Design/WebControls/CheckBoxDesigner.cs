using System;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SupportsPreviewControl(true)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class CheckBoxDesigner : ControlDesigner
	{
		public override string GetDesignTimeHtml()
		{
			CheckBox checkBox = (CheckBox)base.ViewControl;
			string text = checkBox.Text;
			bool flag = text == null || text.Length == 0;
			if (flag)
			{
				checkBox.Text = "[" + checkBox.ID + "]";
			}
			string designTimeHtml = base.GetDesignTimeHtml();
			if (flag)
			{
				checkBox.Text = text;
			}
			return designTimeHtml;
		}
	}
}
