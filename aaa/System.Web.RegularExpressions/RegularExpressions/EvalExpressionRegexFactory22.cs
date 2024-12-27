using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000042 RID: 66
	internal class EvalExpressionRegexFactory22 : RegexRunnerFactory
	{
		// Token: 0x06000098 RID: 152 RVA: 0x0000E704 File Offset: 0x0000D704
		public override RegexRunner CreateInstance()
		{
			return new EvalExpressionRegexRunner22();
		}
	}
}
