using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001DC RID: 476
	[Guid("d8b1aacb-5142-4abb-bcc1-e9dc9052a89e")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE
	{
		// Token: 0x060014C1 RID: 5313
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] StoreApplicationReference[] rgelt);

		// Token: 0x060014C2 RID: 5314
		void Skip([In] uint celt);

		// Token: 0x060014C3 RID: 5315
		void Reset();

		// Token: 0x060014C4 RID: 5316
		IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE Clone();
	}
}
