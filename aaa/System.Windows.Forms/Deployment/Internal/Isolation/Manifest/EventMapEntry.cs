using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000AD RID: 173
	[StructLayout(LayoutKind.Sequential)]
	internal class EventMapEntry
	{
		// Token: 0x04000D09 RID: 3337
		[MarshalAs(UnmanagedType.LPWStr)]
		public string MapName;

		// Token: 0x04000D0A RID: 3338
		[MarshalAs(UnmanagedType.LPWStr)]
		public string Name;

		// Token: 0x04000D0B RID: 3339
		public uint Value;

		// Token: 0x04000D0C RID: 3340
		public bool IsValueMap;
	}
}
