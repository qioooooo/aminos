using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000113 RID: 275
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class DS_REPSYNCALL_ERRINFO
	{
		// Token: 0x0400068A RID: 1674
		public IntPtr pszSvrId;

		// Token: 0x0400068B RID: 1675
		public SyncFromAllServersErrorCategory error;

		// Token: 0x0400068C RID: 1676
		public int dwWin32Err;

		// Token: 0x0400068D RID: 1677
		public IntPtr pszSrcId;
	}
}
