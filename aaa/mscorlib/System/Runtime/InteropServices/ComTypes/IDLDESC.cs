using System;

namespace System.Runtime.InteropServices.ComTypes
{
	// Token: 0x02000570 RID: 1392
	[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
	public struct IDLDESC
	{
		// Token: 0x04001B20 RID: 6944
		public IntPtr dwReserved;

		// Token: 0x04001B21 RID: 6945
		public IDLFLAG wIDLFlags;
	}
}
