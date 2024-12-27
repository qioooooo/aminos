using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000568 RID: 1384
	[StructLayout(LayoutKind.Explicit, CharSet = CharSet.Unicode)]
	public struct BINDPTR
	{
		// Token: 0x04001AD9 RID: 6873
		[FieldOffset(0)]
		public IntPtr lpfuncdesc;

		// Token: 0x04001ADA RID: 6874
		[FieldOffset(0)]
		public IntPtr lpvardesc;

		// Token: 0x04001ADB RID: 6875
		[FieldOffset(0)]
		public IntPtr lptcomp;
	}
}
