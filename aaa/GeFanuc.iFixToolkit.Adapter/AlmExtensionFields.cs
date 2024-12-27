using System;
using System.Runtime.InteropServices;

namespace GeFanuc.iFixToolkit.Adapter
{
	// Token: 0x02000003 RID: 3
	public struct AlmExtensionFields
	{
		// Token: 0x0400000B RID: 11
		public short iExtensionType;

		// Token: 0x0400000C RID: 12
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
		public string szExtText1;

		// Token: 0x0400000D RID: 13
		public char cSpare1;

		// Token: 0x0400000E RID: 14
		public char cSpare2;

		// Token: 0x0400000F RID: 15
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 81)]
		public string szExtText2;

		// Token: 0x04000010 RID: 16
		public int lReserved2;
	}
}
