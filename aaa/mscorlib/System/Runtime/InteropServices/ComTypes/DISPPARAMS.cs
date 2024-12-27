using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000579 RID: 1401
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct DISPPARAMS
	{
		// Token: 0x04001B40 RID: 6976
		public IntPtr rgvarg;

		// Token: 0x04001B41 RID: 6977
		public IntPtr rgdispidNamedArgs;

		// Token: 0x04001B42 RID: 6978
		public int cArgs;

		// Token: 0x04001B43 RID: 6979
		public int cNamedArgs;
	}
}
