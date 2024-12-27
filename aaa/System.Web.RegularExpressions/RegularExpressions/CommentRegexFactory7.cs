using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000015 RID: 21
	internal class CommentRegexFactory7 : RegexRunnerFactory
	{
		// Token: 0x0600002F RID: 47 RVA: 0x000067E8 File Offset: 0x000057E8
		public override RegexRunner CreateInstance()
		{
			return new CommentRegexRunner7();
		}
	}
}
