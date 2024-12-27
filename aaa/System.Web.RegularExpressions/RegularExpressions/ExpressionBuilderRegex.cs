using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000031 RID: 49
	internal class ExpressionBuilderRegex : Regex
	{
		// Token: 0x06000070 RID: 112 RVA: 0x0000AFE8 File Offset: 0x00009FE8
		public ExpressionBuilderRegex()
		{
			this.pattern = "\\G\\s*<%\\s*\\$\\s*(?<code>.*)?%>\\s*\\z";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new ExpressionBuilderRegexFactory16();
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
