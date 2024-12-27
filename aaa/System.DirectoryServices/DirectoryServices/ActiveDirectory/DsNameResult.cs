using System;
using System.Runtime.InteropServices;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000B8 RID: 184
	[StructLayout(LayoutKind.Sequential)]
	internal sealed class DsNameResult
	{
		// Token: 0x040004BA RID: 1210
		public int itemCount;

		// Token: 0x040004BB RID: 1211
		public IntPtr items;
	}
}
