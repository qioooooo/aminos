using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200018F RID: 399
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("55b2dec1-d0f6-4bf4-91b1-30f73ad8e4df")]
	[ComImport]
	internal interface IMuiResourceTypeIdIntEntry
	{
		// Token: 0x17000255 RID: 597
		// (get) Token: 0x0600140B RID: 5131
		MuiResourceTypeIdIntEntry AllData { get; }

		// Token: 0x17000256 RID: 598
		// (get) Token: 0x0600140C RID: 5132
		object StringIds
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x17000257 RID: 599
		// (get) Token: 0x0600140D RID: 5133
		object IntegerIds
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
	}
}
