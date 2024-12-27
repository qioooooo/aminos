using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000049 RID: 73
	internal class TagRegex40 : Regex
	{
		// Token: 0x060000A8 RID: 168 RVA: 0x00010730 File Offset: 0x0000F730
		public TagRegex40()
		{
			this.pattern = "\\G<(?<tagname>[\\w:\\.]+)(\\s+(?<attrname>\\w[-\\w:]*)(\\s*=\\s*\"(?<attrval>[^\"]*)\"|\\s*=\\s*'(?<attrval>[^']*)'|\\s*=\\s*(?<attrval><%#.*?%>)|\\s*=\\s*(?<attrval>[^\\s=\"'/>]*)|(?<attrval>\\s*?)))*\\s*(?<empty>/)?>";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new TagRegex40Factory24();
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
