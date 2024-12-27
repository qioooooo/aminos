using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000177 RID: 375
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("285a8860-c84a-11d7-850f-005cd062464f")]
	[ComImport]
	internal interface ICDF
	{
		// Token: 0x060013E3 RID: 5091
		ISection GetRootSection(uint SectionId);

		// Token: 0x060013E4 RID: 5092
		ISectionEntry GetRootSectionEntry(uint SectionId);

		// Token: 0x17000239 RID: 569
		// (get) Token: 0x060013E5 RID: 5093
		object _NewEnum
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x1700023A RID: 570
		// (get) Token: 0x060013E6 RID: 5094
		uint Count { get; }

		// Token: 0x060013E7 RID: 5095
		object GetItem(uint SectionId);
	}
}
