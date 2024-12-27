using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001CD RID: 461
	[Guid("9f27c750-7dfb-46a1-a673-52e53e2337a9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IDirectoryEntry
	{
		// Token: 0x1700022F RID: 559
		// (get) Token: 0x06000846 RID: 2118
		DirectoryEntry AllData { get; }

		// Token: 0x17000230 RID: 560
		// (get) Token: 0x06000847 RID: 2119
		uint Flags { get; }

		// Token: 0x17000231 RID: 561
		// (get) Token: 0x06000848 RID: 2120
		uint Protection { get; }

		// Token: 0x17000232 RID: 562
		// (get) Token: 0x06000849 RID: 2121
		string BuildFilter
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000233 RID: 563
		// (get) Token: 0x0600084A RID: 2122
		object SecurityDescriptor
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
	}
}
