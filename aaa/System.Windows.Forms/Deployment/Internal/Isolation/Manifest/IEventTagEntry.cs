using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020000B2 RID: 178
	[Guid("8AD3FC86-AFD3-477a-8FD5-146C291195BD")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEventTagEntry
	{
		// Token: 0x1700010F RID: 271
		// (get) Token: 0x060002E2 RID: 738
		EventTagEntry AllData { get; }

		// Token: 0x17000110 RID: 272
		// (get) Token: 0x060002E3 RID: 739
		string TagData
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}

		// Token: 0x17000111 RID: 273
		// (get) Token: 0x060002E4 RID: 740
		uint EventID { get; }
	}
}
