using System;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x020003B5 RID: 949
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class UserControlFileEditor : UrlEditor
	{
		// Token: 0x1700067F RID: 1663
		// (get) Token: 0x0600231C RID: 8988 RVA: 0x000BE39E File Offset: 0x000BD39E
		protected override string Caption
		{
			get
			{
				return SR.GetString("UserControlFileEditor_Caption");
			}
		}

		// Token: 0x17000680 RID: 1664
		// (get) Token: 0x0600231D RID: 8989 RVA: 0x000BE3AA File Offset: 0x000BD3AA
		protected override string Filter
		{
			get
			{
				return SR.GetString("UserControlFileEditor_Filter");
			}
		}
	}
}
