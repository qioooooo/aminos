using System;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design.WebControls
{
	// Token: 0x02000471 RID: 1137
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class MailDefinitionBodyFileNameEditor : UrlEditor
	{
		// Token: 0x170007AD RID: 1965
		// (get) Token: 0x06002949 RID: 10569 RVA: 0x000E21D9 File Offset: 0x000E11D9
		protected override string Caption
		{
			get
			{
				return SR.GetString("MailDefinitionBodyFileNameEditor_DefaultCaption");
			}
		}

		// Token: 0x170007AE RID: 1966
		// (get) Token: 0x0600294A RID: 10570 RVA: 0x000E21E5 File Offset: 0x000E11E5
		protected override string Filter
		{
			get
			{
				return SR.GetString("MailDefinitionBodyFileNameEditor_DefaultFilter");
			}
		}
	}
}
