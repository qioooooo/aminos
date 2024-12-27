using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000105 RID: 261
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("b840a2f5-a497-4a6d-9038-cd3ec2fbd222")]
	[ComImport]
	internal interface IEnumSTORE_CATEGORY
	{
		// Token: 0x06000630 RID: 1584
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_CATEGORY[] rgElements);

		// Token: 0x06000631 RID: 1585
		void Skip([In] uint ulElements);

		// Token: 0x06000632 RID: 1586
		void Reset();

		// Token: 0x06000633 RID: 1587
		IEnumSTORE_CATEGORY Clone();
	}
}
