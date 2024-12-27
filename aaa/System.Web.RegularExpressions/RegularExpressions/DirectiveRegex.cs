using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000007 RID: 7
	public class DirectiveRegex : Regex
	{
		// Token: 0x0600000E RID: 14 RVA: 0x00004CDC File Offset: 0x00003CDC
		public DirectiveRegex()
		{
			this.pattern = "\\G<%\\s*@(\\s*(?<attrname>\\w[\\w:]*(?=\\W))(\\s*(?<equal>=)\\s*\"(?<attrval>[^\"]*)\"|\\s*(?<equal>=)\\s*'(?<attrval>[^']*)'|\\s*(?<equal>=)\\s*(?<attrval>[^\\s%>]*)|(?<equal>)(?<attrval>\\s*?)))*\\s*?%>";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new DirectiveRegexFactory2();
			this.capnames = new Hashtable();
			this.capnames.Add("attrval", 5);
			this.capnames.Add("2", 2);
			this.capnames.Add("0", 0);
			this.capnames.Add("1", 1);
			this.capnames.Add("equal", 4);
			this.capnames.Add("attrname", 3);
			this.capslist = new string[6];
			this.capslist[0] = "0";
			this.capslist[1] = "1";
			this.capslist[2] = "2";
			this.capslist[3] = "attrname";
			this.capslist[4] = "equal";
			this.capslist[5] = "attrval";
			this.capsize = 6;
			base.InitializeReferences();
		}
	}
}
