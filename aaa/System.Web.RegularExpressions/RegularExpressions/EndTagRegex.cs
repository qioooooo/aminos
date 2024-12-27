using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200000A RID: 10
	public class EndTagRegex : Regex
	{
		// Token: 0x06000015 RID: 21 RVA: 0x0000525C File Offset: 0x0000425C
		public EndTagRegex()
		{
			this.pattern = "\\G</(?<tagname>[\\w:\\.]+)\\s*>";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new EndTagRegexFactory3();
			this.capnames = new Hashtable();
			this.capnames.Add("0", 0);
			this.capnames.Add("tagname", 1);
			this.capslist = new string[2];
			this.capslist[0] = "0";
			this.capslist[1] = "tagname";
			this.capsize = 2;
			base.InitializeReferences();
		}
	}
}
