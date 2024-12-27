using System;
using System.Text.RegularExpressions;

namespace System.Web.Util
{
	// Token: 0x0200079B RID: 1947
	internal class Wildcard
	{
		// Token: 0x06005D65 RID: 23909 RVA: 0x00176294 File Offset: 0x00175294
		internal Wildcard(string pattern, bool caseInsensitive)
		{
			this._pattern = pattern;
			this._caseInsensitive = caseInsensitive;
		}

		// Token: 0x06005D66 RID: 23910 RVA: 0x001762AC File Offset: 0x001752AC
		internal bool IsMatch(string input)
		{
			this.EnsureRegex();
			return this._regex.IsMatch(input);
		}

		// Token: 0x06005D67 RID: 23911 RVA: 0x001762CD File Offset: 0x001752CD
		protected void EnsureRegex()
		{
			if (this._regex != null)
			{
				return;
			}
			this._regex = this.RegexFromWildcard(this._pattern, this._caseInsensitive);
		}

		// Token: 0x06005D68 RID: 23912 RVA: 0x001762F0 File Offset: 0x001752F0
		protected virtual Regex RegexFromWildcard(string pattern, bool caseInsensitive)
		{
			RegexOptions regexOptions;
			if (pattern.Length > 0 && pattern[0] == '*')
			{
				regexOptions = RegexOptions.Singleline | RegexOptions.RightToLeft;
			}
			else
			{
				regexOptions = RegexOptions.Singleline;
			}
			if (caseInsensitive)
			{
				regexOptions |= RegexOptions.IgnoreCase | RegexOptions.CultureInvariant;
			}
			pattern = Wildcard.metaRegex.Replace(pattern, "\\$0");
			pattern = Wildcard.questRegex.Replace(pattern, ".");
			pattern = Wildcard.starRegex.Replace(pattern, ".*");
			pattern = Wildcard.commaRegex.Replace(pattern, "\\z|\\A");
			return new Regex("\\A" + pattern + "\\z", regexOptions);
		}

		// Token: 0x040031D2 RID: 12754
		internal string _pattern;

		// Token: 0x040031D3 RID: 12755
		internal bool _caseInsensitive;

		// Token: 0x040031D4 RID: 12756
		internal Regex _regex;

		// Token: 0x040031D5 RID: 12757
		protected static Regex metaRegex = new Regex("[\\+\\{\\\\\\[\\|\\(\\)\\.\\^\\$]");

		// Token: 0x040031D6 RID: 12758
		protected static Regex questRegex = new Regex("\\?");

		// Token: 0x040031D7 RID: 12759
		protected static Regex starRegex = new Regex("\\*");

		// Token: 0x040031D8 RID: 12760
		protected static Regex commaRegex = new Regex(",");

		// Token: 0x040031D9 RID: 12761
		protected static Regex slashRegex = new Regex("(?=/)");

		// Token: 0x040031DA RID: 12762
		protected static Regex backslashRegex = new Regex("(?=[\\\\:])");
	}
}
