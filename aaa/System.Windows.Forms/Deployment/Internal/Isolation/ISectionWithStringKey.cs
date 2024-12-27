using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000047 RID: 71
	[Guid("285a8871-c84a-11d7-850f-005cd062464f")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISectionWithStringKey
	{
		// Token: 0x060001F5 RID: 501
		void Lookup([MarshalAs(UnmanagedType.LPWStr)] string wzStringKey, [MarshalAs(UnmanagedType.Interface)] out object ppUnknown);

		// Token: 0x1700005D RID: 93
		// (get) Token: 0x060001F6 RID: 502
		bool IsCaseInsensitive { get; }
	}
}
