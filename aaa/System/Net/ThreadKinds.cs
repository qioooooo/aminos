using System;

namespace System.Net
{
	// Token: 0x020004EB RID: 1259
	[Flags]
	internal enum ThreadKinds
	{
		// Token: 0x040026A5 RID: 9893
		Unknown = 0,
		// Token: 0x040026A6 RID: 9894
		User = 1,
		// Token: 0x040026A7 RID: 9895
		System = 2,
		// Token: 0x040026A8 RID: 9896
		Sync = 4,
		// Token: 0x040026A9 RID: 9897
		Async = 8,
		// Token: 0x040026AA RID: 9898
		Timer = 16,
		// Token: 0x040026AB RID: 9899
		CompletionPort = 32,
		// Token: 0x040026AC RID: 9900
		Worker = 64,
		// Token: 0x040026AD RID: 9901
		Finalization = 128,
		// Token: 0x040026AE RID: 9902
		Other = 256,
		// Token: 0x040026AF RID: 9903
		OwnerMask = 3,
		// Token: 0x040026B0 RID: 9904
		SyncMask = 12,
		// Token: 0x040026B1 RID: 9905
		SourceMask = 496,
		// Token: 0x040026B2 RID: 9906
		SafeSources = 352,
		// Token: 0x040026B3 RID: 9907
		ThreadPool = 96
	}
}
