using System;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000469 RID: 1129
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class LoginNameDesigner : ControlDesigner
	{
		// Token: 0x1700079A RID: 1946
		// (get) Token: 0x0600290C RID: 10508 RVA: 0x000E13BB File Offset: 0x000E03BB
		protected override bool UsePreviewControl
		{
			get
			{
				return true;
			}
		}

		// Token: 0x0600290D RID: 10509 RVA: 0x000E13BE File Offset: 0x000E03BE
		protected override string GetErrorDesignTimeHtml(Exception e)
		{
			return base.CreatePlaceHolderDesignTimeHtml(SR.GetString("Control_ErrorRenderingShort") + "<br />" + e.Message);
		}
	}
}
