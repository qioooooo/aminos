using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000012 RID: 18
	internal class DatabindExprRegexFactory6 : RegexRunnerFactory
	{
		// Token: 0x06000028 RID: 40 RVA: 0x0000627C File Offset: 0x0000527C
		public override RegexRunner CreateInstance()
		{
			return new DatabindExprRegexRunner6();
		}
	}
}
