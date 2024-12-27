using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001DF RID: 479
	[Guid("5fa4f590-a416-4b22-ac79-7c3f0d31f303")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY
	{
		// Token: 0x060014D0 RID: 5328
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] StoreOperationMetadataProperty[] AppIds);

		// Token: 0x060014D1 RID: 5329
		void Skip([In] uint celt);

		// Token: 0x060014D2 RID: 5330
		void Reset();

		// Token: 0x060014D3 RID: 5331
		IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY Clone();
	}
}
