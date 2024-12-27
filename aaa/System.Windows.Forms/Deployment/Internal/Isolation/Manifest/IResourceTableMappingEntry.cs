using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000094 RID: 148
	[Guid("70A4ECEE-B195-4c59-85BF-44B6ACA83F07")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IResourceTableMappingEntry
	{
		// Token: 0x170000C9 RID: 201
		// (get) Token: 0x0600028F RID: 655
		ResourceTableMappingEntry AllData { get; }

		// Token: 0x170000CA RID: 202
		// (get) Token: 0x06000290 RID: 656
		string id
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000CB RID: 203
		// (get) Token: 0x06000291 RID: 657
		string FinalStringMapped
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
