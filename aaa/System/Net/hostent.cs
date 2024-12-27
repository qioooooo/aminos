using System;
using System.Runtime.InteropServices;

namespace System.Net
{
	// Token: 0x02000420 RID: 1056
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct hostent
	{
		// Token: 0x04002136 RID: 8502
		public IntPtr h_name;

		// Token: 0x04002137 RID: 8503
		public IntPtr h_aliases;

		// Token: 0x04002138 RID: 8504
		public short h_addrtype;

		// Token: 0x04002139 RID: 8505
		public short h_length;

		// Token: 0x0400213A RID: 8506
		public IntPtr h_addr_list;
	}
}
