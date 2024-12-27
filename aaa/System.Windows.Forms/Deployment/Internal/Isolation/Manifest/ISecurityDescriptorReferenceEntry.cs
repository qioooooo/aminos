using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000BE RID: 190
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a75b74e9-2c00-4ebb-b3f9-62a670aaa07e")]
	[ComImport]
	internal interface ISecurityDescriptorReferenceEntry
	{
		// Token: 0x17000124 RID: 292
		// (get) Token: 0x06000301 RID: 769
		SecurityDescriptorReferenceEntry AllData { get; }

		// Token: 0x17000125 RID: 293
		// (get) Token: 0x06000302 RID: 770
		string Name
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000126 RID: 294
		// (get) Token: 0x06000303 RID: 771
		string BuildFilter
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
