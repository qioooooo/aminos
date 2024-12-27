using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200000F RID: 15
	internal class AspExprRegexFactory5 : RegexRunnerFactory
	{
		// Token: 0x06000021 RID: 33 RVA: 0x00005D30 File Offset: 0x00004D30
		public override RegexRunner CreateInstance()
		{
			return new AspExprRegexRunner5();
		}
	}
}
