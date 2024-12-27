using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000B8 RID: 184
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("186685d1-6673-48c3-bc83-95859bb591df")]
	[ComImport]
	internal interface IRegistryKeyEntry
	{
		// Token: 0x17000118 RID: 280
		// (get) Token: 0x060002F0 RID: 752
		RegistryKeyEntry AllData { get; }

		// Token: 0x17000119 RID: 281
		// (get) Token: 0x060002F1 RID: 753
		uint Flags { get; }

		// Token: 0x1700011A RID: 282
		// (get) Token: 0x060002F2 RID: 754
		uint Protection { get; }

		// Token: 0x1700011B RID: 283
		// (get) Token: 0x060002F3 RID: 755
		string BuildFilter
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700011C RID: 284
		// (get) Token: 0x060002F4 RID: 756
		object SecurityDescriptor
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x1700011D RID: 285
		// (get) Token: 0x060002F5 RID: 757
		object Values
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x1700011E RID: 286
		// (get) Token: 0x060002F6 RID: 758
		object Keys
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
	}
}
