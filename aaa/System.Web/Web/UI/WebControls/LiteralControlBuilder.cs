using System;
using System.Security.Permissions;

namespace System.Web.UI.WebControls
{
	// Token: 0x020005CE RID: 1486
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	public class LiteralControlBuilder : ControlBuilder
	{
		// Token: 0x06004882 RID: 18562 RVA: 0x0012781B File Offset: 0x0012681B
		public override bool AllowWhitespaceLiterals()
		{
			return false;
		}

		// Token: 0x06004883 RID: 18563 RVA: 0x0012781E File Offset: 0x0012681E
		public override void AppendLiteralString(string s)
		{
			if (Util.IsWhiteSpaceString(s))
			{
				base.AppendLiteralString(s);
				return;
			}
			base.PreprocessAttribute(string.Empty, "text", s, false);
		}

		// Token: 0x06004884 RID: 18564 RVA: 0x00127844 File Offset: 0x00126844
		public override void AppendSubBuilder(ControlBuilder subBuilder)
		{
			throw new HttpException(SR.GetString("Control_does_not_allow_children", new object[] { base.ControlType.ToString() }));
		}
	}
}
