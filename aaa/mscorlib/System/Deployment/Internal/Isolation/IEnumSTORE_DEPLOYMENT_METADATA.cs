using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001DD RID: 477
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("f9fd4090-93db-45c0-af87-624940f19cff")]
	[ComImport]
	internal interface IEnumSTORE_DEPLOYMENT_METADATA
	{
		// Token: 0x060014C5 RID: 5317
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] IDefinitionAppId[] AppIds);

		// Token: 0x060014C6 RID: 5318
		void Skip([In] uint celt);

		// Token: 0x060014C7 RID: 5319
		void Reset();

		// Token: 0x060014C8 RID: 5320
		IEnumSTORE_DEPLOYMENT_METADATA Clone();
	}
}
