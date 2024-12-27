using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000030 RID: 48
	internal class ExpressionBuilderRegexFactory16 : RegexRunnerFactory
	{
		// Token: 0x0600006E RID: 110 RVA: 0x0000AFC0 File Offset: 0x00009FC0
		public override RegexRunner CreateInstance()
		{
			return new ExpressionBuilderRegexRunner16();
		}
	}
}
