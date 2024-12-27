using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x02000091 RID: 145
	[Guid("8AD3FC86-AFD3-477a-8FD5-146C291195BA")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IWindowClassEntry
	{
		// Token: 0x170000C5 RID: 197
		// (get) Token: 0x0600028A RID: 650
		WindowClassEntry AllData { get; }

		// Token: 0x170000C6 RID: 198
		// (get) Token: 0x0600028B RID: 651
		string ClassName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000C7 RID: 199
		// (get) Token: 0x0600028C RID: 652
		string HostDll
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x170000C8 RID: 200
		// (get) Token: 0x0600028D RID: 653
		bool fVersioned { get; }
	}
}
