using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x0200004B RID: 75
	[Guid("285a8860-c84a-11d7-850f-005cd062464f")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ICDF
	{
		// Token: 0x060001FE RID: 510
		ISection GetRootSection(uint SectionId);

		// Token: 0x060001FF RID: 511
		ISectionEntry GetRootSectionEntry(uint SectionId);

		// Token: 0x1700005E RID: 94
		// (get) Token: 0x06000200 RID: 512
		object _NewEnum
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x1700005F RID: 95
		// (get) Token: 0x06000201 RID: 513
		uint Count { get; }

		// Token: 0x06000202 RID: 514
		object GetItem(uint SectionId);
	}
}
