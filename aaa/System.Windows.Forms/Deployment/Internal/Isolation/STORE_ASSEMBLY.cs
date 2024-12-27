using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000C8 RID: 200
	internal struct STORE_ASSEMBLY
	{
		// Token: 0x04000D66 RID: 3430
		public uint Status;

		// Token: 0x04000D67 RID: 3431
		public IDefinitionIdentity DefinitionIdentity;

		// Token: 0x04000D68 RID: 3432
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ManifestPath;

		// Token: 0x04000D69 RID: 3433
		public ulong AssemblySize;

		// Token: 0x04000D6A RID: 3434
		public ulong ChangeId;
	}
}
