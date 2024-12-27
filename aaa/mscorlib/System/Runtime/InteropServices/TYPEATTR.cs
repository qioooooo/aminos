using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x0200053A RID: 1338
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.TYPEATTR instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct TYPEATTR
	{
		// Token: 0x04001A31 RID: 6705
		public const int MEMBER_ID_NIL = -1;

		// Token: 0x04001A32 RID: 6706
		public Guid guid;

		// Token: 0x04001A33 RID: 6707
		public int lcid;

		// Token: 0x04001A34 RID: 6708
		public int dwReserved;

		// Token: 0x04001A35 RID: 6709
		public int memidConstructor;

		// Token: 0x04001A36 RID: 6710
		public int memidDestructor;

		// Token: 0x04001A37 RID: 6711
		public IntPtr lpstrSchema;

		// Token: 0x04001A38 RID: 6712
		public int cbSizeInstance;

		// Token: 0x04001A39 RID: 6713
		public TYPEKIND typekind;

		// Token: 0x04001A3A RID: 6714
		public short cFuncs;

		// Token: 0x04001A3B RID: 6715
		public short cVars;

		// Token: 0x04001A3C RID: 6716
		public short cImplTypes;

		// Token: 0x04001A3D RID: 6717
		public short cbSizeVft;

		// Token: 0x04001A3E RID: 6718
		public short cbAlignment;

		// Token: 0x04001A3F RID: 6719
		public TYPEFLAGS wTypeFlags;

		// Token: 0x04001A40 RID: 6720
		public short wMajorVerNum;

		// Token: 0x04001A41 RID: 6721
		public short wMinorVerNum;

		// Token: 0x04001A42 RID: 6722
		public TYPEDESC tdescAlias;

		// Token: 0x04001A43 RID: 6723
		public IDLDESC idldescType;
	}
}
