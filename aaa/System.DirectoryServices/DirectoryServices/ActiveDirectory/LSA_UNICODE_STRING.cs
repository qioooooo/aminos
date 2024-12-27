using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x0200011C RID: 284
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class LSA_UNICODE_STRING
	{
		// Token: 0x040006AE RID: 1710
		public short Length;

		// Token: 0x040006AF RID: 1711
		public short MaximumLength;

		// Token: 0x040006B0 RID: 1712
		public IntPtr Buffer;
	}
}
