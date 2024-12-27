using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000FF RID: 255
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("5fa4f590-a416-4b22-ac79-7c3f0d31f303")]
	[ComImport]
	internal interface IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY
	{
		// Token: 0x0600060F RID: 1551
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] StoreOperationMetadataProperty[] AppIds);

		// Token: 0x06000610 RID: 1552
		void Skip([In] uint celt);

		// Token: 0x06000611 RID: 1553
		void Reset();

		// Token: 0x06000612 RID: 1554
		IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY Clone();
	}
}
