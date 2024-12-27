using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200001C RID: 28
	public class TextRegex : Regex
	{
		// Token: 0x0600003F RID: 63 RVA: 0x00007738 File Offset: 0x00006738
		public TextRegex()
		{
			this.pattern = "\\G[^<]+";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new TextRegexFactory9();
			this.capsize = 1;
			base.InitializeReferences();
		}
	}
}
