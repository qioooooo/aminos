using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000DA RID: 218
	[Guid("b840a2f5-a497-4a6d-9038-cd3ec2fbd222")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_CATEGORY
	{
		// Token: 0x06000346 RID: 838
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_CATEGORY[] rgElements);

		// Token: 0x06000347 RID: 839
		void Skip([In] uint ulElements);

		// Token: 0x06000348 RID: 840
		void Reset();

		// Token: 0x06000349 RID: 841
		IEnumSTORE_CATEGORY Clone();
	}
}
