using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000BB RID: 187
	[Guid("9f27c750-7dfb-46a1-a673-52e53e2337a9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IDirectoryEntry
	{
		// Token: 0x1700011F RID: 287
		// (get) Token: 0x060002FB RID: 763
		DirectoryEntry AllData { get; }

		// Token: 0x17000120 RID: 288
		// (get) Token: 0x060002FC RID: 764
		uint Flags { get; }

		// Token: 0x17000121 RID: 289
		// (get) Token: 0x060002FD RID: 765
		uint Protection { get; }

		// Token: 0x17000122 RID: 290
		// (get) Token: 0x060002FE RID: 766
		string BuildFilter
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000123 RID: 291
		// (get) Token: 0x060002FF RID: 767
		object SecurityDescriptor
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}
	}
}
