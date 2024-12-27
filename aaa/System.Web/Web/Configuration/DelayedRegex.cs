using System;
using System.Text.RegularExpressions;

namespace System.Web.Configuration
{
	// Token: 0x020001D3 RID: 467
	internal class DelayedRegex
	{
		// Token: 0x06001A41 RID: 6721 RVA: 0x0007B33E File Offset: 0x0007A33E
		internal DelayedRegex(string s)
		{
			this._regex = null;
			this._regstring = s;
		}

		// Token: 0x06001A42 RID: 6722 RVA: 0x0007B354 File Offset: 0x0007A354
		internal Match Match(string s)
		{
			this.EnsureRegex();
			return this._regex.Match(s);
		}

		// Token: 0x06001A43 RID: 6723 RVA: 0x0007B368 File Offset: 0x0007A368
		internal int GroupNumberFromName(string name)
		{
			this.EnsureRegex();
			return this._regex.GroupNumberFromName(name);
		}

		// Token: 0x06001A44 RID: 6724 RVA: 0x0007B37C File Offset: 0x0007A37C
		internal void EnsureRegex()
		{
			string regstring = this._regstring;
			if (this._regex == null)
			{
				this._regex = new Regex(regstring);
				this._regstring = null;
			}
		}

		// Token: 0x040017CD RID: 6093
		private string _regstring;

		// Token: 0x040017CE RID: 6094
		private Regex _regex;
	}
}
