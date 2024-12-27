using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001E5 RID: 485
	[Guid("b840a2f5-a497-4a6d-9038-cd3ec2fbd222")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_CATEGORY
	{
		// Token: 0x060014F1 RID: 5361
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_CATEGORY[] rgElements);

		// Token: 0x060014F2 RID: 5362
		void Skip([In] uint ulElements);

		// Token: 0x060014F3 RID: 5363
		void Reset();

		// Token: 0x060014F4 RID: 5364
		IEnumSTORE_CATEGORY Clone();
	}
}
