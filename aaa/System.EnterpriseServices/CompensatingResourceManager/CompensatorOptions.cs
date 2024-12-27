using System;

namespace System.EnterpriseServices.CompensatingResourceManager
{
	// Token: 0x020000A5 RID: 165
	[Flags]
	[Serializable]
	public enum CompensatorOptions
	{
		// Token: 0x040001D0 RID: 464
		PreparePhase = 1,
		// Token: 0x040001D1 RID: 465
		CommitPhase = 2,
		// Token: 0x040001D2 RID: 466
		AbortPhase = 4,
		// Token: 0x040001D3 RID: 467
		AllPhases = 7,
		// Token: 0x040001D4 RID: 468
		FailIfInDoubtsRemain = 16
	}
}
