using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000B5 RID: 181
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("49e1fe8d-ebb8-4593-8c4e-3e14c845b142")]
	[ComImport]
	internal interface IRegistryValueEntry
	{
		// Token: 0x17000112 RID: 274
		// (get) Token: 0x060002E6 RID: 742
		RegistryValueEntry AllData { get; }

		// Token: 0x17000113 RID: 275
		// (get) Token: 0x060002E7 RID: 743
		uint Flags { get; }

		// Token: 0x17000114 RID: 276
		// (get) Token: 0x060002E8 RID: 744
		uint OperationHint { get; }

		// Token: 0x17000115 RID: 277
		// (get) Token: 0x060002E9 RID: 745
		uint Type { get; }

		// Token: 0x17000116 RID: 278
		// (get) Token: 0x060002EA RID: 746
		string Value
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000117 RID: 279
		// (get) Token: 0x060002EB RID: 747
		string BuildFilter
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
