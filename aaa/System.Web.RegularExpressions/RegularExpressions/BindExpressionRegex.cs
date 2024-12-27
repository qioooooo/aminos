using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000034 RID: 52
	internal class BindExpressionRegex : Regex
	{
		// Token: 0x06000077 RID: 119 RVA: 0x0000B738 File Offset: 0x0000A738
		public BindExpressionRegex()
		{
			this.pattern = "^\\s*bind\\s*\\((?<params>.*)\\)\\s*\\z";
			this.roptions = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.CultureInvariant;
			this.factory = new BindExpressionRegexFactory17();
			this.capnames = new Hashtable();
			this.capnames.Add("params", 1);
			this.capnames.Add("0", 0);
			this.capslist = new string[2];
			this.capslist[0] = "0";
			this.capslist[1] = "params";
			this.capsize = 2;
			base.InitializeReferences();
		}
	}
}
