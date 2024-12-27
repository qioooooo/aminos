using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000109 RID: 265
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal sealed class DS_REPL_OP
	{
		// Token: 0x04000651 RID: 1617
		public long ftimeEnqueued;

		// Token: 0x04000652 RID: 1618
		public int ulSerialNumber;

		// Token: 0x04000653 RID: 1619
		public int ulPriority;

		// Token: 0x04000654 RID: 1620
		public ReplicationOperationType OpType;

		// Token: 0x04000655 RID: 1621
		public int ulOptions;

		// Token: 0x04000656 RID: 1622
		public IntPtr pszNamingContext;

		// Token: 0x04000657 RID: 1623
		public IntPtr pszDsaDN;

		// Token: 0x04000658 RID: 1624
		public IntPtr pszDsaAddress;

		// Token: 0x04000659 RID: 1625
		public Guid uuidNamingContextObjGuid;

		// Token: 0x0400065A RID: 1626
		public Guid uuidDsaObjGuid;
	}
}
