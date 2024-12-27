using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000565 RID: 1381
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct STATSTG
	{
		// Token: 0x04001AC7 RID: 6855
		public string pwcsName;

		// Token: 0x04001AC8 RID: 6856
		public int type;

		// Token: 0x04001AC9 RID: 6857
		public long cbSize;

		// Token: 0x04001ACA RID: 6858
		public FILETIME mtime;

		// Token: 0x04001ACB RID: 6859
		public FILETIME ctime;

		// Token: 0x04001ACC RID: 6860
		public FILETIME atime;

		// Token: 0x04001ACD RID: 6861
		public int grfMode;

		// Token: 0x04001ACE RID: 6862
		public int grfLocksSupported;

		// Token: 0x04001ACF RID: 6863
		public Guid clsid;

		// Token: 0x04001AD0 RID: 6864
		public int grfStateBits;

		// Token: 0x04001AD1 RID: 6865
		public int reserved;
	}
}
