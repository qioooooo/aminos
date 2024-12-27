using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200000D RID: 13
	public class AspCodeRegex : Regex
	{
		// Token: 0x0600001C RID: 28 RVA: 0x0000575C File Offset: 0x0000475C
		public AspCodeRegex()
		{
			this.pattern = "\\G<%(?!@)(?<code>.*?)%>";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new AspCodeRegexFactory4();
			this.capnames = new Hashtable();
			this.capnames.Add("code", 1);
			this.capnames.Add("0", 0);
			this.capslist = new string[2];
			this.capslist[0] = "0";
			this.capslist[1] = "code";
			this.capsize = 2;
			base.InitializeReferences();
		}
	}
}
