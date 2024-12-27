using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200006D RID: 109
	[Guid("397927f5-10f2-4ecb-bfe1-3c264212a193")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IMuiResourceMapEntry
	{
		// Token: 0x1700007D RID: 125
		// (get) Token: 0x0600022D RID: 557
		MuiResourceMapEntry AllData { get; }

		// Token: 0x1700007E RID: 126
		// (get) Token: 0x0600022E RID: 558
		object ResourceTypeIdInt
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x1700007F RID: 127
		// (get) Token: 0x0600022F RID: 559
		object ResourceTypeIdString
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
	}
}
