using System;

namespace Microsoft.JScript
{
	// Token: 0x020000B9 RID: 185
	internal class OpListItem
	{
		// Token: 0x06000884 RID: 2180 RVA: 0x00040DF9 File Offset: 0x0003FDF9
		internal OpListItem(JSToken op, OpPrec prec, OpListItem prev)
		{
			this._prev = prev;
			this._operator = op;
			this._prec = prec;
		}

		// Token: 0x0400047E RID: 1150
		internal OpListItem _prev;

		// Token: 0x0400047F RID: 1151
		internal JSToken _operator;

		// Token: 0x04000480 RID: 1152
		internal OpPrec _prec;
	}
}
