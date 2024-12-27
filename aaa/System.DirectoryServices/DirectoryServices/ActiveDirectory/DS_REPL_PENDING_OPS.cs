using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000108 RID: 264
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class DS_REPL_PENDING_OPS
	{
		// Token: 0x0400064F RID: 1615
		public long ftimeCurrentOpStarted;

		// Token: 0x04000650 RID: 1616
		public int cNumPendingOps;
	}
}
