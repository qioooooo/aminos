using System;
using System.Collections;

namespace System.Text.RegularExpressions
{
	// Token: 0x02000026 RID: 38
	internal class MatchSparse : Match
	{
		// Token: 0x060001B6 RID: 438 RVA: 0x0000D85E File Offset: 0x0000C85E
		internal MatchSparse(Regex regex, Hashtable caps, int capcount, string text, int begpos, int len, int startpos)
			: base(regex, capcount, text, begpos, len, startpos)
		{
			this._caps = caps;
		}

		// Token: 0x17000039 RID: 57
		// (get) Token: 0x060001B7 RID: 439 RVA: 0x0000D877 File Offset: 0x0000C877
		public override GroupCollection Groups
		{
			get
			{
				if (this._groupcoll == null)
				{
					this._groupcoll = new GroupCollection(this, this._caps);
				}
				return this._groupcoll;
			}
		}

		// Token: 0x04000755 RID: 1877
		internal new Hashtable _caps;
	}
}
