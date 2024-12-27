using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000095 RID: 149
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class berval
	{
		// Token: 0x040002EB RID: 747
		public int bv_len;

		// Token: 0x040002EC RID: 748
		public IntPtr bv_val = (IntPtr)0;
	}
}
