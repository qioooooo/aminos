using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000107 RID: 263
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class DS_REPL_CURSOR
	{
		// Token: 0x0400064D RID: 1613
		public Guid uuidSourceDsaInvocationID;

		// Token: 0x0400064E RID: 1614
		public long usnAttributeFilter;
	}
}
