using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001D3 RID: 467
	internal struct STORE_ASSEMBLY
	{
		// Token: 0x040007FC RID: 2044
		public uint Status;

		// Token: 0x040007FD RID: 2045
		public IDefinitionIdentity DefinitionIdentity;

		// Token: 0x040007FE RID: 2046
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ManifestPath;

		// Token: 0x040007FF RID: 2047
		public ulong AssemblySize;

		// Token: 0x04000800 RID: 2048
		public ulong ChangeId;
	}
}
