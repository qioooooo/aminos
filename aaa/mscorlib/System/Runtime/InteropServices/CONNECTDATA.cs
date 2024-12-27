using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000527 RID: 1319
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.CONNECTDATA instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct CONNECTDATA
	{
		// Token: 0x040019F9 RID: 6649
		[MarshalAs(UnmanagedType.Interface)]
		public object pUnk;

		// Token: 0x040019FA RID: 6650
		public int dwCookie;
	}
}
