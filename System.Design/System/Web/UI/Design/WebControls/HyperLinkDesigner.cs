using System;
using System.ComponentModel.Design;
using System.Security.Permissions;
using System.Web.UI.WebControls;

namespace System.Web.UI.Design.WebControls
{
	[SupportsPreviewControl(true)]
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class HyperLinkDesigner : TextControlDesigner
	{
		public override string GetDesignTimeHtml()
		{
			HyperLink hyperLink = (HyperLink)base.Component;
			string text = hyperLink.Text;
			string imageUrl = hyperLink.ImageUrl;
			string navigateUrl = hyperLink.NavigateUrl;
			bool flag = text.Trim().Length == 0 && imageUrl.Trim().Length == 0;
			bool flag2 = navigateUrl.Trim().Length == 0;
			bool flag3 = hyperLink.HasControls();
			Control[] array = null;
			if (flag)
			{
				if (flag3)
				{
					array = new Control[hyperLink.Controls.Count];
					hyperLink.Controls.CopyTo(array, 0);
				}
				hyperLink.Text = "[" + hyperLink.ID + "]";
			}
			if (flag2)
			{
				hyperLink.NavigateUrl = "url";
			}
			string designTimeHtml;
			try
			{
				designTimeHtml = base.GetDesignTimeHtml();
			}
			finally
			{
				if (flag)
				{
					hyperLink.Text = text;
					if (flag3)
					{
						foreach (Control control in array)
						{
							hyperLink.Controls.Add(control);
						}
					}
				}
				if (flag2)
				{
					hyperLink.NavigateUrl = navigateUrl;
				}
			}
			return designTimeHtml;
		}

		public override void OnComponentChanged(object sender, ComponentChangedEventArgs ce)
		{
			base.OnComponentChanged(sender, new ComponentChangedEventArgs(ce.Component, null, null, null));
		}
	}
}
