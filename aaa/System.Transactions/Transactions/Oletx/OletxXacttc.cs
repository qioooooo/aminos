using System;

namespace System.Transactions.Oletx
{
	// Token: 0x0200007D RID: 125
	[Flags]
	internal enum OletxXacttc
	{
		// Token: 0x040001A1 RID: 417
		XACTTC_NONE = 0,
		// Token: 0x040001A2 RID: 418
		XACTTC_SYNC_PHASEONE = 1,
		// Token: 0x040001A3 RID: 419
		XACTTC_SYNC_PHASETWO = 2,
		// Token: 0x040001A4 RID: 420
		XACTTC_SYNC = 2,
		// Token: 0x040001A5 RID: 421
		XACTTC_ASYNC_PHASEONE = 4,
		// Token: 0x040001A6 RID: 422
		XACTTC_ASYNC = 4
	}
}
