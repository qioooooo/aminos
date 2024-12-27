using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200001E RID: 30
	internal class GTRegexFactory10 : RegexRunnerFactory
	{
		// Token: 0x06000044 RID: 68 RVA: 0x000079A8 File Offset: 0x000069A8
		public override RegexRunner CreateInstance()
		{
			return new GTRegexRunner10();
		}
	}
}
