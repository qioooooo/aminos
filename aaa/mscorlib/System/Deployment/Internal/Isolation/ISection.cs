using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000172 RID: 370
	[Guid("285a8862-c84a-11d7-850f-005cd062464f")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISection
	{
		// Token: 0x17000234 RID: 564
		// (get) Token: 0x060013D6 RID: 5078
		object _NewEnum
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x17000235 RID: 565
		// (get) Token: 0x060013D7 RID: 5079
		uint Count { get; }

		// Token: 0x17000236 RID: 566
		// (get) Token: 0x060013D8 RID: 5080
		uint SectionID { get; }

		// Token: 0x17000237 RID: 567
		// (get) Token: 0x060013D9 RID: 5081
		string SectionName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
