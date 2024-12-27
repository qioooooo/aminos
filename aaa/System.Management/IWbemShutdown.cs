using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000C4 RID: 196
	[Guid("B7B31DF9-D515-11D3-A11C-00105A1F515A")]
	[InterfaceType(1)]
	[ComImport]
	internal interface IWbemShutdown
	{
		// Token: 0x060005EF RID: 1519
		[PreserveSig]
		int Shutdown_([In] int uReason, [In] uint uMaxMilliseconds, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx);
	}
}
