using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001D3 RID: 467
	[Guid("8CD3FC85-AFD3-477a-8FD5-146C291195BB")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ICounterSetEntry
	{
		// Token: 0x17000237 RID: 567
		// (get) Token: 0x06000850 RID: 2128
		CounterSetEntry AllData { get; }

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x06000851 RID: 2129
		Guid CounterSetGuid { get; }

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x06000852 RID: 2130
		Guid ProviderGuid { get; }

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x06000853 RID: 2131
		string Name
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700023B RID: 571
		// (get) Token: 0x06000854 RID: 2132
		string Description
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700023C RID: 572
		// (get) Token: 0x06000855 RID: 2133
		bool InstanceType { get; }
	}
}
