using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000011 RID: 17
	[ComVisible(false)]
	[StructLayout(LayoutKind.Sequential, Pack = 1)]
	public struct BOID
	{
		// Token: 0x0400000E RID: 14
		[MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
		public byte[] rgb;
	}
}
