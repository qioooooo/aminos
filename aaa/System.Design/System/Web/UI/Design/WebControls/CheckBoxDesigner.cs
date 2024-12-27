using System;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000409 RID: 1033
	[SupportsPreviewControl(true)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class CheckBoxDesigner : ControlDesigner
	{
		// Token: 0x060025D5 RID: 9685 RVA: 0x000CBE0C File Offset: 0x000CAE0C
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
