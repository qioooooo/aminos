using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200017C RID: 380
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("55b2dec1-d0f6-4bf4-91b1-30f73ad8e4df")]
	[ComImport]
	internal interface IMuiResourceTypeIdIntEntry
	{
		// Token: 0x1700018A RID: 394
		// (get) Token: 0x06000771 RID: 1905
		MuiResourceTypeIdIntEntry AllData { get; }

		// Token: 0x1700018B RID: 395
		// (get) Token: 0x06000772 RID: 1906
		object StringIds
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x1700018C RID: 396
		// (get) Token: 0x06000773 RID: 1907
		object IntegerIds
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
	}
}
