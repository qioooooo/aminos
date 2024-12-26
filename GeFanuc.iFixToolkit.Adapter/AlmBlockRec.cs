using System;
using System.Runtime.InteropServices;

namespace GeFanuc.iFixToolkit.Adapter
{
	// Token: 0x02000008 RID: 8
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct AlmBlockRec
	{
		// Token: 0x04000028 RID: 40
		public AlmCanonicalValue aCurrentValue;

		// Token: 0x04000029 RID: 41
		public VSP aVsp;

		// Token: 0x0400002A RID: 42
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 31)]
		public string szTag;

		// Token: 0x0400002B RID: 43
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 20)]
		public string szField;

		// Token: 0x0400002C RID: 44
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 34)]
		public string szEgu;

		// Token: 0x0400002D RID: 45
		public int ulState;

		// Token: 0x0400002E RID: 46
		public int ulFlag;

		// Token: 0x0400002F RID: 47
		public int ulAlmNum;

		// Token: 0x04000030 RID: 48
		public int ulReserved;
	}
}
