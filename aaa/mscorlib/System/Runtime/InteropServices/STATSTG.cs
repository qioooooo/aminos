using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000532 RID: 1330
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.STATSTG instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct STATSTG
	{
		// Token: 0x040019FD RID: 6653
		public string pwcsName;

		// Token: 0x040019FE RID: 6654
		public int type;

		// Token: 0x040019FF RID: 6655
		public long cbSize;

		// Token: 0x04001A00 RID: 6656
		public FILETIME mtime;

		// Token: 0x04001A01 RID: 6657
		public FILETIME ctime;

		// Token: 0x04001A02 RID: 6658
		public FILETIME atime;

		// Token: 0x04001A03 RID: 6659
		public int grfMode;

		// Token: 0x04001A04 RID: 6660
		public int grfLocksSupported;

		// Token: 0x04001A05 RID: 6661
		public Guid clsid;

		// Token: 0x04001A06 RID: 6662
		public int grfStateBits;

		// Token: 0x04001A07 RID: 6663
		public int reserved;
	}
}
