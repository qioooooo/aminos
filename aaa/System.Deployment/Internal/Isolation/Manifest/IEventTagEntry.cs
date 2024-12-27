using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C4 RID: 452
	[Guid("8AD3FC86-AFD3-477a-8FD5-146C291195BD")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEventTagEntry
	{
		// Token: 0x1700021F RID: 543
		// (get) Token: 0x0600082D RID: 2093
		EventTagEntry AllData { get; }

		// Token: 0x17000220 RID: 544
		// (get) Token: 0x0600082E RID: 2094
		string TagData
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000221 RID: 545
		// (get) Token: 0x0600082F RID: 2095
		uint EventID { get; }
	}
}
