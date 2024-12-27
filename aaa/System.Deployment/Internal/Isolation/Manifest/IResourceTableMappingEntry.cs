using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A6 RID: 422
	[Guid("70A4ECEE-B195-4c59-85BF-44B6ACA83F07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IResourceTableMappingEntry
	{
		// Token: 0x170001D9 RID: 473
		// (get) Token: 0x060007DA RID: 2010
		ResourceTableMappingEntry AllData { get; }

		// Token: 0x170001DA RID: 474
		// (get) Token: 0x060007DB RID: 2011
		string id
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001DB RID: 475
		// (get) Token: 0x060007DC RID: 2012
		string FinalStringMapped
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
