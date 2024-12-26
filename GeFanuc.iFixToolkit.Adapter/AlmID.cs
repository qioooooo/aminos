using System;
using System.Runtime.InteropServices;

namespace GeFanuc.iFixToolkit.Adapter
{
	// Token: 0x0200000A RID: 10
	public struct AlmID
	{
		// Token: 0x04000036 RID: 54
		private short iType;

		// Token: 0x04000037 RID: 55
		private short iSpare;

		// Token: 0x04000038 RID: 56
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 168)]
		public string szBuf1;

		// Token: 0x04000039 RID: 57
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 168)]
		public string szBuf2;
	}
}
