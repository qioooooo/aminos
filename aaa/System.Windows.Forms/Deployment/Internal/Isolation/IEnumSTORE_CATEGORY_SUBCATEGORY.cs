using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000DC RID: 220
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("19be1967-b2fc-4dc1-9627-f3cb6305d2a7")]
	[ComImport]
	internal interface IEnumSTORE_CATEGORY_SUBCATEGORY
	{
		// Token: 0x06000351 RID: 849
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_CATEGORY_SUBCATEGORY[] rgElements);

		// Token: 0x06000352 RID: 850
		void Skip([In] uint ulElements);

		// Token: 0x06000353 RID: 851
		void Reset();

		// Token: 0x06000354 RID: 852
		IEnumSTORE_CATEGORY_SUBCATEGORY Clone();
	}
}
