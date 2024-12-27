using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200018C RID: 396
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("11df5cad-c183-479b-9a44-3842b71639ce")]
	[ComImport]
	internal interface IMuiResourceTypeIdStringEntry
	{
		// Token: 0x17000252 RID: 594
		// (get) Token: 0x06001404 RID: 5124
		MuiResourceTypeIdStringEntry AllData { get; }

		// Token: 0x17000253 RID: 595
		// (get) Token: 0x06001405 RID: 5125
		object StringIds
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x17000254 RID: 596
		// (get) Token: 0x06001406 RID: 5126
		object IntegerIds
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
	}
}
