using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000106 RID: 262
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class DS_REPL_CURSOR_3
	{
		// Token: 0x04000649 RID: 1609
		public Guid uuidSourceDsaInvocationID;

		// Token: 0x0400064A RID: 1610
		public long usnAttributeFilter;

		// Token: 0x0400064B RID: 1611
		public long ftimeLastSyncSuccess;

		// Token: 0x0400064C RID: 1612
		public IntPtr pszSourceDsaDN;
	}
}
