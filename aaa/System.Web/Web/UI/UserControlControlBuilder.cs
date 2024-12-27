using System;
using System.Security.Permissions;

namespace System.Web.UI
{
	// Token: 0x02000423 RID: 1059
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class UserControlControlBuilder : ControlBuilder
	{
		// Token: 0x060032FA RID: 13050 RVA: 0x000DDB70 File Offset: 0x000DCB70
		public override object BuildObject()
		{
			object obj = base.BuildObject();
			if (base.InDesigner)
			{
				IUserControlDesignerAccessor userControlDesignerAccessor = (IUserControlDesignerAccessor)obj;
				userControlDesignerAccessor.TagName = base.TagName;
				if (this._innerText != null)
				{
					userControlDesignerAccessor.InnerText = this._innerText;
				}
			}
			return obj;
		}

		// Token: 0x060032FB RID: 13051 RVA: 0x000DDBB4 File Offset: 0x000DCBB4
		public override bool NeedsTagInnerText()
		{
			return base.InDesigner;
		}

		// Token: 0x060032FC RID: 13052 RVA: 0x000DDBBC File Offset: 0x000DCBBC
		public override void SetTagInnerText(string text)
		{
			this._innerText = text;
		}

		// Token: 0x040023DD RID: 9181
		private string _innerText;
	}
}
