using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000025 RID: 37
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("da91b74e-5388-4783-949d-c1cd5fb00506")]
	[ComImport]
	internal interface IManagedPoolAction
	{
		// Token: 0x06000081 RID: 129
		void LastRelease();
	}
}
