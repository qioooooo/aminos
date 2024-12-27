using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001AD RID: 429
	[Guid("1E0422A1-F0D2-44ae-914B-8A2DECCFD22B")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ICLRSurrogateEntry
	{
		// Token: 0x1700028D RID: 653
		// (get) Token: 0x06001456 RID: 5206
		CLRSurrogateEntry AllData { get; }

		// Token: 0x1700028E RID: 654
		// (get) Token: 0x06001457 RID: 5207
		Guid Clsid { get; }

		// Token: 0x1700028F RID: 655
		// (get) Token: 0x06001458 RID: 5208
		string RuntimeVersion
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000290 RID: 656
		// (get) Token: 0x06001459 RID: 5209
		string ClassName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
