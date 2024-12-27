using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000046 RID: 70
	internal class BrowserCapsRefRegex : Regex
	{
		// Token: 0x060000A1 RID: 161 RVA: 0x0000EF74 File Offset: 0x0000DF74
		public BrowserCapsRefRegex()
		{
			this.pattern = "\\$(?:\\{(?<name>\\w+)\\})";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new BrowserCapsRefRegexFactory23();
			this.capnames = new Hashtable();
			this.capnames.Add("name", 1);
			this.capnames.Add("0", 0);
			this.capslist = new string[2];
			this.capslist[0] = "0";
			this.capslist[1] = "name";
			this.capsize = 2;
			base.InitializeReferences();
		}
	}
}
