using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x0200004B RID: 75
	internal class DirectiveRegex40Factory25 : RegexRunnerFactory
	{
		// Token: 0x060000AD RID: 173 RVA: 0x00011BFC File Offset: 0x00010BFC
		public override RegexRunner CreateInstance()
		{
			return new DirectiveRegex40Runner25();
		}
	}
}
