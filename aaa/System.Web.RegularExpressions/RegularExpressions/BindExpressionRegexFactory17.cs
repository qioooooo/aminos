using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000033 RID: 51
	internal class BindExpressionRegexFactory17 : RegexRunnerFactory
	{
		// Token: 0x06000075 RID: 117 RVA: 0x0000B710 File Offset: 0x0000A710
		public override RegexRunner CreateInstance()
		{
			return new BindExpressionRegexRunner17();
		}
	}
}
