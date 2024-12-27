using System;
using System.Collections;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000019 RID: 25
	public class IncludeRegex : Regex
	{
		// Token: 0x06000038 RID: 56 RVA: 0x000073A8 File Offset: 0x000063A8
		public IncludeRegex()
		{
			this.pattern = "\\G<!--\\s*#(?i:include)\\s*(?<pathtype>[\\w]+)\\s*=\\s*[\"']?(?<filename>[^\\\"']*?)[\"']?\\s*-->";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new IncludeRegexFactory8();
			this.capnames = new Hashtable();
			this.capnames.Add("pathtype", 1);
			this.capnames.Add("filename", 2);
			this.capnames.Add("0", 0);
			this.capslist = new string[3];
			this.capslist[0] = "0";
			this.capslist[1] = "pathtype";
			this.capslist[2] = "filename";
			this.capsize = 3;
			base.InitializeReferences();
		}
	}
}
