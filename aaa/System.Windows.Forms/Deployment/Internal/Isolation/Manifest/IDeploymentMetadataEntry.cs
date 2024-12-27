using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000A3 RID: 163
	[Guid("CFA3F59F-334D-46bf-A5A5-5D11BB2D7EBC")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IDeploymentMetadataEntry
	{
		// Token: 0x170000DF RID: 223
		// (get) Token: 0x060002AA RID: 682
		DeploymentMetadataEntry AllData { get; }

		// Token: 0x170000E0 RID: 224
		// (get) Token: 0x060002AB RID: 683
		string DeploymentProviderCodebase
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000E1 RID: 225
		// (get) Token: 0x060002AC RID: 684
		string MinimumRequiredVersion
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000E2 RID: 226
		// (get) Token: 0x060002AD RID: 685
		ushort MaximumAge { get; }

		// Token: 0x170000E3 RID: 227
		// (get) Token: 0x060002AE RID: 686
		byte MaximumAge_Unit { get; }

		// Token: 0x170000E4 RID: 228
		// (get) Token: 0x060002AF RID: 687
		uint DeploymentFlags { get; }
	}
}
