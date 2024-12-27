using System;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x020003C2 RID: 962
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class XmlUrlEditor : UrlEditor
	{
		// Token: 0x17000694 RID: 1684
		// (get) Token: 0x06002357 RID: 9047 RVA: 0x000BEC9C File Offset: 0x000BDC9C
		protected override string Caption
		{
			get
			{
				return SR.GetString("UrlPicker_XmlCaption");
			}
		}

		// Token: 0x17000695 RID: 1685
		// (get) Token: 0x06002358 RID: 9048 RVA: 0x000BECA8 File Offset: 0x000BDCA8
		protected override string Filter
		{
			get
			{
				return SR.GetString("UrlPicker_XmlFilter");
			}
		}

		// Token: 0x17000696 RID: 1686
		// (get) Token: 0x06002359 RID: 9049 RVA: 0x000BECB4 File Offset: 0x000BDCB4
		protected override UrlBuilderOptions Options
		{
			get
			{
				return UrlBuilderOptions.NoAbsolute;
			}
		}
	}
}
