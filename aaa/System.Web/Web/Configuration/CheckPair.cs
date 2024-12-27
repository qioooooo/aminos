using System;
using System.Text.RegularExpressions;

namespace System.Web.Configuration
{
	// Token: 0x020001B8 RID: 440
	internal class CheckPair
	{
		// Token: 0x0600193B RID: 6459 RVA: 0x0007883C File Offset: 0x0007783C
		internal CheckPair(string header, string match, bool nonMatch)
		{
			this._header = header;
			this._match = match;
			this._nonMatch = nonMatch;
			new Regex(match);
		}

		// Token: 0x0600193C RID: 6460 RVA: 0x00078860 File Offset: 0x00077860
		internal CheckPair(string header, string match)
		{
			this._header = header;
			this._match = match;
			this._nonMatch = false;
			new Regex(match);
		}

		// Token: 0x17000487 RID: 1159
		// (get) Token: 0x0600193D RID: 6461 RVA: 0x00078884 File Offset: 0x00077884
		public string Header
		{
			get
			{
				return this._header;
			}
		}

		// Token: 0x17000488 RID: 1160
		// (get) Token: 0x0600193E RID: 6462 RVA: 0x0007888C File Offset: 0x0007788C
		public string MatchString
		{
			get
			{
				return this._match;
			}
		}

		// Token: 0x17000489 RID: 1161
		// (get) Token: 0x0600193F RID: 6463 RVA: 0x00078894 File Offset: 0x00077894
		public bool NonMatch
		{
			get
			{
				return this._nonMatch;
			}
		}

		// Token: 0x04001724 RID: 5924
		private string _header;

		// Token: 0x04001725 RID: 5925
		private string _match;

		// Token: 0x04001726 RID: 5926
		private bool _nonMatch;
	}
}
