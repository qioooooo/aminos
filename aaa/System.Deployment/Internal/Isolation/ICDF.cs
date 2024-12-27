using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001DC RID: 476
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("285a8860-c84a-11d7-850f-005cd062464f")]
	[ComImport]
	internal interface ICDF
	{
		// Token: 0x0600086D RID: 2157
		ISection GetRootSection(uint SectionId);

		// Token: 0x0600086E RID: 2158
		ISectionEntry GetRootSectionEntry(uint SectionId);

		// Token: 0x1700024B RID: 587
		// (get) Token: 0x0600086F RID: 2159
		object _NewEnum
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x1700024C RID: 588
		// (get) Token: 0x06000870 RID: 2160
		uint Count { get; }

		// Token: 0x06000871 RID: 2161
		object GetItem(uint SectionId);
	}
}
