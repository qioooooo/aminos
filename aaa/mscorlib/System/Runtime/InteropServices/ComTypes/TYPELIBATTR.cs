using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000583 RID: 1411
	[Serializable]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct TYPELIBATTR
	{
		// Token: 0x04001B89 RID: 7049
		public Guid guid;

		// Token: 0x04001B8A RID: 7050
		public int lcid;

		// Token: 0x04001B8B RID: 7051
		public SYSKIND syskind;

		// Token: 0x04001B8C RID: 7052
		public short wMajorVerNum;

		// Token: 0x04001B8D RID: 7053
		public short wMinorVerNum;

		// Token: 0x04001B8E RID: 7054
		public LIBFLAGS wLibFlags;
	}
}
