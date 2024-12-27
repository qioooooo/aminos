using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200003D RID: 61
	internal class WebResourceRegex : Regex
	{
		// Token: 0x0600008C RID: 140 RVA: 0x0000DD58 File Offset: 0x0000CD58
		public WebResourceRegex()
		{
			this.pattern = "<%\\s*=\\s*WebResource\\(\"(?<resourceName>[^\"]*)\"\\)\\s*%>";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new WebResourceRegexFactory20();
			this.capnames = new Hashtable();
			this.capnames.Add("resourceName", 1);
			this.capnames.Add("0", 0);
			this.capslist = new string[2];
			this.capslist[0] = "0";
			this.capslist[1] = "resourceName";
			this.capsize = 2;
			base.InitializeReferences();
		}
	}
}
