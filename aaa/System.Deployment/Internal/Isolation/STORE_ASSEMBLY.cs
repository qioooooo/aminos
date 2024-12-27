using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020000F3 RID: 243
	internal struct STORE_ASSEMBLY
	{
		// Token: 0x040004DA RID: 1242
		public uint Status;

		// Token: 0x040004DB RID: 1243
		public IDefinitionIdentity DefinitionIdentity;

		// Token: 0x040004DC RID: 1244
		[MarshalAs(UnmanagedType.LPWStr)]
		public string ManifestPath;

		// Token: 0x040004DD RID: 1245
		public ulong AssemblySize;

		// Token: 0x040004DE RID: 1246
		public ulong ChangeId;
	}
}
