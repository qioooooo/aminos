using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200002E RID: 46
	public class DataBindRegex : Regex
	{
		// Token: 0x06000069 RID: 105 RVA: 0x0000A740 File Offset: 0x00009740
		public DataBindRegex()
		{
			this.pattern = "\\G\\s*<%\\s*?#(?<code>.*?)?%>\\s*\\z";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new DataBindRegexFactory15();
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
