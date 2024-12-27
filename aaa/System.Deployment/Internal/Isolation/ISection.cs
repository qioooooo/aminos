using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001D7 RID: 471
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("285a8862-c84a-11d7-850f-005cd062464f")]
	[ComImport]
	internal interface ISection
	{
		// Token: 0x17000246 RID: 582
		// (get) Token: 0x06000860 RID: 2144
		object _NewEnum
		{
			[return: MarshalAs(UnmanagedType.Interface)]
			get;
		}

		// Token: 0x17000247 RID: 583
		// (get) Token: 0x06000861 RID: 2145
		uint Count { get; }

		// Token: 0x17000248 RID: 584
		// (get) Token: 0x06000862 RID: 2146
		uint SectionID { get; }

		// Token: 0x17000249 RID: 585
		// (get) Token: 0x06000863 RID: 2147
		string SectionName
		{
			[return: MarshalAs(UnmanagedType.LPWStr)]
			get;
		}
	}
}
