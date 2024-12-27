using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000BE RID: 190
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class LUID
	{
		// Token: 0x040004DF RID: 1247
		public int LowPart;

		// Token: 0x040004E0 RID: 1248
		public int HighPart;
	}
}
