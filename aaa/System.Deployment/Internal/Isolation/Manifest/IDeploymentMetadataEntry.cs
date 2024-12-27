using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B5 RID: 437
	[Guid("CFA3F59F-334D-46bf-A5A5-5D11BB2D7EBC")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IDeploymentMetadataEntry
	{
		// Token: 0x170001EF RID: 495
		// (get) Token: 0x060007F5 RID: 2037
		DeploymentMetadataEntry AllData { get; }

		// Token: 0x170001F0 RID: 496
		// (get) Token: 0x060007F6 RID: 2038
		string DeploymentProviderCodebase
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001F1 RID: 497
		// (get) Token: 0x060007F7 RID: 2039
		string MinimumRequiredVersion
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001F2 RID: 498
		// (get) Token: 0x060007F8 RID: 2040
		ushort MaximumAge { get; }

		// Token: 0x170001F3 RID: 499
		// (get) Token: 0x060007F9 RID: 2041
		byte MaximumAge_Unit { get; }

		// Token: 0x170001F4 RID: 500
		// (get) Token: 0x060007FA RID: 2042
		uint DeploymentFlags { get; }
	}
}
