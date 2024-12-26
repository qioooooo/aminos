using System;
using System.Runtime.InteropServices;

namespace GeFanuc.iFixToolkit.Adapter
{
	// Token: 0x02000009 RID: 9
	public struct AlmOperRec
	{
		// Token: 0x04000031 RID: 49
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 9)]
		public string szOperatorNodeName;

		// Token: 0x04000032 RID: 50
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
		public string szOperatorLoginName;

		// Token: 0x04000033 RID: 51
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
		public string szAppName;

		// Token: 0x04000034 RID: 52
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
		public string szTag;

		// Token: 0x04000035 RID: 53
		public int ulReserved;
	}
}
