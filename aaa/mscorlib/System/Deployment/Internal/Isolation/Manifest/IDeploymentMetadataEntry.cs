using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C8 RID: 456
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("CFA3F59F-334D-46bf-A5A5-5D11BB2D7EBC")]
	[ComImport]
	internal interface IDeploymentMetadataEntry
	{
		// Token: 0x170002BA RID: 698
		// (get) Token: 0x0600148F RID: 5263
		DeploymentMetadataEntry AllData { get; }

		// Token: 0x170002BB RID: 699
		// (get) Token: 0x06001490 RID: 5264
		string DeploymentProviderCodebase
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002BC RID: 700
		// (get) Token: 0x06001491 RID: 5265
		string MinimumRequiredVersion
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002BD RID: 701
		// (get) Token: 0x06001492 RID: 5266
		ushort MaximumAge { get; }

		// Token: 0x170002BE RID: 702
		// (get) Token: 0x06001493 RID: 5267
		byte MaximumAge_Unit { get; }

		// Token: 0x170002BF RID: 703
		// (get) Token: 0x06001494 RID: 5268
		uint DeploymentFlags { get; }
	}
}
