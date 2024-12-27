using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000C1 RID: 193
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8CD3FC85-AFD3-477a-8FD5-146C291195BB")]
	[ComImport]
	internal interface ICounterSetEntry
	{
		// Token: 0x17000127 RID: 295
		// (get) Token: 0x06000305 RID: 773
		CounterSetEntry AllData { get; }

		// Token: 0x17000128 RID: 296
		// (get) Token: 0x06000306 RID: 774
		Guid CounterSetGuid { get; }

		// Token: 0x17000129 RID: 297
		// (get) Token: 0x06000307 RID: 775
		Guid ProviderGuid { get; }

		// Token: 0x1700012A RID: 298
		// (get) Token: 0x06000308 RID: 776
		string Name
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700012B RID: 299
		// (get) Token: 0x06000309 RID: 777
		string Description
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700012C RID: 300
		// (get) Token: 0x0600030A RID: 778
		bool InstanceType { get; }
	}
}
