using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000546 RID: 1350
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.EXCEPINFO instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct EXCEPINFO
	{
		// Token: 0x04001A74 RID: 6772
		public short wCode;

		// Token: 0x04001A75 RID: 6773
		public short wReserved;

		// Token: 0x04001A76 RID: 6774
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrSource;

		// Token: 0x04001A77 RID: 6775
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrDescription;

		// Token: 0x04001A78 RID: 6776
		[MarshalAs(UnmanagedType.BStr)]
		public string bstrHelpFile;

		// Token: 0x04001A79 RID: 6777
		public int dwHelpContext;

		// Token: 0x04001A7A RID: 6778
		public IntPtr pvReserved;

		// Token: 0x04001A7B RID: 6779
		public IntPtr pfnDeferredFillIn;
	}
}
