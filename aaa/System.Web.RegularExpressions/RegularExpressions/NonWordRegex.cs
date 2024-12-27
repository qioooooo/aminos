using System;
using System.Text.RegularExpressions;

namespace System.Web.RegularExpressions
{
	// Token: 0x02000040 RID: 64
	internal class NonWordRegex : Regex
	{
		// Token: 0x06000093 RID: 147 RVA: 0x0000E040 File Offset: 0x0000D040
		public NonWordRegex()
		{
			this.pattern = "\\W";
			this.roptions = RegexOptions.Multiline | RegexOptions.Singleline;
			this.factory = new NonWordRegexFactory21();
			this.capsize = 1;
			base.InitializeReferences();
		}
	}
}
