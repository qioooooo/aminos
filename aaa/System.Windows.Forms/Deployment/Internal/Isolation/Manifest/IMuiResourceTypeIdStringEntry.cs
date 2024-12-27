using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000067 RID: 103
	[Guid("11df5cad-c183-479b-9a44-3842b71639ce")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IMuiResourceTypeIdStringEntry
	{
		// Token: 0x17000077 RID: 119
		// (get) Token: 0x0600021F RID: 543
		MuiResourceTypeIdStringEntry AllData { get; }

		// Token: 0x17000078 RID: 120
		// (get) Token: 0x06000220 RID: 544
		object StringIds
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x17000079 RID: 121
		// (get) Token: 0x06000221 RID: 545
		object IntegerIds
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
	}
}
