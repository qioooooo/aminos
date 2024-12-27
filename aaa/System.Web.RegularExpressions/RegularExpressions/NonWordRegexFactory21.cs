using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200003F RID: 63
	internal class NonWordRegexFactory21 : RegexRunnerFactory
	{
		// Token: 0x06000091 RID: 145 RVA: 0x0000E018 File Offset: 0x0000D018
		public override RegexRunner CreateInstance()
		{
			return new NonWordRegexRunner21();
		}
	}
}
