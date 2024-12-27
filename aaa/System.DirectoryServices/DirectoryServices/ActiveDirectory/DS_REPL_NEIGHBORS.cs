using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200010A RID: 266
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class DS_REPL_NEIGHBORS
	{
		// Token: 0x0400065B RID: 1627
		public int cNumNeighbors;

		// Token: 0x0400065C RID: 1628
		public int dwReserved;
	}
}
