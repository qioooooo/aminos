using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000192 RID: 402
	[Guid("397927f5-10f2-4ecb-bfe1-3c264212a193")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IMuiResourceMapEntry
	{
		// Token: 0x17000258 RID: 600
		// (get) Token: 0x06001412 RID: 5138
		MuiResourceMapEntry AllData { get; }

		// Token: 0x17000259 RID: 601
		// (get) Token: 0x06001413 RID: 5139
		object ResourceTypeIdInt
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x1700025A RID: 602
		// (get) Token: 0x06001414 RID: 5140
		object ResourceTypeIdString
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
	}
}
