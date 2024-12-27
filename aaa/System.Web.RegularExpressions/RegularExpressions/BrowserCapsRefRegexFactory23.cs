using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000045 RID: 69
	internal class BrowserCapsRefRegexFactory23 : RegexRunnerFactory
	{
		// Token: 0x0600009F RID: 159 RVA: 0x0000EF4C File Offset: 0x0000DF4C
		public override RegexRunner CreateInstance()
		{
			return new BrowserCapsRefRegexRunner23();
		}
	}
}
