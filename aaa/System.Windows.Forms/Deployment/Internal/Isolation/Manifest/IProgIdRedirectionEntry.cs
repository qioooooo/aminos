using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000085 RID: 133
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("54F198EC-A63A-45ea-A984-452F68D9B35B")]
	[ComImport]
	internal interface IProgIdRedirectionEntry
	{
		// Token: 0x170000AF RID: 175
		// (get) Token: 0x0600026D RID: 621
		ProgIdRedirectionEntry AllData { get; }

		// Token: 0x170000B0 RID: 176
		// (get) Token: 0x0600026E RID: 622
		string ProgId
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000B1 RID: 177
		// (get) Token: 0x0600026F RID: 623
		Guid RedirectedGuid { get; }
	}
}
