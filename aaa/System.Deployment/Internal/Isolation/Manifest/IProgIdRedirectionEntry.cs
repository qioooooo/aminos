using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000197 RID: 407
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("54F198EC-A63A-45ea-A984-452F68D9B35B")]
	[ComImport]
	internal interface IProgIdRedirectionEntry
	{
		// Token: 0x170001BF RID: 447
		// (get) Token: 0x060007B8 RID: 1976
		ProgIdRedirectionEntry AllData { get; }

		// Token: 0x170001C0 RID: 448
		// (get) Token: 0x060007B9 RID: 1977
		string ProgId
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001C1 RID: 449
		// (get) Token: 0x060007BA RID: 1978
		Guid RedirectedGuid { get; }
	}
}
