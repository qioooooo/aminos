using System;
using System.Runtime.InteropServices;
using System.Security;

namespace System.Web.Services.Interop
{
	// Token: 0x0200001D RID: 29
	[InterfaceType(ComInterfaceType.InterfaceIsIUnknown)]
	[SuppressUnmanagedCodeSecurity]
	[Guid("C43CC2F3-90AF-4e93-9112-DFB8B36749B5")]
	[ComImport]
	internal interface INotifySink2
	{
		// Token: 0x0600006D RID: 109
		void OnSyncCallOut([In] CallId callId, out IntPtr out_ppBuffer, [In] [Out] ref int inout_pBufferSize);

		// Token: 0x0600006E RID: 110
		void OnSyncCallEnter([In] CallId callId, [MarshalAs(UnmanagedType.LPArray)] [In] byte[] in_pBuffer, [In] int in_BufferSize);

		// Token: 0x0600006F RID: 111
		void OnSyncCallReturn([In] CallId callId, [MarshalAs(UnmanagedType.LPArray)] [In] byte[] in_pBuffer, [In] int in_BufferSize);

		// Token: 0x06000070 RID: 112
		void OnSyncCallExit([In] CallId callId, out IntPtr out_ppBuffer, [In] [Out] ref int inout_pBufferSize);
	}
}
