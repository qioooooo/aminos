using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001E3 RID: 483
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("a5c6aaa3-03e4-478d-b9f5-2e45908d5e4f")]
	[ComImport]
	internal interface IEnumSTORE_ASSEMBLY_FILE
	{
		// Token: 0x060014E6 RID: 5350
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_ASSEMBLY_FILE[] rgelt);

		// Token: 0x060014E7 RID: 5351
		void Skip([In] uint celt);

		// Token: 0x060014E8 RID: 5352
		void Reset();

		// Token: 0x060014E9 RID: 5353
		IEnumSTORE_ASSEMBLY_FILE Clone();
	}
}
