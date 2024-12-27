using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001CB RID: 459
	[Guid("CF168CF4-4E8F-4d92-9D2A-60E5CA21CF85")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IDependentOSMetadataEntry
	{
		// Token: 0x170002C0 RID: 704
		// (get) Token: 0x06001496 RID: 5270
		DependentOSMetadataEntry AllData { get; }

		// Token: 0x170002C1 RID: 705
		// (get) Token: 0x06001497 RID: 5271
		string SupportUrl
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002C2 RID: 706
		// (get) Token: 0x06001498 RID: 5272
		string Description
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002C3 RID: 707
		// (get) Token: 0x06001499 RID: 5273
		ushort MajorVersion { get; }

		// Token: 0x170002C4 RID: 708
		// (get) Token: 0x0600149A RID: 5274
		ushort MinorVersion { get; }

		// Token: 0x170002C5 RID: 709
		// (get) Token: 0x0600149B RID: 5275
		ushort BuildNumber { get; }

		// Token: 0x170002C6 RID: 710
		// (get) Token: 0x0600149C RID: 5276
		byte ServicePackMajor { get; }

		// Token: 0x170002C7 RID: 711
		// (get) Token: 0x0600149D RID: 5277
		byte ServicePackMinor { get; }
	}
}
