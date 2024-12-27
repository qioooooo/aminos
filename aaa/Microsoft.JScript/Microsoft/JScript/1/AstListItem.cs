using System;

namespace Microsoft.JScript
{
	// Token: 0x020000B8 RID: 184
	internal class AstListItem
	{
		// Token: 0x06000883 RID: 2179 RVA: 0x00040DE3 File Offset: 0x0003FDE3
		internal AstListItem(AST term, AstListItem prev)
		{
			this._prev = prev;
			this._term = term;
		}

		// Token: 0x0400047C RID: 1148
		internal AstListItem _prev;

		// Token: 0x0400047D RID: 1149
		internal AST _term;
	}
}
