using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000112 RID: 274
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class DS_REPSYNCALL_UPDATE
	{
		// Token: 0x04000687 RID: 1671
		public SyncFromAllServersEvent eventType;

		// Token: 0x04000688 RID: 1672
		public IntPtr pErrInfo;

		// Token: 0x04000689 RID: 1673
		public IntPtr pSync;
	}
}
