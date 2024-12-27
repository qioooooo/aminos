using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000013 RID: 19
	public class DatabindExprRegex : Regex
	{
		// Token: 0x0600002A RID: 42 RVA: 0x000062A4 File Offset: 0x000052A4
		public DatabindExprRegex()
		{
			this.pattern = "\\G<%#(?<code>.*?)?%>";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new DatabindExprRegexFactory6();
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
