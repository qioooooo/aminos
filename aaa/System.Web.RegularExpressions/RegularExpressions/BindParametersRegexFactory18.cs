using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000036 RID: 54
	internal class BindParametersRegexFactory18 : RegexRunnerFactory
	{
		// Token: 0x0600007C RID: 124 RVA: 0x0000CC04 File Offset: 0x0000BC04
		public override RegexRunner CreateInstance()
		{
			return new BindParametersRegexRunner18();
		}
	}
}
