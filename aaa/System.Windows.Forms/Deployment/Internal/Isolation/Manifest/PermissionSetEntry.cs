using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000098 RID: 152
	[StructLayout(LayoutKind.Sequential)]
	internal class PermissionSetEntry
	{
		// Token: 0x04000C9F RID: 3231
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Id;

		// Token: 0x04000CA0 RID: 3232
		[MarshalAs(UnmanagedType.LPWStr)]
		public string XmlSegment;
	}
}
