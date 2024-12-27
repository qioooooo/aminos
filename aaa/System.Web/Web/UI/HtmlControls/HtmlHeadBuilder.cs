using System;
using System.Collections;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x02000495 RID: 1173
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class HtmlHeadBuilder : ControlBuilder
	{
		// Token: 0x060036E3 RID: 14051 RVA: 0x000ECB04 File Offset: 0x000EBB04
		public override Type GetChildControlType(string tagName, IDictionary attribs)
		{
			if (string.Equals(tagName, "title", StringComparison.OrdinalIgnoreCase))
			{
				return typeof(HtmlTitle);
			}
			if (string.Equals(tagName, "link", StringComparison.OrdinalIgnoreCase))
			{
				return typeof(HtmlLink);
			}
			if (string.Equals(tagName, "meta", StringComparison.OrdinalIgnoreCase))
			{
				return typeof(HtmlMeta);
			}
			return null;
		}

		// Token: 0x060036E4 RID: 14052 RVA: 0x000ECB5D File Offset: 0x000EBB5D
		public override bool AllowWhitespaceLiterals()
		{
			return false;
		}
	}
}
