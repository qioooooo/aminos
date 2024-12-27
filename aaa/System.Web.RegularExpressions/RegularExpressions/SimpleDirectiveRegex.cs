using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200002B RID: 43
	public class SimpleDirectiveRegex : Regex
	{
		// Token: 0x06000062 RID: 98 RVA: 0x00009F18 File Offset: 0x00008F18
		public SimpleDirectiveRegex()
		{
			this.pattern = "<%\\s*@(\\s*(?<attrname>\\w[\\w:]*(?=\\W))(\\s*(?<equal>=)\\s*\"(?<attrval>[^\"]*)\"|\\s*(?<equal>=)\\s*'(?<attrval>[^']*)'|\\s*(?<equal>=)\\s*(?<attrval>[^\\s%>]*)|(?<equal>)(?<attrval>\\s*?)))*\\s*?%>";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new SimpleDirectiveRegexFactory14();
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
