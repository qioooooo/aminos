using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000010 RID: 16
	public class AspExprRegex : Regex
	{
		// Token: 0x06000023 RID: 35 RVA: 0x00005D58 File Offset: 0x00004D58
		public AspExprRegex()
		{
			this.pattern = "\\G<%\\s*?=(?<code>.*?)?%>";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new AspExprRegexFactory5();
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
