using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001D0 RID: 464
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a75b74e9-2c00-4ebb-b3f9-62a670aaa07e")]
	[ComImport]
	internal interface ISecurityDescriptorReferenceEntry
	{
		// Token: 0x17000234 RID: 564
		// (get) Token: 0x0600084C RID: 2124
		SecurityDescriptorReferenceEntry AllData { get; }

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x0600084D RID: 2125
		string Name
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x0600084E RID: 2126
		string BuildFilter
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
