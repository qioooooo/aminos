using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200002D RID: 45
	internal class DataBindRegexFactory15 : RegexRunnerFactory
	{
		// Token: 0x06000067 RID: 103 RVA: 0x0000A718 File Offset: 0x00009718
		public override RegexRunner CreateInstance()
		{
			return new DataBindRegexRunner15();
		}
	}
}
