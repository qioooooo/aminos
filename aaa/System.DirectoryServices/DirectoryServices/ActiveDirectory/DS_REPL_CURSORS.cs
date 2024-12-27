using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000105 RID: 261
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class DS_REPL_CURSORS
	{
		// Token: 0x04000647 RID: 1607
		public int cNumCursors;

		// Token: 0x04000648 RID: 1608
		public int reserved;
	}
}
