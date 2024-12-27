using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200018B RID: 395
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("DA0C3B27-6B6B-4b80-A8F8-6CE14F4BC0A4")]
	[ComImport]
	internal interface ICategoryMembershipDataEntry
	{
		// Token: 0x170001AC RID: 428
		// (get) Token: 0x060007A1 RID: 1953
		CategoryMembershipDataEntry AllData { get; }

		// Token: 0x170001AD RID: 429
		// (get) Token: 0x060007A2 RID: 1954
		uint index { get; }

		// Token: 0x170001AE RID: 430
		// (get) Token: 0x060007A3 RID: 1955
		string Xml
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001AF RID: 431
		// (get) Token: 0x060007A4 RID: 1956
		string Description
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
