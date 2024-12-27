using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200056D RID: 1389
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct TYPEATTR
	{
		// Token: 0x04001AFB RID: 6907
		public const int MEMBER_ID_NIL = -1;

		// Token: 0x04001AFC RID: 6908
		public Guid guid;

		// Token: 0x04001AFD RID: 6909
		public int lcid;

		// Token: 0x04001AFE RID: 6910
		public int dwReserved;

		// Token: 0x04001AFF RID: 6911
		public int memidConstructor;

		// Token: 0x04001B00 RID: 6912
		public int memidDestructor;

		// Token: 0x04001B01 RID: 6913
		public IntPtr lpstrSchema;

		// Token: 0x04001B02 RID: 6914
		public int cbSizeInstance;

		// Token: 0x04001B03 RID: 6915
		public TYPEKIND typekind;

		// Token: 0x04001B04 RID: 6916
		public short cFuncs;

		// Token: 0x04001B05 RID: 6917
		public short cVars;

		// Token: 0x04001B06 RID: 6918
		public short cImplTypes;

		// Token: 0x04001B07 RID: 6919
		public short cbSizeVft;

		// Token: 0x04001B08 RID: 6920
		public short cbAlignment;

		// Token: 0x04001B09 RID: 6921
		public TYPEFLAGS wTypeFlags;

		// Token: 0x04001B0A RID: 6922
		public short wMajorVerNum;

		// Token: 0x04001B0B RID: 6923
		public short wMinorVerNum;

		// Token: 0x04001B0C RID: 6924
		public TYPEDESC tdescAlias;

		// Token: 0x04001B0D RID: 6925
		public IDLDESC idldescType;
	}
}
