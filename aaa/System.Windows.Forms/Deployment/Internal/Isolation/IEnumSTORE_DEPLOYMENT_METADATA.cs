using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000D2 RID: 210
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("f9fd4090-93db-45c0-af87-624940f19cff")]
	[ComImport]
	internal interface IEnumSTORE_DEPLOYMENT_METADATA
	{
		// Token: 0x0600031A RID: 794
		uint Next([In] uint celt, [MarshalAs(UnmanagedType.LPArray)] [Out] IDefinitionAppId[] AppIds);

		// Token: 0x0600031B RID: 795
		void Skip([In] uint celt);

		// Token: 0x0600031C RID: 796
		void Reset();

		// Token: 0x0600031D RID: 797
		IEnumSTORE_DEPLOYMENT_METADATA Clone();
	}
}
