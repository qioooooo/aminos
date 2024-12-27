using System;
using System.Runtime.InteropServices;

namespace System.Deployment.Internal.Isolation
{
	// Token: 0x02000173 RID: 371
	[Guid("285a8871-c84a-11d7-850f-005cd062464f")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ISectionWithStringKey
	{
		// Token: 0x060013DA RID: 5082
		void Lookup([MarshalAs(UnmanagedType.LPWStr)] string wzStringKey, [MarshalAs(UnmanagedType.Interface)] out object ppUnknown);

		// Token: 0x17000238 RID: 568
		// (get) Token: 0x060013DB RID: 5083
		bool IsCaseInsensitive { get; }
	}
}
