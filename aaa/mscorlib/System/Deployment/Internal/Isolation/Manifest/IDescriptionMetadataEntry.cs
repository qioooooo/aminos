using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C5 RID: 453
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("CB73147E-5FC2-4c31-B4E6-58D13DBE1A08")]
	[ComImport]
	internal interface IDescriptionMetadataEntry
	{
		// Token: 0x170002B3 RID: 691
		// (get) Token: 0x06001487 RID: 5255
		DescriptionMetadataEntry AllData { get; }

		// Token: 0x170002B4 RID: 692
		// (get) Token: 0x06001488 RID: 5256
		string Publisher
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002B5 RID: 693
		// (get) Token: 0x06001489 RID: 5257
		string Product
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002B6 RID: 694
		// (get) Token: 0x0600148A RID: 5258
		string SupportUrl
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002B7 RID: 695
		// (get) Token: 0x0600148B RID: 5259
		string IconFile
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002B8 RID: 696
		// (get) Token: 0x0600148C RID: 5260
		string ErrorReportUrl
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002B9 RID: 697
		// (get) Token: 0x0600148D RID: 5261
		string SuiteName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
