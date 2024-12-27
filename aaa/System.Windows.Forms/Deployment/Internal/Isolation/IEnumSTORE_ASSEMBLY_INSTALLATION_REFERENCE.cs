using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000D1 RID: 209
	[Guid("d8b1aacb-5142-4abb-bcc1-e9dc9052a89e")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE
	{
		// Token: 0x06000316 RID: 790
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] StoreApplicationReference[] rgelt);

		// Token: 0x06000317 RID: 791
		void Skip([In] uint celt);

		// Token: 0x06000318 RID: 792
		void Reset();

		// Token: 0x06000319 RID: 793
		IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE Clone();
	}
}
