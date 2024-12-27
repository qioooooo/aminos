using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001C7 RID: 455
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("49e1fe8d-ebb8-4593-8c4e-3e14c845b142")]
	[ComImport]
	internal interface IRegistryValueEntry
	{
		// Token: 0x17000222 RID: 546
		// (get) Token: 0x06000831 RID: 2097
		RegistryValueEntry AllData { get; }

		// Token: 0x17000223 RID: 547
		// (get) Token: 0x06000832 RID: 2098
		uint Flags { get; }

		// Token: 0x17000224 RID: 548
		// (get) Token: 0x06000833 RID: 2099
		uint OperationHint { get; }

		// Token: 0x17000225 RID: 549
		// (get) Token: 0x06000834 RID: 2100
		uint Type { get; }

		// Token: 0x17000226 RID: 550
		// (get) Token: 0x06000835 RID: 2101
		string Value
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000227 RID: 551
		// (get) Token: 0x06000836 RID: 2102
		string BuildFilter
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
