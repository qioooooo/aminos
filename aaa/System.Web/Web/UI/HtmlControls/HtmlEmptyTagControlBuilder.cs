using System;
using System.Security.Permissions;

namespace System.Web.UI.HtmlControls
{
	// Token: 0x02000493 RID: 1171
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public sealed class HtmlEmptyTagControlBuilder : ControlBuilder
	{
		// Token: 0x060036DD RID: 14045 RVA: 0x000ECAC2 File Offset: 0x000EBAC2
		public override bool HasBody()
		{
			return false;
		}
	}
}
