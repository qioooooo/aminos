using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000B2 RID: 178
	[Flags]
	public enum LocatorOptions : long
	{
		// Token: 0x0400047E RID: 1150
		ForceRediscovery = 1L,
		// Token: 0x0400047F RID: 1151
		KdcRequired = 1024L,
		// Token: 0x04000480 RID: 1152
		TimeServerRequired = 2048L,
		// Token: 0x04000481 RID: 1153
		WriteableRequired = 4096L,
		// Token: 0x04000482 RID: 1154
		AvoidSelf = 16384L
	}
}
