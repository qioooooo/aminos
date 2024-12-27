using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001EE RID: 494
	[Guid("f3549d9c-fc73-4793-9c00-1cd204254c0c")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IEnumDefinitionIdentity
	{
		// Token: 0x0600151F RID: 5407
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] IDefinitionIdentity[] DefinitionIdentity);

		// Token: 0x06001520 RID: 5408
		void Skip([In] uint celt);

		// Token: 0x06001521 RID: 5409
		void Reset();

		// Token: 0x06001522 RID: 5410
		IEnumDefinitionIdentity Clone();
	}
}
