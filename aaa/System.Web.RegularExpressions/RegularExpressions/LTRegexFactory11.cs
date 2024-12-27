using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000021 RID: 33
	internal class LTRegexFactory11 : RegexRunnerFactory
	{
		// Token: 0x0600004B RID: 75 RVA: 0x00007C4C File Offset: 0x00006C4C
		public override RegexRunner CreateInstance()
		{
			return new LTRegexRunner11();
		}
	}
}
