using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000C4 RID: 196
	[Guid("8CD3FC86-AFD3-477a-8FD5-146C291195BB")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ICounterEntry
	{
		// Token: 0x1700012D RID: 301
		// (get) Token: 0x0600030C RID: 780
		CounterEntry AllData { get; }

		// Token: 0x1700012E RID: 302
		// (get) Token: 0x0600030D RID: 781
		Guid CounterSetGuid { get; }

		// Token: 0x1700012F RID: 303
		// (get) Token: 0x0600030E RID: 782
		uint CounterId { get; }

		// Token: 0x17000130 RID: 304
		// (get) Token: 0x0600030F RID: 783
		string Name
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000131 RID: 305
		// (get) Token: 0x06000310 RID: 784
		string Description
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000132 RID: 306
		// (get) Token: 0x06000311 RID: 785
		uint CounterType { get; }

		// Token: 0x17000133 RID: 307
		// (get) Token: 0x06000312 RID: 786
		ulong Attributes { get; }

		// Token: 0x17000134 RID: 308
		// (get) Token: 0x06000313 RID: 787
		uint BaseId { get; }

		// Token: 0x17000135 RID: 309
		// (get) Token: 0x06000314 RID: 788
		uint DefaultScale { get; }
	}
}
