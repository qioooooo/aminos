using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x020000C2 RID: 194
	[Guid("227AC7A8-8423-42ce-B7CF-03061EC9AAA3")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ICreateWithLocalTransaction
	{
		// Token: 0x06000484 RID: 1156
		[return: MarshalAs(UnmanagedType.Interface)]
		object CreateInstanceWithSysTx(ITransactionProxy pTransaction, [MarshalAs(UnmanagedType.LPStruct)] [In] Guid rclsid, [MarshalAs(UnmanagedType.LPStruct)] [In] Guid riid);
	}
}
