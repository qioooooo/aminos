using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B1 RID: 433
	[StructLayout(LayoutKind.Sequential)]
	internal class AssemblyReferenceEntry
	{
		// Token: 0x0400077F RID: 1919
		public IReferenceIdentity ReferenceIdentity;

		// Token: 0x04000780 RID: 1920
		public uint Flags;

		// Token: 0x04000781 RID: 1921
		public AssemblyReferenceDependentAssemblyEntry DependentAssembly;
	}
}
