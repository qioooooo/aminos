using System;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000FD RID: 253
	[Flags]
	internal enum DS_DOMAINTRUST_FLAG
	{
		// Token: 0x04000621 RID: 1569
		DS_DOMAIN_IN_FOREST = 1,
		// Token: 0x04000622 RID: 1570
		DS_DOMAIN_DIRECT_OUTBOUND = 2,
		// Token: 0x04000623 RID: 1571
		DS_DOMAIN_TREE_ROOT = 4,
		// Token: 0x04000624 RID: 1572
		DS_DOMAIN_PRIMARY = 8,
		// Token: 0x04000625 RID: 1573
		DS_DOMAIN_NATIVE_MODE = 16,
		// Token: 0x04000626 RID: 1574
		DS_DOMAIN_DIRECT_INBOUND = 32
	}
}
