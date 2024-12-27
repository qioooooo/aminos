using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B2 RID: 434
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("CB73147E-5FC2-4c31-B4E6-58D13DBE1A08")]
	[ComImport]
	internal interface IDescriptionMetadataEntry
	{
		// Token: 0x170001E8 RID: 488
		// (get) Token: 0x060007ED RID: 2029
		DescriptionMetadataEntry AllData { get; }

		// Token: 0x170001E9 RID: 489
		// (get) Token: 0x060007EE RID: 2030
		string Publisher
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001EA RID: 490
		// (get) Token: 0x060007EF RID: 2031
		string Product
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001EB RID: 491
		// (get) Token: 0x060007F0 RID: 2032
		string SupportUrl
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001EC RID: 492
		// (get) Token: 0x060007F1 RID: 2033
		string IconFile
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001ED RID: 493
		// (get) Token: 0x060007F2 RID: 2034
		string ErrorReportUrl
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001EE RID: 494
		// (get) Token: 0x060007F3 RID: 2035
		string SuiteName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
