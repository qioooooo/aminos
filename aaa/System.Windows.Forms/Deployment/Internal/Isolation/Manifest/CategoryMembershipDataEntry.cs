using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000077 RID: 119
	[StructLayout(LayoutKind.Sequential)]
	internal class CategoryMembershipDataEntry
	{
		// Token: 0x04000C46 RID: 3142
		public uint index;

		// Token: 0x04000C47 RID: 3143
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Xml;

		// Token: 0x04000C48 RID: 3144
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Description;
	}
}
