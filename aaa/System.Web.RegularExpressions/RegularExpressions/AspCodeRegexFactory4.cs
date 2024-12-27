using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200000C RID: 12
	internal class AspCodeRegexFactory4 : RegexRunnerFactory
	{
		// Token: 0x0600001A RID: 26 RVA: 0x00005734 File Offset: 0x00004734
		public override RegexRunner CreateInstance()
		{
			return new AspCodeRegexRunner4();
		}
	}
}
