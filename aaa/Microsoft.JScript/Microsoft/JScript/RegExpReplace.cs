using System;
using System.Text.RegularExpressions;

namespace Microsoft.JScript
{
	// Token: 0x02000108 RID: 264
	internal abstract class RegExpReplace
	{
		// Token: 0x06000B3C RID: 2876 RVA: 0x000559C9 File Offset: 0x000549C9
		internal RegExpReplace()
		{
			this.lastMatch = null;
		}

		// Token: 0x06000B3D RID: 2877
		internal abstract string Evaluate(Match match);

		// Token: 0x040006C8 RID: 1736
		internal Match lastMatch;
	}
}
