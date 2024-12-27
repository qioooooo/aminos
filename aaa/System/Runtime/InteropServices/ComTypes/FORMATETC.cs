using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x0200026E RID: 622
	public struct FORMATETC
	{
		// Token: 0x0400123B RID: 4667
		[MarshalAs(UnmanagedType.U2)]
		public short cfFormat;

		// Token: 0x0400123C RID: 4668
		public IntPtr ptd;

		// Token: 0x0400123D RID: 4669
		[MarshalAs(UnmanagedType.U4)]
		public DVASPECT dwAspect;

		// Token: 0x0400123E RID: 4670
		public int lindex;

		// Token: 0x0400123F RID: 4671
		[MarshalAs(UnmanagedType.U4)]
		public TYMED tymed;
	}
}
