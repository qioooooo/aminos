using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x02000066 RID: 102
	[Guid("1113f52d-dc7f-4943-aed6-88d04027e32a")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	public interface IProcessInitializer
	{
		// Token: 0x06000224 RID: 548
		void Startup([MarshalAs(UnmanagedType.IUnknown)] [In] object punkProcessControl);

		// Token: 0x06000225 RID: 549
		void Shutdown();
	}
}
