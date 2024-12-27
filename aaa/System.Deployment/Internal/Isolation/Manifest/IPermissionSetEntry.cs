using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001AC RID: 428
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("EBE5A1ED-FEBC-42c4-A9E1-E087C6E36635")]
	[ComImport]
	internal interface IPermissionSetEntry
	{
		// Token: 0x170001E2 RID: 482
		// (get) Token: 0x060007E5 RID: 2021
		PermissionSetEntry AllData { get; }

		// Token: 0x170001E3 RID: 483
		// (get) Token: 0x060007E6 RID: 2022
		string Id
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001E4 RID: 484
		// (get) Token: 0x060007E7 RID: 2023
		string XmlSegment
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
