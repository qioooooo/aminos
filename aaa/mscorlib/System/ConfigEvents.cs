using System;

namespace System
{
	// Token: 0x0200007E RID: 126
	[Serializable]
	internal enum ConfigEvents
	{
		// Token: 0x0400022E RID: 558
		StartDocument,
		// Token: 0x0400022F RID: 559
		StartDTD,
		// Token: 0x04000230 RID: 560
		EndDTD,
		// Token: 0x04000231 RID: 561
		StartDTDSubset,
		// Token: 0x04000232 RID: 562
		EndDTDSubset,
		// Token: 0x04000233 RID: 563
		EndProlog,
		// Token: 0x04000234 RID: 564
		StartEntity,
		// Token: 0x04000235 RID: 565
		EndEntity,
		// Token: 0x04000236 RID: 566
		EndDocument,
		// Token: 0x04000237 RID: 567
		DataAvailable,
		// Token: 0x04000238 RID: 568
		LastEvent = 9
	}
}
