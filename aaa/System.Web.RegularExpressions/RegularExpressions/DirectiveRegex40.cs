using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200004C RID: 76
	internal class DirectiveRegex40 : Regex
	{
		// Token: 0x060000AF RID: 175 RVA: 0x00011C24 File Offset: 0x00010C24
		public DirectiveRegex40()
		{
			this.pattern = "\\G<%\\s*@(\\s*(?<attrname>\\w[\\w:]*(?=\\W))(\\s*(?<equal>=)\\s*\"(?<attrval>[^\"]*)\"|\\s*(?<equal>=)\\s*'(?<attrval>[^']*)'|\\s*(?<equal>=)\\s*(?<attrval>[^\\s\"'%>]*)|(?<equal>)(?<attrval>\\s*?)))*\\s*?%>";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new DirectiveRegex40Factory25();
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
