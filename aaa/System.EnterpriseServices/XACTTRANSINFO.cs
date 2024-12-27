using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000012 RID: 18
	[ComVisible(false)]
	[StructLayout(LayoutKind.Sequential, Pack = 4)]
	public struct XACTTRANSINFO
	{
		// Token: 0x0400000F RID: 15
		public BOID uow;

		// Token: 0x04000010 RID: 16
		public int isoLevel;

		// Token: 0x04000011 RID: 17
		public int isoFlags;

		// Token: 0x04000012 RID: 18
		public int grfTCSupported;

		// Token: 0x04000013 RID: 19
		public int grfRMSupported;

		// Token: 0x04000014 RID: 20
		public int grfTCSupportedRetaining;

		// Token: 0x04000015 RID: 21
		public int grfRMSupportedRetaining;
	}
}
