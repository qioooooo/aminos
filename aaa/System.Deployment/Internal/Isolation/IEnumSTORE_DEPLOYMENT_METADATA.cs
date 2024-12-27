using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000FD RID: 253
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("f9fd4090-93db-45c0-af87-624940f19cff")]
	[ComImport]
	internal interface IEnumSTORE_DEPLOYMENT_METADATA
	{
		// Token: 0x06000604 RID: 1540
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] IDefinitionAppId[] AppIds);

		// Token: 0x06000605 RID: 1541
		void Skip([In] uint celt);

		// Token: 0x06000606 RID: 1542
		void Reset();

		// Token: 0x06000607 RID: 1543
		IEnumSTORE_DEPLOYMENT_METADATA Clone();
	}
}
