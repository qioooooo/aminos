using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200001F RID: 31
	public class GTRegex : Regex
	{
		// Token: 0x06000046 RID: 70 RVA: 0x000079D0 File Offset: 0x000069D0
		public GTRegex()
		{
			this.pattern = "[^%]>";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new GTRegexFactory10();
			this.capsize = 1;
			base.InitializeReferences();
		}
	}
}
