using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000004 RID: 4
	public class TagRegex : Regex
	{
		// Token: 0x06000007 RID: 7 RVA: 0x000037E8 File Offset: 0x000027E8
		public TagRegex()
		{
			this.pattern = "\\G<(?<tagname>[\\w:\\.]+)(\\s+(?<attrname>\\w[-\\w:]*)(\\s*=\\s*\"(?<attrval>[^\"]*)\"|\\s*=\\s*'(?<attrval>[^']*)'|\\s*=\\s*(?<attrval><%#.*?%>)|\\s*=\\s*(?<attrval>[^\\s=/>]*)|(?<attrval>\\s*?)))*\\s*(?<empty>/)?>";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new TagRegexFactory1();
			this.capnames = new Hashtable();
			this.capnames.Add("attrval", 5);
			this.capnames.Add("empty", 6);
			this.capnames.Add("1", 1);
			this.capnames.Add("0", 0);
			this.capnames.Add("tagname", 3);
			this.capnames.Add("2", 2);
			this.capnames.Add("attrname", 4);
			this.capslist = new string[7];
			this.capslist[0] = "0";
			this.capslist[1] = "1";
			this.capslist[2] = "2";
			this.capslist[3] = "tagname";
			this.capslist[4] = "attrname";
			this.capslist[5] = "attrval";
			this.capslist[6] = "empty";
			this.capsize = 7;
			base.InitializeReferences();
		}
	}
}
