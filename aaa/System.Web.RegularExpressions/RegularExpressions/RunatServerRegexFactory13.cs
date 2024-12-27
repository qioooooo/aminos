using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000027 RID: 39
	internal class RunatServerRegexFactory13 : RegexRunnerFactory
	{
		// Token: 0x06000059 RID: 89 RVA: 0x000089AC File Offset: 0x000079AC
		public override RegexRunner CreateInstance()
		{
			return new RunatServerRegexRunner13();
		}
	}
}
