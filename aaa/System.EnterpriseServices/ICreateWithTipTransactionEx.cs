using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x020000C0 RID: 192
	[Guid("455ACF59-5345-11D2-99CF-00C04F797BC9")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface ICreateWithTipTransactionEx
	{
		// Token: 0x06000482 RID: 1154
		[return: MarshalAs(UnmanagedType.Interface)]
		object CreateInstance(string bstrTipUrl, [MarshalAs(UnmanagedType.LPStruct)] [In] Guid rclsid, [MarshalAs(UnmanagedType.LPStruct)] [In] Guid riid);
	}
}
