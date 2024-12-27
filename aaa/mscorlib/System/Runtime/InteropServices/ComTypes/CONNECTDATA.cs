using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200055A RID: 1370
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct CONNECTDATA
	{
		// Token: 0x04001AC3 RID: 6851
		[MarshalAs(UnmanagedType.Interface)]
		public object pUnk;

		// Token: 0x04001AC4 RID: 6852
		public int dwCookie;
	}
}
