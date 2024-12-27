using System;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x020003BD RID: 957
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class XmlDataFileEditor : UrlEditor
	{
		// Token: 0x17000687 RID: 1671
		// (get) Token: 0x0600233D RID: 9021 RVA: 0x000BE793 File Offset: 0x000BD793
		protected override string Caption
		{
			get
			{
				return SR.GetString("XmlDataFileEditor_Caption");
			}
		}

		// Token: 0x17000688 RID: 1672
		// (get) Token: 0x0600233E RID: 9022 RVA: 0x000BE79F File Offset: 0x000BD79F
		protected override string Filter
		{
			get
			{
				return SR.GetString("XmlDataFileEditor_Filter");
			}
		}
	}
}
