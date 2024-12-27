using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B6 RID: 438
	[Guid("8AD3FC86-AFD3-477a-8FD5-146C291195BA")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IWindowClassEntry
	{
		// Token: 0x170002A0 RID: 672
		// (get) Token: 0x0600146F RID: 5231
		WindowClassEntry AllData { get; }

		// Token: 0x170002A1 RID: 673
		// (get) Token: 0x06001470 RID: 5232
		string ClassName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002A2 RID: 674
		// (get) Token: 0x06001471 RID: 5233
		string HostDll
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170002A3 RID: 675
		// (get) Token: 0x06001472 RID: 5234
		bool fVersioned { get; }
	}
}
