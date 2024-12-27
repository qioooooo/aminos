using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000535 RID: 1333
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.BINDPTR instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
	public struct BINDPTR
	{
		// Token: 0x04001A0F RID: 6671
		[FieldOffset(0)]
		public IntPtr lpfuncdesc;

		// Token: 0x04001A10 RID: 6672
		[FieldOffset(0)]
		public IntPtr lpvardesc;

		// Token: 0x04001A11 RID: 6673
		[FieldOffset(0)]
		public IntPtr lptcomp;
	}
}
