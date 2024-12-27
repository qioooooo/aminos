using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200008C RID: 140
	[StructLayout(LayoutKind.Sequential)]
	internal class AssemblyReferenceEntry
	{
		// Token: 0x04000C85 RID: 3205
		public IReferenceIdentity ReferenceIdentity;

		// Token: 0x04000C86 RID: 3206
		public uint Flags;

		// Token: 0x04000C87 RID: 3207
		public AssemblyReferenceDependentAssemblyEntry DependentAssembly;
	}
}
