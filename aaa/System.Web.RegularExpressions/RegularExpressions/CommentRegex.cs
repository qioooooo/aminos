using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000016 RID: 22
	public class CommentRegex : Regex
	{
		// Token: 0x06000031 RID: 49 RVA: 0x00006810 File Offset: 0x00005810
		public CommentRegex()
		{
			this.pattern = "\\G<%--(([^-]*)-)*?-%>";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new CommentRegexFactory7();
			this.capsize = 3;
			base.InitializeReferences();
		}
	}
}
