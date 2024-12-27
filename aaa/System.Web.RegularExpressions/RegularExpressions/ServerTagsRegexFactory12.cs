using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000024 RID: 36
	internal class ServerTagsRegexFactory12 : RegexRunnerFactory
	{
		// Token: 0x06000052 RID: 82 RVA: 0x0000839C File Offset: 0x0000739C
		public override RegexRunner CreateInstance()
		{
			return new ServerTagsRegexRunner12();
		}
	}
}
