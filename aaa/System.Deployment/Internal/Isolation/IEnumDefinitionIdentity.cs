using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000110 RID: 272
	[Guid("f3549d9c-fc73-4793-9c00-1cd204254c0c")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumDefinitionIdentity
	{
		// Token: 0x0600066C RID: 1644
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] IDefinitionIdentity[] DefinitionIdentity);

		// Token: 0x0600066D RID: 1645
		void Skip([In] uint celt);

		// Token: 0x0600066E RID: 1646
		void Reset();

		// Token: 0x0600066F RID: 1647
		IEnumDefinitionIdentity Clone();
	}
}
