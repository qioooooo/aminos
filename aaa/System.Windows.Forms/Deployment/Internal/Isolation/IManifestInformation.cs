using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000131 RID: 305
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("81c85208-fe61-4c15-b5bb-ff5ea66baad9")]
	[ComImport]
	internal interface IManifestInformation
	{
		// Token: 0x0600045D RID: 1117
		void get_FullPath([MarshalAs(UnmanagedType.LPWStr)] out string FullPath);
	}
}
