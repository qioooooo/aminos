using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000176 RID: 374
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("24abe1f7-a396-4a03-9adf-1d5b86a5569f")]
	[ComImport]
	internal interface IMuiResourceIdLookupMapEntry
	{
		// Token: 0x17000185 RID: 389
		// (get) Token: 0x06000764 RID: 1892
		MuiResourceIdLookupMapEntry AllData { get; }

		// Token: 0x17000186 RID: 390
		// (get) Token: 0x06000765 RID: 1893
		uint Count { get; }
	}
}
