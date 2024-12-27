using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000101 RID: 257
	[Guid("a5c637bf-6eaa-4e5f-b535-55299657e33e")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_ASSEMBLY
	{
		// Token: 0x0600061A RID: 1562
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_ASSEMBLY[] rgelt);

		// Token: 0x0600061B RID: 1563
		void Skip([In] uint celt);

		// Token: 0x0600061C RID: 1564
		void Reset();

		// Token: 0x0600061D RID: 1565
		IEnumSTORE_ASSEMBLY Clone();
	}
}
