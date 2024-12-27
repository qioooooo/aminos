using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000B3 RID: 179
	[Flags]
	internal enum PrivateLocatorFlags : long
	{
		// Token: 0x04000484 RID: 1156
		DirectoryServicesRequired = 16L,
		// Token: 0x04000485 RID: 1157
		DirectoryServicesPreferred = 32L,
		// Token: 0x04000486 RID: 1158
		GCRequired = 64L,
		// Token: 0x04000487 RID: 1159
		PdcRequired = 128L,
		// Token: 0x04000488 RID: 1160
		BackgroundOnly = 256L,
		// Token: 0x04000489 RID: 1161
		IPRequired = 512L,
		// Token: 0x0400048A RID: 1162
		DSWriteableRequired = 4096L,
		// Token: 0x0400048B RID: 1163
		GoodTimeServerPreferred = 8192L,
		// Token: 0x0400048C RID: 1164
		OnlyLDAPNeeded = 32768L,
		// Token: 0x0400048D RID: 1165
		IsFlatName = 65536L,
		// Token: 0x0400048E RID: 1166
		IsDNSName = 131072L,
		// Token: 0x0400048F RID: 1167
		ReturnDNSName = 1073741824L,
		// Token: 0x04000490 RID: 1168
		ReturnFlatName = 2147483648L
	}
}
