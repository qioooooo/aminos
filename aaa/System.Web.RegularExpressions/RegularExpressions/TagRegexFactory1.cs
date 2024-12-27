using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000003 RID: 3
	internal class TagRegexFactory1 : RegexRunnerFactory
	{
		// Token: 0x06000005 RID: 5 RVA: 0x000037C0 File Offset: 0x000027C0
		public override RegexRunner CreateInstance()
		{
			return new TagRegexRunner1();
		}
	}
}
