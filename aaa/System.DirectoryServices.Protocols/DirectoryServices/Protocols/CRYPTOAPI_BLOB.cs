using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x02000099 RID: 153
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	internal struct CRYPTOAPI_BLOB
	{
		// Token: 0x040002F6 RID: 758
		public int cbData;

		// Token: 0x040002F7 RID: 759
		public IntPtr pbData;
	}
}
