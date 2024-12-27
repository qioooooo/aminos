using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B8 RID: 440
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("CF168CF4-4E8F-4d92-9D2A-60E5CA21CF85")]
	[ComImport]
	internal interface IDependentOSMetadataEntry
	{
		// Token: 0x170001F5 RID: 501
		// (get) Token: 0x060007FC RID: 2044
		DependentOSMetadataEntry AllData { get; }

		// Token: 0x170001F6 RID: 502
		// (get) Token: 0x060007FD RID: 2045
		string SupportUrl
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001F7 RID: 503
		// (get) Token: 0x060007FE RID: 2046
		string Description
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001F8 RID: 504
		// (get) Token: 0x060007FF RID: 2047
		ushort MajorVersion { get; }

		// Token: 0x170001F9 RID: 505
		// (get) Token: 0x06000800 RID: 2048
		ushort MinorVersion { get; }

		// Token: 0x170001FA RID: 506
		// (get) Token: 0x06000801 RID: 2049
		ushort BuildNumber { get; }

		// Token: 0x170001FB RID: 507
		// (get) Token: 0x06000802 RID: 2050
		byte ServicePackMajor { get; }

		// Token: 0x170001FC RID: 508
		// (get) Token: 0x06000803 RID: 2051
		byte ServicePackMinor { get; }
	}
}
