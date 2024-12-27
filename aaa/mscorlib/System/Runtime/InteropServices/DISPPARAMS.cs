using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000545 RID: 1349
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.DISPPARAMS instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct DISPPARAMS
	{
		// Token: 0x04001A70 RID: 6768
		public IntPtr rgvarg;

		// Token: 0x04001A71 RID: 6769
		public IntPtr rgdispidNamedArgs;

		// Token: 0x04001A72 RID: 6770
		public int cArgs;

		// Token: 0x04001A73 RID: 6771
		public int cNamedArgs;
	}
}
