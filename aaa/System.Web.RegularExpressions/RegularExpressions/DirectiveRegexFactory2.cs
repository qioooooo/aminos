using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000006 RID: 6
	internal class DirectiveRegexFactory2 : RegexRunnerFactory
	{
		// Token: 0x0600000C RID: 12 RVA: 0x00004CB4 File Offset: 0x00003CB4
		public override RegexRunner CreateInstance()
		{
			return new DirectiveRegexRunner2();
		}
	}
}
