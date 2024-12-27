using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001E9 RID: 489
	[Guid("5ba7cb30-8508-4114-8c77-262fcda4fadb")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_CATEGORY_INSTANCE
	{
		// Token: 0x06001507 RID: 5383
		uint Next([In] uint ulElements, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_CATEGORY_INSTANCE[] rgInstances);

		// Token: 0x06001508 RID: 5384
		void Skip([In] uint ulElements);

		// Token: 0x06001509 RID: 5385
		void Reset();

		// Token: 0x0600150A RID: 5386
		IEnumSTORE_CATEGORY_INSTANCE Clone();
	}
}
