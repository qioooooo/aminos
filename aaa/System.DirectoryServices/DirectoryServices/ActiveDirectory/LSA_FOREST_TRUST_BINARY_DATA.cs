using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200011E RID: 286
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class LSA_FOREST_TRUST_BINARY_DATA
	{
		// Token: 0x040006B8 RID: 1720
		public int Length;

		// Token: 0x040006B9 RID: 1721
		public IntPtr Buffer;
	}
}
