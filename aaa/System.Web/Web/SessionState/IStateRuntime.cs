using System;
using System.Runtime.InteropServices;
using System.Security.Permissions;

namespace System.Web.SessionState
{
	// Token: 0x0200037C RID: 892
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[Guid("7297744b-e188-40bf-b7e9-56698d25cf44")]
	[AspNetHostingPermission(SecurityAction.LinkDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
	[ComImport]
	public interface IStateRuntime
	{
		// Token: 0x06002B52 RID: 11090
		void StopProcessing();

		// Token: 0x06002B53 RID: 11091
		void ProcessRequest([MarshalAs(UnmanagedType.SysInt)] [In] IntPtr tracker, [MarshalAs(UnmanagedType.I4)] [In] int verb, [MarshalAs(UnmanagedType.LPWStr)] [In] string uri, [MarshalAs(UnmanagedType.I4)] [In] int exclusive, [MarshalAs(UnmanagedType.I4)] [In] int timeout, [MarshalAs(UnmanagedType.I4)] [In] int lockCookieExists, [MarshalAs(UnmanagedType.I4)] [In] int lockCookie, [MarshalAs(UnmanagedType.I4)] [In] int contentLength, [MarshalAs(UnmanagedType.SysInt)] [In] IntPtr content);

		// Token: 0x06002B54 RID: 11092
		void ProcessRequest([MarshalAs(UnmanagedType.SysInt)] [In] IntPtr tracker, [MarshalAs(UnmanagedType.I4)] [In] int verb, [MarshalAs(UnmanagedType.LPWStr)] [In] string uri, [MarshalAs(UnmanagedType.I4)] [In] int exclusive, [MarshalAs(UnmanagedType.I4)] [In] int extraFlags, [MarshalAs(UnmanagedType.I4)] [In] int timeout, [MarshalAs(UnmanagedType.I4)] [In] int lockCookieExists, [MarshalAs(UnmanagedType.I4)] [In] int lockCookie, [MarshalAs(UnmanagedType.I4)] [In] int contentLength, [MarshalAs(UnmanagedType.SysInt)] [In] IntPtr content);
	}
}
