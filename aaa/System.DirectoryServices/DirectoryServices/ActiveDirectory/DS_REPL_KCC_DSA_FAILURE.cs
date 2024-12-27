using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200010D RID: 269
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	internal sealed class DS_REPL_KCC_DSA_FAILURE
	{
		// Token: 0x0400066F RID: 1647
		public IntPtr pszDsaDN;

		// Token: 0x04000670 RID: 1648
		public Guid uuidDsaObjGuid;

		// Token: 0x04000671 RID: 1649
		public long ftimeFirstFailure;

		// Token: 0x04000672 RID: 1650
		public int cNumFailures;

		// Token: 0x04000673 RID: 1651
		public int dwLastResult;
	}
}
