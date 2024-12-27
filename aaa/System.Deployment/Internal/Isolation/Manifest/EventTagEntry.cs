using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C2 RID: 450
	[StructLayout(LayoutKind.Sequential)]
	internal class EventTagEntry
	{
		// Token: 0x0400079B RID: 1947
		[MarshalAs(UnmanagedType.LPWStr)]
		public string TagData;

		// Token: 0x0400079C RID: 1948
		public uint EventID;
	}
}
