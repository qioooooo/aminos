using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000B0 RID: 176
	[StructLayout(LayoutKind.Sequential)]
	internal class EventTagEntry
	{
		// Token: 0x04000D11 RID: 3345
		[MarshalAs(UnmanagedType.LPWStr)]
		public string TagData;

		// Token: 0x04000D12 RID: 3346
		public uint EventID;
	}
}
