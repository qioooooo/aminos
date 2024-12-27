using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200003A RID: 58
	internal class FormatStringRegex : Regex
	{
		// Token: 0x06000085 RID: 133 RVA: 0x0000D4B8 File Offset: 0x0000C4B8
		public FormatStringRegex()
		{
			this.pattern = "^(([^\"]*(\"\")?)*)$";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new FormatStringRegexFactory19();
			this.capsize = 4;
			base.InitializeReferences();
		}
	}
}
