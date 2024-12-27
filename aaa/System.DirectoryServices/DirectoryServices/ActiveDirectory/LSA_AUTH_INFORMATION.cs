using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x02000125 RID: 293
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class LSA_AUTH_INFORMATION
	{
		// Token: 0x040006D6 RID: 1750
		public LARGE_INTEGER LastUpdateTime;

		// Token: 0x040006D7 RID: 1751
		public int AuthType;

		// Token: 0x040006D8 RID: 1752
		public int AuthInfoLength;

		// Token: 0x040006D9 RID: 1753
		public IntPtr AuthInfo;
	}
}
