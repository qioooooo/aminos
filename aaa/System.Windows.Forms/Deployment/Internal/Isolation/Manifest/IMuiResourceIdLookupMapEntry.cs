using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000064 RID: 100
	[Guid("24abe1f7-a396-4a03-9adf-1d5b86a5569f")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IMuiResourceIdLookupMapEntry
	{
		// Token: 0x17000075 RID: 117
		// (get) Token: 0x06000219 RID: 537
		MuiResourceIdLookupMapEntry AllData { get; }

		// Token: 0x17000076 RID: 118
		// (get) Token: 0x0600021A RID: 538
		uint Count { get; }
	}
}
