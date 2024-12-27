using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A3 RID: 419
	[Guid("8AD3FC86-AFD3-477a-8FD5-146C291195BA")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IWindowClassEntry
	{
		// Token: 0x170001D5 RID: 469
		// (get) Token: 0x060007D5 RID: 2005
		WindowClassEntry AllData { get; }

		// Token: 0x170001D6 RID: 470
		// (get) Token: 0x060007D6 RID: 2006
		string ClassName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001D7 RID: 471
		// (get) Token: 0x060007D7 RID: 2007
		string HostDll
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170001D8 RID: 472
		// (get) Token: 0x060007D8 RID: 2008
		bool fVersioned { get; }
	}
}
