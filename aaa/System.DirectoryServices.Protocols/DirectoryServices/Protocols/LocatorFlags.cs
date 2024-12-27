using System;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000086 RID: 134
	[Flags]
	public enum LocatorFlags : long
	{
		// Token: 0x04000277 RID: 631
		None = 0L,
		// Token: 0x04000278 RID: 632
		ForceRediscovery = 1L,
		// Token: 0x04000279 RID: 633
		DirectoryServicesRequired = 16L,
		// Token: 0x0400027A RID: 634
		DirectoryServicesPreferred = 32L,
		// Token: 0x0400027B RID: 635
		GCRequired = 64L,
		// Token: 0x0400027C RID: 636
		PdcRequired = 128L,
		// Token: 0x0400027D RID: 637
		IPRequired = 512L,
		// Token: 0x0400027E RID: 638
		KdcRequired = 1024L,
		// Token: 0x0400027F RID: 639
		TimeServerRequired = 2048L,
		// Token: 0x04000280 RID: 640
		WriteableRequired = 4096L,
		// Token: 0x04000281 RID: 641
		GoodTimeServerPreferred = 8192L,
		// Token: 0x04000282 RID: 642
		AvoidSelf = 16384L,
		// Token: 0x04000283 RID: 643
		OnlyLdapNeeded = 32768L,
		// Token: 0x04000284 RID: 644
		IsFlatName = 65536L,
		// Token: 0x04000285 RID: 645
		IsDnsName = 131072L,
		// Token: 0x04000286 RID: 646
		ReturnDnsName = 1073741824L,
		// Token: 0x04000287 RID: 647
		ReturnFlatName = 2147483648L
	}
}
