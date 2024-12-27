using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000194 RID: 404
	[Guid("3903B11B-FBE8-477c-825F-DB828B5FD174")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ICOMServerEntry
	{
		// Token: 0x170001B6 RID: 438
		// (get) Token: 0x060007AE RID: 1966
		COMServerEntry AllData { get; }

		// Token: 0x170001B7 RID: 439
		// (get) Token: 0x060007AF RID: 1967
		Guid Clsid { get; }

		// Token: 0x170001B8 RID: 440
		// (get) Token: 0x060007B0 RID: 1968
		uint Flags { get; }

		// Token: 0x170001B9 RID: 441
		// (get) Token: 0x060007B1 RID: 1969
		Guid ConfiguredGuid { get; }

		// Token: 0x170001BA RID: 442
		// (get) Token: 0x060007B2 RID: 1970
		Guid ImplementedClsid { get; }

		// Token: 0x170001BB RID: 443
		// (get) Token: 0x060007B3 RID: 1971
		Guid TypeLibrary { get; }

		// Token: 0x170001BC RID: 444
		// (get) Token: 0x060007B4 RID: 1972
		uint ThreadingModel { get; }

		// Token: 0x170001BD RID: 445
		// (get) Token: 0x060007B5 RID: 1973
		string RuntimeVersion
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001BE RID: 446
		// (get) Token: 0x060007B6 RID: 1974
		string HostFile
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
