using System;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x020003C4 RID: 964
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class XslTransformFileEditor : UrlEditor
	{
		// Token: 0x17000699 RID: 1689
		// (get) Token: 0x0600235E RID: 9054 RVA: 0x000BECDF File Offset: 0x000BDCDF
		protected override string Caption
		{
			get
			{
				return SR.GetString("XslTransformFileEditor_Caption");
			}
		}

		// Token: 0x1700069A RID: 1690
		// (get) Token: 0x0600235F RID: 9055 RVA: 0x000BECEB File Offset: 0x000BDCEB
		protected override string Filter
		{
			get
			{
				return SR.GetString("XslTransformFileEditor_Filter");
			}
		}
	}
}
