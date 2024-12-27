using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200019C RID: 412
	[StructLayout(LayoutKind.Sequential)]
	internal class CategoryMembershipDataEntry
	{
		// Token: 0x04000740 RID: 1856
		public uint index;

		// Token: 0x04000741 RID: 1857
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Xml;

		// Token: 0x04000742 RID: 1858
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;
	}
}
