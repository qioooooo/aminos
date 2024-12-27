using System;
using System.Text.RegularExpressions;

namespace Microsoft.JScript
{
	// Token: 0x0200010A RID: 266
	internal class ReplaceWithString : RegExpReplace
	{
		// Token: 0x06000B40 RID: 2880 RVA: 0x00055B83 File Offset: 0x00054B83
		internal ReplaceWithString(string replaceString)
		{
			this.replaceString = replaceString;
		}

		// Token: 0x06000B41 RID: 2881 RVA: 0x00055B92 File Offset: 0x00054B92
		internal override string Evaluate(Match match)
		{
			this.lastMatch = match;
			return match.Result(this.replaceString);
		}

		// Token: 0x040006CD RID: 1741
		private string replaceString;
	}
}
