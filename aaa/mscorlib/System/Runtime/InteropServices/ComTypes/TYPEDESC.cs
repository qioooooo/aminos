using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000573 RID: 1395
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct TYPEDESC
	{
		// Token: 0x04001B2D RID: 6957
		public IntPtr lpValue;

		// Token: 0x04001B2E RID: 6958
		public short vt;
	}
}
