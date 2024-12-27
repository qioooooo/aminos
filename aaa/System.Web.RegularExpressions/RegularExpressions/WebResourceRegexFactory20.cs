using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200003C RID: 60
	internal class WebResourceRegexFactory20 : RegexRunnerFactory
	{
		// Token: 0x0600008A RID: 138 RVA: 0x0000DD30 File Offset: 0x0000CD30
		public override RegexRunner CreateInstance()
		{
			return new WebResourceRegexRunner20();
		}
	}
}
