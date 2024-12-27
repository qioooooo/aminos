using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000086 RID: 134
	[StructLayout(LayoutKind.Sequential)]
	internal class CLRSurrogateEntry
	{
		// Token: 0x04000C68 RID: 3176
		public Guid Clsid;

		// Token: 0x04000C69 RID: 3177
		[MarshalAs(UnmanagedType.LPWStr)]
		public string RuntimeVersion;

		// Token: 0x04000C6A RID: 3178
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ClassName;
	}
}
