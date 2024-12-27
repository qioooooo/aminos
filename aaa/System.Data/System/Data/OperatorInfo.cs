using System;

namespace System.Data
{
	// Token: 0x020001AD RID: 429
	internal sealed class OperatorInfo
	{
		// Token: 0x060018C5 RID: 6341 RVA: 0x0023BEA8 File Offset: 0x0023B2A8
		internal OperatorInfo(Nodes type, int op, int pri)
		{
			this.type = type;
			this.op = op;
			this.priority = pri;
		}

		// Token: 0x04000D9E RID: 3486
		internal Nodes type;

		// Token: 0x04000D9F RID: 3487
		internal int op;

		// Token: 0x04000DA0 RID: 3488
		internal int priority;
	}
}
