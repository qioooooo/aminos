using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200001B RID: 27
	internal class TextRegexFactory9 : RegexRunnerFactory
	{
		// Token: 0x0600003D RID: 61 RVA: 0x00007710 File Offset: 0x00006710
		public override RegexRunner CreateInstance()
		{
			return new TextRegexRunner9();
		}
	}
}
