using System;
using System.Runtime.InteropServices;

namespace System.EnterpriseServices
{
	// Token: 0x0200000A RID: 10
	[Guid("51372AE0-CAE7-11CF-BE81-00AA00A2FA25")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface IObjectContext
	{
		// Token: 0x06000012 RID: 18
		[return: MarshalAs(UnmanagedType.Interface)]
		object CreateInstance([MarshalAs(UnmanagedType.LPStruct)] Guid rclsid, [MarshalAs(UnmanagedType.LPStruct)] Guid riid);

		// Token: 0x06000013 RID: 19
		void SetComplete();

		// Token: 0x06000014 RID: 20
		void SetAbort();

		// Token: 0x06000015 RID: 21
		void EnableCommit();

		// Token: 0x06000016 RID: 22
		void DisableCommit();

		// Token: 0x06000017 RID: 23
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsInTransaction();

		// Token: 0x06000018 RID: 24
		[PreserveSig]
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsSecurityEnabled();

		// Token: 0x06000019 RID: 25
		[return: MarshalAs(UnmanagedType.Bool)]
		bool IsCallerInRole([MarshalAs(UnmanagedType.BStr)] [In] string role);
	}
}
