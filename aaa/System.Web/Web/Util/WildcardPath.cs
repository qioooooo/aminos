using System;
using System.Text.RegularExpressions;

namespace System.Web.Util
{
	// Token: 0x0200079C RID: 1948
	internal abstract class WildcardPath : Wildcard
	{
		// Token: 0x06005D6A RID: 23914 RVA: 0x001763EB File Offset: 0x001753EB
		internal WildcardPath(string pattern, bool caseInsensitive)
			: base(pattern, caseInsensitive)
		{
		}

		// Token: 0x06005D6B RID: 23915 RVA: 0x001763F5 File Offset: 0x001753F5
		internal bool IsSuffix(string input)
		{
			this.EnsureSuffix();
			return this._suffix.IsMatch(input);
		}

		// Token: 0x06005D6C RID: 23916 RVA: 0x00176409 File Offset: 0x00175409
		protected void EnsureSuffix()
		{
			if (this._suffix != null)
			{
				return;
			}
			this._suffix = this.SuffixFromWildcard(this._pattern, this._caseInsensitive);
		}

		// Token: 0x06005D6D RID: 23917
		protected abstract Regex SuffixFromWildcard(string pattern, bool caseInsensitive);

		// Token: 0x06005D6E RID: 23918
		protected abstract Regex[][] DirsFromWildcard(string pattern);

		// Token: 0x06005D6F RID: 23919
		protected abstract string[] SplitDirs(string input);

		// Token: 0x040031DB RID: 12763
		private Regex _suffix;
	}
}
