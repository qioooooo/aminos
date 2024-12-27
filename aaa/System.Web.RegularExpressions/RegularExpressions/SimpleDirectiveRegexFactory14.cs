using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200002A RID: 42
	internal class SimpleDirectiveRegexFactory14 : RegexRunnerFactory
	{
		// Token: 0x06000060 RID: 96 RVA: 0x00009EF0 File Offset: 0x00008EF0
		public override RegexRunner CreateInstance()
		{
			return new SimpleDirectiveRegexRunner14();
		}
	}
}
