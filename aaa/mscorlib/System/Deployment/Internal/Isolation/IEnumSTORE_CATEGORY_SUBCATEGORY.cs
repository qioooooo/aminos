﻿using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001E7 RID: 487
	[Guid("19be1967-b2fc-4dc1-9627-f3cb6305d2a7")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_CATEGORY_SUBCATEGORY
	{
		// Token: 0x060014FC RID: 5372
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_CATEGORY_SUBCATEGORY[] rgElements);

		// Token: 0x060014FD RID: 5373
		void Skip([In] uint ulElements);

		// Token: 0x060014FE RID: 5374
		void Reset();

		// Token: 0x060014FF RID: 5375
		IEnumSTORE_CATEGORY_SUBCATEGORY Clone();
	}
}
