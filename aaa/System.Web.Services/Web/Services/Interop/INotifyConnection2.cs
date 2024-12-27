using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Web.Services.Interop
{
	// Token: 0x0200001C RID: 28
	[SuppressUnmanagedCodeSecurity]
	[Guid("1AF04045-6659-4aaa-9F4B-2741AC56224B")]
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[ComImport]
	internal interface INotifyConnection2
	{
		// Token: 0x0600006B RID: 107
		[return: MarshalAs(UnmanagedType.Interface)]
		INotifySink2 RegisterNotifySource([MarshalAs(UnmanagedType.Interface)] [In] INotifySource2 in_pNotifySource);

		// Token: 0x0600006C RID: 108
		void UnregisterNotifySource([MarshalAs(UnmanagedType.Interface)] [In] INotifySource2 in_pNotifySource);
	}
}
