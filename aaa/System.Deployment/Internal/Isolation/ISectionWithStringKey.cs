using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x020001D8 RID: 472
	[Guid("285a8871-c84a-11d7-850f-005cd062464f")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISectionWithStringKey
	{
		// Token: 0x06000864 RID: 2148
		void Lookup([MarshalAs(UnmanagedType.LPWStr)] string wzStringKey, [MarshalAs(UnmanagedType.Interface)] out object ppUnknown);

		// Token: 0x1700024A RID: 586
		// (get) Token: 0x06000865 RID: 2149
		bool IsCaseInsensitive { get; }
	}
}
