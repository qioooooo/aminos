using System;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x0200037E RID: 894
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class ImageUrlEditor : UrlEditor
	{
		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06002135 RID: 8501 RVA: 0x000B8EA1 File Offset: 0x000B7EA1
		protected override string Caption
		{
			get
			{
				return SR.GetString("UrlPicker_ImageCaption");
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06002136 RID: 8502 RVA: 0x000B8EAD File Offset: 0x000B7EAD
		protected override string Filter
		{
			get
			{
				return SR.GetString("UrlPicker_ImageFilter");
			}
		}
	}
}
