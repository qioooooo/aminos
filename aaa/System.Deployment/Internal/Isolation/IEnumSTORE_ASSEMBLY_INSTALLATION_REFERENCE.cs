using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000FC RID: 252
	[Guid("d8b1aacb-5142-4abb-bcc1-e9dc9052a89e")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE
	{
		// Token: 0x06000600 RID: 1536
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] StoreApplicationReference[] rgelt);

		// Token: 0x06000601 RID: 1537
		void Skip([In] uint celt);

		// Token: 0x06000602 RID: 1538
		void Reset();

		// Token: 0x06000603 RID: 1539
		IEnumSTORE_ASSEMBLY_INSTALLATION_REFERENCE Clone();
	}
}
