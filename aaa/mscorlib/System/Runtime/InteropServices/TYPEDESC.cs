using System;

namespace System.Runtime.InteropServices
{
	// Token: 0x02000540 RID: 1344
	[Obsolete("Use System.Runtime.InteropServices.ComTypes.TYPEDESC instead. http://go.microsoft.com/fwlink/?linkid=14202", false)]
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct TYPEDESC
	{
		// Token: 0x04001A63 RID: 6755
		public IntPtr lpValue;

		// Token: 0x04001A64 RID: 6756
		public short vt;
	}
}
