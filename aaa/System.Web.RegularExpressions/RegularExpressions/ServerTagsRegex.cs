using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000025 RID: 37
	public class ServerTagsRegex : Regex
	{
		// Token: 0x06000054 RID: 84 RVA: 0x000083C4 File Offset: 0x000073C4
		public ServerTagsRegex()
		{
			this.pattern = "<%(?![#$])(([^%]*)%)*?>";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new ServerTagsRegexFactory12();
			this.capsize = 3;
			base.InitializeReferences();
		}
	}
}
