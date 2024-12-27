using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200009A RID: 154
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct SecPkgContext_IssuerListInfoEx
	{
		// Token: 0x040002F8 RID: 760
		public IntPtr aIssuers;

		// Token: 0x040002F9 RID: 761
		public int cIssuers;
	}
}
