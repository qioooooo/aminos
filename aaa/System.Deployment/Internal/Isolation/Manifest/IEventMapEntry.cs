using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C1 RID: 449
	[Guid("8AD3FC86-AFD3-477a-8FD5-146C291195BC")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEventMapEntry
	{
		// Token: 0x1700021A RID: 538
		// (get) Token: 0x06000827 RID: 2087
		EventMapEntry AllData { get; }

		// Token: 0x1700021B RID: 539
		// (get) Token: 0x06000828 RID: 2088
		string MapName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700021C RID: 540
		// (get) Token: 0x06000829 RID: 2089
		string Name
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700021D RID: 541
		// (get) Token: 0x0600082A RID: 2090
		uint Value { get; }

		// Token: 0x1700021E RID: 542
		// (get) Token: 0x0600082B RID: 2091
		bool IsValueMap { get; }
	}
}
