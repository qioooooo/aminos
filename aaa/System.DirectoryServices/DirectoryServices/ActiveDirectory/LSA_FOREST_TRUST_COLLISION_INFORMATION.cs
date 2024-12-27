using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000121 RID: 289
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class LSA_FOREST_TRUST_COLLISION_INFORMATION
	{
		// Token: 0x040006C6 RID: 1734
		public int RecordCount;

		// Token: 0x040006C7 RID: 1735
		public IntPtr Entries;
	}
}
