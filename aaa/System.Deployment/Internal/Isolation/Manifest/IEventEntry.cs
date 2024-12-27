using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001BE RID: 446
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8AD3FC86-AFD3-477a-8FD5-146C291195BB")]
	[ComImport]
	internal interface IEventEntry
	{
		// Token: 0x17000211 RID: 529
		// (get) Token: 0x0600081D RID: 2077
		EventEntry AllData { get; }

		// Token: 0x17000212 RID: 530
		// (get) Token: 0x0600081E RID: 2078
		uint EventID { get; }

		// Token: 0x17000213 RID: 531
		// (get) Token: 0x0600081F RID: 2079
		uint Level { get; }

		// Token: 0x17000214 RID: 532
		// (get) Token: 0x06000820 RID: 2080
		uint Version { get; }

		// Token: 0x17000215 RID: 533
		// (get) Token: 0x06000821 RID: 2081
		Guid Guid { get; }

		// Token: 0x17000216 RID: 534
		// (get) Token: 0x06000822 RID: 2082
		string SubTypeName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000217 RID: 535
		// (get) Token: 0x06000823 RID: 2083
		uint SubTypeValue { get; }

		// Token: 0x17000218 RID: 536
		// (get) Token: 0x06000824 RID: 2084
		string DisplayName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000219 RID: 537
		// (get) Token: 0x06000825 RID: 2085
		uint EventNameMicrodomIndex { get; }
	}
}
