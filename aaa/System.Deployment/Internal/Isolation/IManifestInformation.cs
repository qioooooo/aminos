using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200015A RID: 346
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("81c85208-fe61-4c15-b5bb-ff5ea66baad9")]
	[ComImport]
	internal interface IManifestInformation
	{
		// Token: 0x06000737 RID: 1847
		void get_FullPath([MarshalAs(UnmanagedType.LPWStr)] out string FullPath);
	}
}
