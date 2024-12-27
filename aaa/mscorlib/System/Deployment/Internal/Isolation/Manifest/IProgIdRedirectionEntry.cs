using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001AA RID: 426
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("54F198EC-A63A-45ea-A984-452F68D9B35B")]
	[ComImport]
	internal interface IProgIdRedirectionEntry
	{
		// Token: 0x1700028A RID: 650
		// (get) Token: 0x06001452 RID: 5202
		ProgIdRedirectionEntry AllData { get; }

		// Token: 0x1700028B RID: 651
		// (get) Token: 0x06001453 RID: 5203
		string ProgId
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700028C RID: 652
		// (get) Token: 0x06001454 RID: 5204
		Guid RedirectedGuid { get; }
	}
}
