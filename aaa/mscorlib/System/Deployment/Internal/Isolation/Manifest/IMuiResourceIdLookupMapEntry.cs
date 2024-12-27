using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000189 RID: 393
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("24abe1f7-a396-4a03-9adf-1d5b86a5569f")]
	[ComImport]
	internal interface IMuiResourceIdLookupMapEntry
	{
		// Token: 0x17000250 RID: 592
		// (get) Token: 0x060013FE RID: 5118
		MuiResourceIdLookupMapEntry AllData { get; }

		// Token: 0x17000251 RID: 593
		// (get) Token: 0x060013FF RID: 5119
		uint Count { get; }
	}
}
