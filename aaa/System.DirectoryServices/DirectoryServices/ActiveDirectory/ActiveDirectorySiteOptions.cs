using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000079 RID: 121
	[Flags]
	public enum ActiveDirectorySiteOptions
	{
		// Token: 0x0400032A RID: 810
		None = 0,
		// Token: 0x0400032B RID: 811
		AutoTopologyDisabled = 1,
		// Token: 0x0400032C RID: 812
		TopologyCleanupDisabled = 2,
		// Token: 0x0400032D RID: 813
		AutoMinimumHopDisabled = 4,
		// Token: 0x0400032E RID: 814
		StaleServerDetectDisabled = 8,
		// Token: 0x0400032F RID: 815
		AutoInterSiteTopologyDisabled = 16,
		// Token: 0x04000330 RID: 816
		GroupMembershipCachingEnabled = 32,
		// Token: 0x04000331 RID: 817
		ForceKccWindows2003Behavior = 64,
		// Token: 0x04000332 RID: 818
		UseWindows2000IstgElection = 128,
		// Token: 0x04000333 RID: 819
		RandomBridgeHeaderServerSelectionDisabled = 256,
		// Token: 0x04000334 RID: 820
		UseHashingForReplicationSchedule = 512,
		// Token: 0x04000335 RID: 821
		RedundantServerTopologyEnabled = 1024
	}
}
