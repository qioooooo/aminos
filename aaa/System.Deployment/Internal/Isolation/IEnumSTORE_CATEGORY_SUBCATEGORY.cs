using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000107 RID: 263
	[Guid("19be1967-b2fc-4dc1-9627-f3cb6305d2a7")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_CATEGORY_SUBCATEGORY
	{
		// Token: 0x0600063B RID: 1595
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_CATEGORY_SUBCATEGORY[] rgElements);

		// Token: 0x0600063C RID: 1596
		void Skip([In] uint ulElements);

		// Token: 0x0600063D RID: 1597
		void Reset();

		// Token: 0x0600063E RID: 1598
		IEnumSTORE_CATEGORY_SUBCATEGORY Clone();
	}
}
