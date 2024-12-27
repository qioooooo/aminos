using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200010C RID: 268
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class DS_REPL_KCC_DSA_FAILURES
	{
		// Token: 0x0400066D RID: 1645
		public int cNumEntries;

		// Token: 0x0400066E RID: 1646
		public int dwReserved;
	}
}
