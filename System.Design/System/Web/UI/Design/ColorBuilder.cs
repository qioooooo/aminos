using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Windows.Forms;

namespace System.Web.UI.Design
{
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public sealed class ColorBuilder
	{
		private ColorBuilder()
		{
		}

		public static string BuildColor(IComponent component, Control owner, string initialColor)
		{
			string text = null;
			ISite site = component.Site;
			if (site == null)
			{
				return null;
			}
			if (site != null)
			{
				IWebFormsBuilderUIService webFormsBuilderUIService = (IWebFormsBuilderUIService)site.GetService(typeof(IWebFormsBuilderUIService));
				if (webFormsBuilderUIService != null)
				{
					text = webFormsBuilderUIService.BuildColor(owner, initialColor);
				}
			}
			return text;
		}
	}
}
