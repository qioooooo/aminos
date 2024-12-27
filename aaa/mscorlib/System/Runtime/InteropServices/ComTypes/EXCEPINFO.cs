using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200057A RID: 1402
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct EXCEPINFO
	{
		// Token: 0x04001B44 RID: 6980
		public short wCode;

		// Token: 0x04001B45 RID: 6981
		public short wReserved;

		// Token: 0x04001B46 RID: 6982
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrSource;

		// Token: 0x04001B47 RID: 6983
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrDescription;

		// Token: 0x04001B48 RID: 6984
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrHelpFile;

		// Token: 0x04001B49 RID: 6985
		public int dwHelpContext;

		// Token: 0x04001B4A RID: 6986
		public IntPtr pvReserved;

		// Token: 0x04001B4B RID: 6987
		public IntPtr pfnDeferredFillIn;

		// Token: 0x04001B4C RID: 6988
		public int scode;
	}
}
