using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200054F RID: 1359
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.TYPELIBATTR instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[Serializable]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct TYPELIBATTR
	{
		// Token: 0x04001AB7 RID: 6839
		public Guid guid;

		// Token: 0x04001AB8 RID: 6840
		public int lcid;

		// Token: 0x04001AB9 RID: 6841
		public SYSKIND syskind;

		// Token: 0x04001ABA RID: 6842
		public short wMajorVerNum;

		// Token: 0x04001ABB RID: 6843
		public short wMinorVerNum;

		// Token: 0x04001ABC RID: 6844
		public LIBFLAGS wLibFlags;
	}
}
