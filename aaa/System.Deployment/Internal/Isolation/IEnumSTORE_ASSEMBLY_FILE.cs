using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000103 RID: 259
	[Guid("a5c6aaa3-03e4-478d-b9f5-2e45908d5e4f")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_ASSEMBLY_FILE
	{
		// Token: 0x06000625 RID: 1573
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_ASSEMBLY_FILE[] rgelt);

		// Token: 0x06000626 RID: 1574
		void Skip([In] uint celt);

		// Token: 0x06000627 RID: 1575
		void Reset();

		// Token: 0x06000628 RID: 1576
		IEnumSTORE_ASSEMBLY_FILE Clone();
	}
}
