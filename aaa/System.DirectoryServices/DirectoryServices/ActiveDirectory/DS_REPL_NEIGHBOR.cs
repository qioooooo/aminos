using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200010B RID: 267
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class DS_REPL_NEIGHBOR
	{
		// Token: 0x0400065D RID: 1629
		public IntPtr pszNamingContext;

		// Token: 0x0400065E RID: 1630
		public IntPtr pszSourceDsaDN;

		// Token: 0x0400065F RID: 1631
		public IntPtr pszSourceDsaAddress;

		// Token: 0x04000660 RID: 1632
		public IntPtr pszAsyncIntersiteTransportDN;

		// Token: 0x04000661 RID: 1633
		public int dwReplicaFlags;

		// Token: 0x04000662 RID: 1634
		public int dwReserved;

		// Token: 0x04000663 RID: 1635
		public Guid uuidNamingContextObjGuid;

		// Token: 0x04000664 RID: 1636
		public Guid uuidSourceDsaObjGuid;

		// Token: 0x04000665 RID: 1637
		public Guid uuidSourceDsaInvocationID;

		// Token: 0x04000666 RID: 1638
		public Guid uuidAsyncIntersiteTransportObjGuid;

		// Token: 0x04000667 RID: 1639
		public long usnLastObjChangeSynced;

		// Token: 0x04000668 RID: 1640
		public long usnAttributeFilter;

		// Token: 0x04000669 RID: 1641
		public long ftimeLastSyncSuccess;

		// Token: 0x0400066A RID: 1642
		public long ftimeLastSyncAttempt;

		// Token: 0x0400066B RID: 1643
		public int dwLastSyncResult;

		// Token: 0x0400066C RID: 1644
		public int cNumConsecutiveSyncFailures;
	}
}
