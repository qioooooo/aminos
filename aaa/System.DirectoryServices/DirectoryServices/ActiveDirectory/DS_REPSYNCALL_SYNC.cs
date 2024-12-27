using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000114 RID: 276
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class DS_REPSYNCALL_SYNC
	{
		// Token: 0x0400068E RID: 1678
		public IntPtr pszSrcId;

		// Token: 0x0400068F RID: 1679
		public IntPtr pszDstId;
	}
}
