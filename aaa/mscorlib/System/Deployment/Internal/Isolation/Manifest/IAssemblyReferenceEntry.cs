using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation.Manifest
{
	// Token: 0x020001B3 RID: 435
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("FD47B733-AFBC-45e4-B7C2-BBEB1D9F766C")]
	[ComImport]
	internal interface IAssemblyReferenceEntry
	{
		// Token: 0x1700029C RID: 668
		// (get) Token: 0x0600146A RID: 5226
		AssemblyReferenceEntry AllData { get; }

		// Token: 0x1700029D RID: 669
		// (get) Token: 0x0600146B RID: 5227
		IReferenceIdentity ReferenceIdentity { get; }

		// Token: 0x1700029E RID: 670
		// (get) Token: 0x0600146C RID: 5228
		uint Flags { get; }

		// Token: 0x1700029F RID: 671
		// (get) Token: 0x0600146D RID: 5229
		IAssemblyReferenceDependentAssemblyEntry DependentAssembly { get; }
	}
}
