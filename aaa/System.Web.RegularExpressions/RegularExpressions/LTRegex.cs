using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000022 RID: 34
	public class LTRegex : Regex
	{
		// Token: 0x0600004D RID: 77 RVA: 0x00007C74 File Offset: 0x00006C74
		public LTRegex()
		{
			this.pattern = "<[^%]";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new LTRegexFactory11();
			this.capsize = 1;
			base.InitializeReferences();
		}
	}
}
