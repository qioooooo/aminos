using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x0200008E RID: 142
	[Guid("FD47B733-AFBC-45e4-B7C2-BBEB1D9F766C")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IAssemblyReferenceEntry
	{
		// Token: 0x170000C1 RID: 193
		// (get) Token: 0x06000285 RID: 645
		AssemblyReferenceEntry AllData { get; }

		// Token: 0x170000C2 RID: 194
		// (get) Token: 0x06000286 RID: 646
		IReferenceIdentity ReferenceIdentity { get; }

		// Token: 0x170000C3 RID: 195
		// (get) Token: 0x06000287 RID: 647
		uint Flags { get; }

		// Token: 0x170000C4 RID: 196
		// (get) Token: 0x06000288 RID: 648
		IAssemblyReferenceDependentAssemblyEntry DependentAssembly { get; }
	}
}
