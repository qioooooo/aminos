using System;
using System.Runtime.InteropServices;

namespace System.Management
{
	// Token: 0x020000BE RID: 190
	[TypeLibType(512)]
	[InterfaceType(1)]
	[Guid("DC12A687-737F-11CF-884D-00AA004B2E24")]
	[ComImport]
	internal interface IWbemLocator
	{
		// Token: 0x060005C3 RID: 1475
		[PreserveSig]
		int ConnectServer_([MarshalAs(UnmanagedType.BStr)] [In] string strNetworkResource, [MarshalAs(UnmanagedType.BStr)] [In] string strUser, [In] IntPtr strPassword, [MarshalAs(UnmanagedType.BStr)] [In] string strLocale, [In] int lSecurityFlags, [MarshalAs(UnmanagedType.BStr)] [In] string strAuthority, [MarshalAs(UnmanagedType.Interface)] [In] IWbemContext pCtx, [MarshalAs(UnmanagedType.Interface)] out IWbemServices ppNamespace);
	}
}
