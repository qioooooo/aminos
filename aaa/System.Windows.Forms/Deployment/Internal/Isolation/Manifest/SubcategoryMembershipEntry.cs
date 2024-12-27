using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200007A RID: 122
	[StructLayout(LayoutKind.Sequential)]
	internal class SubcategoryMembershipEntry
	{
		// Token: 0x04000C4C RID: 3148
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Subcategory;

		// Token: 0x04000C4D RID: 3149
		public ISection CategoryMembershipData;
	}
}
