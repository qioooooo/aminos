using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000109 RID: 265
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("5ba7cb30-8508-4114-8c77-262fcda4fadb")]
	[ComImport]
	internal interface IEnumSTORE_CATEGORY_INSTANCE
	{
		// Token: 0x06000646 RID: 1606
		uint Next([In] uint ulElements, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_CATEGORY_INSTANCE[] rgInstances);

		// Token: 0x06000647 RID: 1607
		void Skip([In] uint ulElements);

		// Token: 0x06000648 RID: 1608
		void Reset();

		// Token: 0x06000649 RID: 1609
		IEnumSTORE_CATEGORY_INSTANCE Clone();
	}
}
