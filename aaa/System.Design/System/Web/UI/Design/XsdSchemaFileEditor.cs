using System;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x020003C3 RID: 963
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class XsdSchemaFileEditor : UrlEditor
	{
		// Token: 0x17000697 RID: 1687
		// (get) Token: 0x0600235B RID: 9051 RVA: 0x000BECBF File Offset: 0x000BDCBF
		protected override string Caption
		{
			get
			{
				return SR.GetString("XsdSchemaFileEditor_Caption");
			}
		}

		// Token: 0x17000698 RID: 1688
		// (get) Token: 0x0600235C RID: 9052 RVA: 0x000BECCB File Offset: 0x000BDCCB
		protected override string Filter
		{
			get
			{
				return SR.GetString("XsdSchemaFileEditor_Filter");
			}
		}
	}
}
