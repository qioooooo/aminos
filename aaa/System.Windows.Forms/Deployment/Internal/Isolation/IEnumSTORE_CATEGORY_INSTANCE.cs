using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000DE RID: 222
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("5ba7cb30-8508-4114-8c77-262fcda4fadb")]
	[ComImport]
	internal interface IEnumSTORE_CATEGORY_INSTANCE
	{
		// Token: 0x0600035C RID: 860
		uint Next([In] uint ulElements, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_CATEGORY_INSTANCE[] rgInstances);

		// Token: 0x0600035D RID: 861
		void Skip([In] uint ulElements);

		// Token: 0x0600035E RID: 862
		void Reset();

		// Token: 0x0600035F RID: 863
		IEnumSTORE_CATEGORY_INSTANCE Clone();
	}
}
