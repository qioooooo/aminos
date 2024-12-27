using System;
using System.Design;
using System.Security.Permissions;

namespace System.Web.UI.Design
{
	// Token: 0x020003C5 RID: 965
	[SecurityPermission(SecurityAction.Demand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	public class XslUrlEditor : UrlEditor
	{
		// Token: 0x1700069B RID: 1691
		// (get) Token: 0x06002361 RID: 9057 RVA: 0x000BECFF File Offset: 0x000BDCFF
		protected override string Caption
		{
			get
			{
				return SR.GetString("UrlPicker_XslCaption");
			}
		}

		// Token: 0x1700069C RID: 1692
		// (get) Token: 0x06002362 RID: 9058 RVA: 0x000BED0B File Offset: 0x000BDD0B
		protected override string Filter
		{
			get
			{
				return SR.GetString("UrlPicker_XslFilter");
			}
		}

		// Token: 0x1700069D RID: 1693
		// (get) Token: 0x06002363 RID: 9059 RVA: 0x000BED17 File Offset: 0x000BDD17
		protected override UrlBuilderOptions Options
		{
			get
			{
				return UrlBuilderOptions.NoAbsolute;
			}
		}
	}
}
