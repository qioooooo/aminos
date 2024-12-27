using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000D8 RID: 216
	[Guid("a5c6aaa3-03e4-478d-b9f5-2e45908d5e4f")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_ASSEMBLY_FILE
	{
		// Token: 0x0600033B RID: 827
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] STORE_ASSEMBLY_FILE[] rgelt);

		// Token: 0x0600033C RID: 828
		void Skip([In] uint celt);

		// Token: 0x0600033D RID: 829
		void Reset();

		// Token: 0x0600033E RID: 830
		IEnumSTORE_ASSEMBLY_FILE Clone();
	}
}
