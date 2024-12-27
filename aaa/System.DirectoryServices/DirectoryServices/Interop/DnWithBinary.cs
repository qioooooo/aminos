using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Interop
{
	// Token: 0x02000057 RID: 87
	[StructLayout(LayoutKind.Sequential)]
	internal class DnWithBinary
	{
		// Token: 0x04000275 RID: 629
		public int dwLength;

		// Token: 0x04000276 RID: 630
		public IntPtr lpBinaryValue;

		// Token: 0x04000277 RID: 631
		public IntPtr pszDNString;
	}
}
