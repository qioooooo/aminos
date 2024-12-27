using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using System.Web.Services.Interop;

namespace System.Web.Services
{
	// Token: 0x02000010 RID: 16
	[ComVisible(false)]
	[SuppressUnmanagedCodeSecurity]
	[SecurityPermission(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
	internal class UnsafeNativeMethods
	{
		// Token: 0x0600001C RID: 28 RVA: 0x0000237C File Offset: 0x0000137C
		private UnsafeNativeMethods()
		{
		}

		// Token: 0x0600001D RID: 29
		[DllImport("ole32.dll", ExactSpelling = true)]
		internal static extern int CoCreateInstance([In] ref Guid clsid, [MarshalAs(UnmanagedType.Interface)] object punkOuter, int context, [In] ref Guid iid, [MarshalAs(UnmanagedType.Interface)] out object punk);

		// Token: 0x0600001E RID: 30 RVA: 0x00002384 File Offset: 0x00001384
		internal static INotifySink2 RegisterNotifySource(INotifyConnection2 connection, INotifySource2 source)
		{
			return connection.RegisterNotifySource(source);
		}

		// Token: 0x0600001F RID: 31 RVA: 0x0000238D File Offset: 0x0000138D
		internal static void UnregisterNotifySource(INotifyConnection2 connection, INotifySource2 source)
		{
			connection.UnregisterNotifySource(source);
		}

		// Token: 0x06000020 RID: 32 RVA: 0x00002396 File Offset: 0x00001396
		internal static void OnSyncCallOut(INotifySink2 sink, CallId callId, out IntPtr out_ppBuffer, ref int inout_pBufferSize)
		{
			sink.OnSyncCallOut(callId, out out_ppBuffer, ref inout_pBufferSize);
		}

		// Token: 0x06000021 RID: 33 RVA: 0x000023A1 File Offset: 0x000013A1
		internal static void OnSyncCallEnter(INotifySink2 sink, CallId callId, byte[] in_pBuffer, int in_BufferSize)
		{
			sink.OnSyncCallEnter(callId, in_pBuffer, in_BufferSize);
		}

		// Token: 0x06000022 RID: 34 RVA: 0x000023AC File Offset: 0x000013AC
		internal static void OnSyncCallReturn(INotifySink2 sink, CallId callId, byte[] in_pBuffer, int in_BufferSize)
		{
			sink.OnSyncCallReturn(callId, in_pBuffer, in_BufferSize);
		}

		// Token: 0x06000023 RID: 35 RVA: 0x000023B7 File Offset: 0x000013B7
		internal static void OnSyncCallExit(INotifySink2 sink, CallId callId, out IntPtr out_ppBuffer, ref int inout_pBufferSize)
		{
			sink.OnSyncCallExit(callId, out out_ppBuffer, ref inout_pBufferSize);
		}
	}
}
