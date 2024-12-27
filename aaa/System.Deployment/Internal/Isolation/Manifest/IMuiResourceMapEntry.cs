using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200017F RID: 383
	[Guid("397927f5-10f2-4ecb-bfe1-3c264212a193")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IMuiResourceMapEntry
	{
		// Token: 0x1700018D RID: 397
		// (get) Token: 0x06000778 RID: 1912
		MuiResourceMapEntry AllData { get; }

		// Token: 0x1700018E RID: 398
		// (get) Token: 0x06000779 RID: 1913
		object ResourceTypeIdInt
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x1700018F RID: 399
		// (get) Token: 0x0600077A RID: 1914
		object ResourceTypeIdString
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
	}
}
