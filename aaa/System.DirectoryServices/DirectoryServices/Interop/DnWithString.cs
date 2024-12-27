using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Interop
{
	// Token: 0x02000058 RID: 88
	[StructLayout(LayoutKind.Sequential)]
	internal class DnWithString
	{
		// Token: 0x04000278 RID: 632
		public IntPtr pszStringValue;

		// Token: 0x04000279 RID: 633
		public IntPtr pszDNString;
	}
}
