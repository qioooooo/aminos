using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000189 RID: 393
	[StructLayout(LayoutKind.Sequential)]
	internal class CategoryMembershipDataEntry
	{
		// Token: 0x040006D0 RID: 1744
		public uint index;

		// Token: 0x040006D1 RID: 1745
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Xml;

		// Token: 0x040006D2 RID: 1746
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;
	}
}
