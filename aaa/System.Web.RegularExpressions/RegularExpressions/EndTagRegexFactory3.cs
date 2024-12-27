using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000009 RID: 9
	internal class EndTagRegexFactory3 : RegexRunnerFactory
	{
		// Token: 0x06000013 RID: 19 RVA: 0x00005234 File Offset: 0x00004234
		public override RegexRunner CreateInstance()
		{
			return new EndTagRegexRunner3();
		}
	}
}
