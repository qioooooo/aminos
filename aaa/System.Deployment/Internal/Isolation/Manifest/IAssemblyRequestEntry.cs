using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001AF RID: 431
	[Guid("2474ECB4-8EFD-4410-9F31-B3E7C4A07731")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IAssemblyRequestEntry
	{
		// Token: 0x170001E5 RID: 485
		// (get) Token: 0x060007E9 RID: 2025
		AssemblyRequestEntry AllData { get; }

		// Token: 0x170001E6 RID: 486
		// (get) Token: 0x060007EA RID: 2026
		string Name
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001E7 RID: 487
		// (get) Token: 0x060007EB RID: 2027
		string permissionSetID
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
