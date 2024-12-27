using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000D6 RID: 214
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a5c637bf-6eaa-4e5f-b535-55299657e33e")]
	[ComImport]
	internal interface IEnumSTORE_ASSEMBLY
	{
		// Token: 0x06000330 RID: 816
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_ASSEMBLY[] rgelt);

		// Token: 0x06000331 RID: 817
		void Skip([In] uint celt);

		// Token: 0x06000332 RID: 818
		void Reset();

		// Token: 0x06000333 RID: 819
		IEnumSTORE_ASSEMBLY Clone();
	}
}
