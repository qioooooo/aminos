using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200006A RID: 106
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("55b2dec1-d0f6-4bf4-91b1-30f73ad8e4df")]
	[ComImport]
	internal interface IMuiResourceTypeIdIntEntry
	{
		// Token: 0x1700007A RID: 122
		// (get) Token: 0x06000226 RID: 550
		MuiResourceTypeIdIntEntry AllData { get; }

		// Token: 0x1700007B RID: 123
		// (get) Token: 0x06000227 RID: 551
		object StringIds
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x1700007C RID: 124
		// (get) Token: 0x06000228 RID: 552
		object IntegerIds
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
	}
}
