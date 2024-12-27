using System;

namespace System.Diagnostics
{
	// Token: 0x0200079C RID: 1948
	public enum ThreadWaitReason
	{
		// Token: 0x040034AC RID: 13484
		Executive,
		// Token: 0x040034AD RID: 13485
		FreePage,
		// Token: 0x040034AE RID: 13486
		PageIn,
		// Token: 0x040034AF RID: 13487
		SystemAllocation,
		// Token: 0x040034B0 RID: 13488
		ExecutionDelay,
		// Token: 0x040034B1 RID: 13489
		Suspended,
		// Token: 0x040034B2 RID: 13490
		UserRequest,
		// Token: 0x040034B3 RID: 13491
		EventPairHigh,
		// Token: 0x040034B4 RID: 13492
		EventPairLow,
		// Token: 0x040034B5 RID: 13493
		LpcReceive,
		// Token: 0x040034B6 RID: 13494
		LpcReply,
		// Token: 0x040034B7 RID: 13495
		VirtualMemory,
		// Token: 0x040034B8 RID: 13496
		PageOut,
		// Token: 0x040034B9 RID: 13497
		Unknown
	}
}
