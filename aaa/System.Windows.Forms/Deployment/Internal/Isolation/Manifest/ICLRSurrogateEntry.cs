using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000088 RID: 136
	[Guid("1E0422A1-F0D2-44ae-914B-8A2DECCFD22B")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ICLRSurrogateEntry
	{
		// Token: 0x170000B2 RID: 178
		// (get) Token: 0x06000271 RID: 625
		CLRSurrogateEntry AllData { get; }

		// Token: 0x170000B3 RID: 179
		// (get) Token: 0x06000272 RID: 626
		Guid Clsid { get; }

		// Token: 0x170000B4 RID: 180
		// (get) Token: 0x06000273 RID: 627
		string RuntimeVersion
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000B5 RID: 181
		// (get) Token: 0x06000274 RID: 628
		string ClassName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
