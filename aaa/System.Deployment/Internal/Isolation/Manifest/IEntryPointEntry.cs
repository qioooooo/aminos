using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A9 RID: 425
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("1583EFE9-832F-4d08-B041-CAC5ACEDB948")]
	[ComImport]
	internal interface IEntryPointEntry
	{
		// Token: 0x170001DC RID: 476
		// (get) Token: 0x060007DE RID: 2014
		EntryPointEntry AllData { get; }

		// Token: 0x170001DD RID: 477
		// (get) Token: 0x060007DF RID: 2015
		string Name
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001DE RID: 478
		// (get) Token: 0x060007E0 RID: 2016
		string CommandLine_File
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001DF RID: 479
		// (get) Token: 0x060007E1 RID: 2017
		string CommandLine_Parameters
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001E0 RID: 480
		// (get) Token: 0x060007E2 RID: 2018
		IReferenceIdentity Identity { get; }

		// Token: 0x170001E1 RID: 481
		// (get) Token: 0x060007E3 RID: 2019
		uint Flags { get; }
	}
}
