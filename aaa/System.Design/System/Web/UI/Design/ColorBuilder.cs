using System;
using System.ComponentModel;
using System.Security.Permissions;
using System.Windows.Forms;

namespace System.Web.UI.Design
{
	// Token: 0x02000323 RID: 803
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public sealed class ColorBuilder
	{
		// Token: 0x06001E2E RID: 7726 RVA: 0x000ABB57 File Offset: 0x000AAB57
		private ColorBuilder()
		{
		}

		// Token: 0x06001E2F RID: 7727 RVA: 0x000ABB60 File Offset: 0x000AAB60
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
