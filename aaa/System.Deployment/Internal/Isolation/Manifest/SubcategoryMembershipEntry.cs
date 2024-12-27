using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200018C RID: 396
	[StructLayout(LayoutKind.Sequential)]
	internal class SubcategoryMembershipEntry
	{
		// Token: 0x040006D6 RID: 1750
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Subcategory;

		// Token: 0x040006D7 RID: 1751
		public ISection CategoryMembershipData;
	}
}
