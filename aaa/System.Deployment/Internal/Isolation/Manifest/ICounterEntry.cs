using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001D6 RID: 470
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("8CD3FC86-AFD3-477a-8FD5-146C291195BB")]
	[ComImport]
	internal interface ICounterEntry
	{
		// Token: 0x1700023D RID: 573
		// (get) Token: 0x06000857 RID: 2135
		CounterEntry AllData { get; }

		// Token: 0x1700023E RID: 574
		// (get) Token: 0x06000858 RID: 2136
		Guid CounterSetGuid { get; }

		// Token: 0x1700023F RID: 575
		// (get) Token: 0x06000859 RID: 2137
		uint CounterId { get; }

		// Token: 0x17000240 RID: 576
		// (get) Token: 0x0600085A RID: 2138
		string Name
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000241 RID: 577
		// (get) Token: 0x0600085B RID: 2139
		string Description
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000242 RID: 578
		// (get) Token: 0x0600085C RID: 2140
		uint CounterType { get; }

		// Token: 0x17000243 RID: 579
		// (get) Token: 0x0600085D RID: 2141
		ulong Attributes { get; }

		// Token: 0x17000244 RID: 580
		// (get) Token: 0x0600085E RID: 2142
		uint BaseId { get; }

		// Token: 0x17000245 RID: 581
		// (get) Token: 0x0600085F RID: 2143
		uint DefaultScale { get; }
	}
}
