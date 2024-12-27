using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001A0 RID: 416
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("FD47B733-AFBC-45e4-B7C2-BBEB1D9F766C")]
	[ComImport]
	internal interface IAssemblyReferenceEntry
	{
		// Token: 0x170001D1 RID: 465
		// (get) Token: 0x060007D0 RID: 2000
		AssemblyReferenceEntry AllData { get; }

		// Token: 0x170001D2 RID: 466
		// (get) Token: 0x060007D1 RID: 2001
		IReferenceIdentity ReferenceIdentity { get; }

		// Token: 0x170001D3 RID: 467
		// (get) Token: 0x060007D2 RID: 2002
		uint Flags { get; }

		// Token: 0x170001D4 RID: 468
		// (get) Token: 0x060007D3 RID: 2003
		IAssemblyReferenceDependentAssemblyEntry DependentAssembly { get; }
	}
}
