using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000D4 RID: 212
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("5fa4f590-a416-4b22-ac79-7c3f0d31f303")]
	[ComImport]
	internal interface IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY
	{
		// Token: 0x06000325 RID: 805
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] StoreOperationMetadataProperty[] AppIds);

		// Token: 0x06000326 RID: 806
		void Skip([In] uint celt);

		// Token: 0x06000327 RID: 807
		void Reset();

		// Token: 0x06000328 RID: 808
		IEnumSTORE_DEPLOYMENT_METADATA_PROPERTY Clone();
	}
}
