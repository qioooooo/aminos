using System;
using System.Runtime.InteropServices;

namespace GeFanuc.iFixToolkit.Adapter
{
	// Token: 0x02000004 RID: 4
	[StructLayout(LayoutKind.Sequential, Pack = 2)]
	public struct AlmCanonicalValue
	{
		// Token: 0x04000011 RID: 17
		[MarshalAs(UnmanagedType.U4)]
		public int ulValueType;

		// Token: 0x04000012 RID: 18
		public almUnion unionValue;

		// Token: 0x04000013 RID: 19
		public short bTextValid;

		// Token: 0x04000014 RID: 20
		[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 80)]
		public string szText;
	}
}
