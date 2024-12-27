using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000119 RID: 281
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class LSA_FOREST_TRUST_INFORMATION
	{
		// Token: 0x040006A4 RID: 1700
		public int RecordCount;

		// Token: 0x040006A5 RID: 1701
		public IntPtr Entries;
	}
}
