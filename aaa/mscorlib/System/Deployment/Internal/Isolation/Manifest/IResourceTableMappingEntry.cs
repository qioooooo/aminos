using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B9 RID: 441
	[Guid("70A4ECEE-B195-4c59-85BF-44B6ACA83F07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IResourceTableMappingEntry
	{
		// Token: 0x170002A4 RID: 676
		// (get) Token: 0x06001474 RID: 5236
		ResourceTableMappingEntry AllData { get; }

		// Token: 0x170002A5 RID: 677
		// (get) Token: 0x06001475 RID: 5237
		string id
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002A6 RID: 678
		// (get) Token: 0x06001476 RID: 5238
		string FinalStringMapped
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
