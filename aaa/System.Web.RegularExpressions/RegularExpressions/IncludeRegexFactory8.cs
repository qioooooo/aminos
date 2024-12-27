using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000018 RID: 24
	internal class IncludeRegexFactory8 : RegexRunnerFactory
	{
		// Token: 0x06000036 RID: 54 RVA: 0x00007380 File Offset: 0x00006380
		public override RegexRunner CreateInstance()
		{
			return new IncludeRegexRunner8();
		}
	}
}
