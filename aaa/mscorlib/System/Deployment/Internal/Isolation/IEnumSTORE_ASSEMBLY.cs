using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001E1 RID: 481
	[Guid("a5c637bf-6eaa-4e5f-b535-55299657e33e")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_ASSEMBLY
	{
		// Token: 0x060014DB RID: 5339
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_ASSEMBLY[] rgelt);

		// Token: 0x060014DC RID: 5340
		void Skip([In] uint celt);

		// Token: 0x060014DD RID: 5341
		void Reset();

		// Token: 0x060014DE RID: 5342
		IEnumSTORE_ASSEMBLY Clone();
	}
}
