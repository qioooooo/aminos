using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000043 RID: 67
	internal class EvalExpressionRegex : Regex
	{
		// Token: 0x0600009A RID: 154 RVA: 0x0000E72C File Offset: 0x0000D72C
		public EvalExpressionRegex()
		{
			this.pattern = "^\\s*eval\\s*\\((?<params>.*)\\)\\s*\\z";
			this.roptions = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline | RegexOptions.CultureInvariant;
			this.factory = new EvalExpressionRegexFactory22();
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
