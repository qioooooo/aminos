using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000046 RID: 70
	[Guid("285a8862-c84a-11d7-850f-005cd062464f")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISection
	{
		// Token: 0x17000059 RID: 89
		// (get) Token: 0x060001F1 RID: 497
		object _NewEnum
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x1700005A RID: 90
		// (get) Token: 0x060001F2 RID: 498
		uint Count { get; }

		// Token: 0x1700005B RID: 91
		// (get) Token: 0x060001F3 RID: 499
		uint SectionID { get; }

		// Token: 0x1700005C RID: 92
		// (get) Token: 0x060001F4 RID: 500
		string SectionName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
