using System;

namespace System.Text.RegularExpressions
{
	// Token: 0x0200001F RID: 31
	internal sealed class RegexPrefix
	{
		// Token: 0x0600014A RID: 330 RVA: 0x0000B204 File Offset: 0x0000A204
		internal RegexPrefix(string prefix, bool ci)
		{
			this._prefix = prefix;
			this._caseInsensitive = ci;
		}

		// Token: 0x1700002A RID: 42
		// (get) Token: 0x0600014B RID: 331 RVA: 0x0000B21A File Offset: 0x0000A21A
		internal string Prefix
		{
			get
			{
				return this._prefix;
			}
		}

		// Token: 0x1700002B RID: 43
		// (get) Token: 0x0600014C RID: 332 RVA: 0x0000B222 File Offset: 0x0000A222
		internal bool CaseInsensitive
		{
			get
			{
				return this._caseInsensitive;
			}
		}

		// Token: 0x1700002C RID: 44
		// (get) Token: 0x0600014D RID: 333 RVA: 0x0000B22A File Offset: 0x0000A22A
		internal static RegexPrefix Empty
		{
			get
			{
				return RegexPrefix._empty;
			}
		}

		// Token: 0x04000726 RID: 1830
		internal string _prefix;

		// Token: 0x04000727 RID: 1831
		internal bool _caseInsensitive;

		// Token: 0x04000728 RID: 1832
		internal static RegexPrefix _empty = new RegexPrefix(string.Empty, false);
	}
}
