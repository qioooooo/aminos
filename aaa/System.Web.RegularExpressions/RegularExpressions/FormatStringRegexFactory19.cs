using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000039 RID: 57
	internal class FormatStringRegexFactory19 : RegexRunnerFactory
	{
		// Token: 0x06000083 RID: 131 RVA: 0x0000D490 File Offset: 0x0000C490
		public override RegexRunner CreateInstance()
		{
			return new FormatStringRegexRunner19();
		}
	}
}
