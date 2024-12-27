using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000104 RID: 260
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class DS_REPL_CURSORS_3
	{
		// Token: 0x04000645 RID: 1605
		public int cNumCursors;

		// Token: 0x04000646 RID: 1606
		public int dwEnumerationContext;
	}
}
