using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000A0 RID: 160
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("CB73147E-5FC2-4c31-B4E6-58D13DBE1A08")]
	[ComImport]
	internal interface IDescriptionMetadataEntry
	{
		// Token: 0x170000D8 RID: 216
		// (get) Token: 0x060002A2 RID: 674
		DescriptionMetadataEntry AllData { get; }

		// Token: 0x170000D9 RID: 217
		// (get) Token: 0x060002A3 RID: 675
		string Publisher
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000DA RID: 218
		// (get) Token: 0x060002A4 RID: 676
		string Product
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000DB RID: 219
		// (get) Token: 0x060002A5 RID: 677
		string SupportUrl
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000DC RID: 220
		// (get) Token: 0x060002A6 RID: 678
		string IconFile
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000DD RID: 221
		// (get) Token: 0x060002A7 RID: 679
		string ErrorReportUrl
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000DE RID: 222
		// (get) Token: 0x060002A8 RID: 680
		string SuiteName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
