using System;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x02000389 RID: 905
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class MdbDataFileEditor : UrlEditor
	{
		// Token: 0x17000610 RID: 1552
		// (get) Token: 0x06002167 RID: 8551 RVA: 0x000B8EE1 File Offset: 0x000B7EE1
		protected override string Caption
		{
			get
			{
				return SR.GetString("MdbDataFileEditor_Caption");
			}
		}

		// Token: 0x17000611 RID: 1553
		// (get) Token: 0x06002168 RID: 8552 RVA: 0x000B8EED File Offset: 0x000B7EED
		protected override string Filter
		{
			get
			{
				return SR.GetString("MdbDataFileEditor_Filter");
			}
		}
	}
}
