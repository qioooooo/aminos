using System;

namespace System.Security.Util
{
	// Token: 0x0200046D RID: 1133
	[Flags]
	[Serializable]
	internal enum QuickCacheEntryType
	{
		// Token: 0x0400174B RID: 5963
		FullTrustZoneMyComputer = 16777216,
		// Token: 0x0400174C RID: 5964
		FullTrustZoneIntranet = 33554432,
		// Token: 0x0400174D RID: 5965
		FullTrustZoneInternet = 67108864,
		// Token: 0x0400174E RID: 5966
		FullTrustZoneTrusted = 134217728,
		// Token: 0x0400174F RID: 5967
		FullTrustZoneUntrusted = 268435456,
		// Token: 0x04001750 RID: 5968
		FullTrustAll = 536870912
	}
}
