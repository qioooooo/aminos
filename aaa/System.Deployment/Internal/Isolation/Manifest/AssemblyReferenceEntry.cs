using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200019E RID: 414
	[StructLayout(LayoutKind.Sequential)]
	internal class AssemblyReferenceEntry
	{
		// Token: 0x0400070F RID: 1807
		public IReferenceIdentity ReferenceIdentity;

		// Token: 0x04000710 RID: 1808
		public uint Flags;

		// Token: 0x04000711 RID: 1809
		public AssemblyReferenceDependentAssemblyEntry DependentAssembly;
	}
}
