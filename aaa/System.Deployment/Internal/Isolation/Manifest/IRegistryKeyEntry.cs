using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001CA RID: 458
	[Guid("186685d1-6673-48c3-bc83-95859bb591df")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IRegistryKeyEntry
	{
		// Token: 0x17000228 RID: 552
		// (get) Token: 0x0600083B RID: 2107
		RegistryKeyEntry AllData { get; }

		// Token: 0x17000229 RID: 553
		// (get) Token: 0x0600083C RID: 2108
		uint Flags { get; }

		// Token: 0x1700022A RID: 554
		// (get) Token: 0x0600083D RID: 2109
		uint Protection { get; }

		// Token: 0x1700022B RID: 555
		// (get) Token: 0x0600083E RID: 2110
		string BuildFilter
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x1700022C RID: 556
		// (get) Token: 0x0600083F RID: 2111
		object SecurityDescriptor
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x1700022D RID: 557
		// (get) Token: 0x06000840 RID: 2112
		object Values
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x1700022E RID: 558
		// (get) Token: 0x06000841 RID: 2113
		object Keys
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
	}
}
