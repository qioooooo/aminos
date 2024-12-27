using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200019A RID: 410
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("1E0422A1-F0D2-44ae-914B-8A2DECCFD22B")]
	[ComImport]
	internal interface ICLRSurrogateEntry
	{
		// Token: 0x170001C2 RID: 450
		// (get) Token: 0x060007BC RID: 1980
		CLRSurrogateEntry AllData { get; }

		// Token: 0x170001C3 RID: 451
		// (get) Token: 0x060007BD RID: 1981
		Guid Clsid { get; }

		// Token: 0x170001C4 RID: 452
		// (get) Token: 0x060007BE RID: 1982
		string RuntimeVersion
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001C5 RID: 453
		// (get) Token: 0x060007BF RID: 1983
		string ClassName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
