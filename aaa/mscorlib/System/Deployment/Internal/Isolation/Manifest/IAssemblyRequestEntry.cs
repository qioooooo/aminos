using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C2 RID: 450
	[Guid("2474ECB4-8EFD-4410-9F31-B3E7C4A07731")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IAssemblyRequestEntry
	{
		// Token: 0x170002B0 RID: 688
		// (get) Token: 0x06001483 RID: 5251
		AssemblyRequestEntry AllData { get; }

		// Token: 0x170002B1 RID: 689
		// (get) Token: 0x06001484 RID: 5252
		string Name
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002B2 RID: 690
		// (get) Token: 0x06001485 RID: 5253
		string permissionSetID
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
