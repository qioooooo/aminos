using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x020000BD RID: 189
	[Guid("C5FEB7C1-346A-11D1-B1CC-00AA00BA3258")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ITransactionResourcePool
	{
		// Token: 0x0600046E RID: 1134
		[PreserveSig]
		int PutResource(IntPtr pPool, [MarshalAs(UnmanagedType.Interface)] object pUnk);

		// Token: 0x0600046F RID: 1135
		[PreserveSig]
		int GetResource(IntPtr pPool, [MarshalAs(UnmanagedType.Interface)] out object obj);
	}
}
