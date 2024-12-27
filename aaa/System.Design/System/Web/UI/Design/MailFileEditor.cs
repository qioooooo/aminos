using System;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x02000388 RID: 904
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class MailFileEditor : UrlEditor
	{
		// Token: 0x1700060E RID: 1550
		// (get) Token: 0x06002164 RID: 8548 RVA: 0x000B8EC1 File Offset: 0x000B7EC1
		protected override string Caption
		{
			get
			{
				return SR.GetString("MailFilePicker_Caption");
			}
		}

		// Token: 0x1700060F RID: 1551
		// (get) Token: 0x06002165 RID: 8549 RVA: 0x000B8ECD File Offset: 0x000B7ECD
		protected override string Filter
		{
			get
			{
				return SR.GetString("MailFilePicker_Filter");
			}
		}
	}
}
