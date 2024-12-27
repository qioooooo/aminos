using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000E5 RID: 229
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("f3549d9c-fc73-4793-9c00-1cd204254c0c")]
	[ComImport]
	internal interface IEnumDefinitionIdentity
	{
		// Token: 0x06000382 RID: 898
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] IDefinitionIdentity[] DefinitionIdentity);

		// Token: 0x06000383 RID: 899
		void Skip([In] uint celt);

		// Token: 0x06000384 RID: 900
		void Reset();

		// Token: 0x06000385 RID: 901
		IEnumDefinitionIdentity Clone();
	}
}
