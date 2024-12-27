using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001BF RID: 447
	[StructLayout(LayoutKind.Sequential)]
	internal class EventMapEntry
	{
		// Token: 0x04000793 RID: 1939
		[MarshalAs(UnmanagedType.LPWStr)]
		public string MapName;

		// Token: 0x04000794 RID: 1940
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x04000795 RID: 1941
		public uint Value;

		// Token: 0x04000796 RID: 1942
		public bool IsValueMap;
	}
}
