using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000028 RID: 40
	public class RunatServerRegex : Regex
	{
		// Token: 0x0600005B RID: 91 RVA: 0x000089D4 File Offset: 0x000079D4
		public RunatServerRegex()
		{
			this.pattern = "runat\\W*server";
			this.roptions = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.CultureInvariant;
			this.factory = new RunatServerRegexFactory13();
			this.capsize = 1;
			base.InitializeReferences();
		}
	}
}
