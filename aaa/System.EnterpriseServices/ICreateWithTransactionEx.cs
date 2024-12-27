using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x020000C1 RID: 193
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("455ACF57-5345-11D2-99CF-00C04F797BC9")]
	[ComImport]
	internal interface ICreateWithTransactionEx
	{
		// Token: 0x06000483 RID: 1155
		[return: MarshalAs(UnmanagedType.Interface)]
		object CreateInstance(ITransaction pTransaction, [MarshalAs(UnmanagedType.LPStruct)] [In] Guid rclsid, [MarshalAs(UnmanagedType.LPStruct)] [In] Guid riid);
	}
}
