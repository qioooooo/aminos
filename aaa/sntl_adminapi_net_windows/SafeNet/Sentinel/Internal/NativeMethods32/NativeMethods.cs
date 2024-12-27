using System;
using System.Runtime.InteropServices;

namespace SafeNet.Sentinel.Internal.NativeMethods32
{
	// Token: 0x0200000C RID: 12
	internal static class NativeMethods
	{
		// Token: 0x06000035 RID: 53
		[DllImport("sntl_adminapi_windows.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_context_new_scope(ref IntPtr context, [MarshalAs(UnmanagedType.LPStr)] string scope);

		// Token: 0x06000036 RID: 54
		[DllImport("sntl_adminapi_windows.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_context_new(ref IntPtr context, [MarshalAs(UnmanagedType.LPStr)] string hostname, ushort sntl_admin_u16_t, [MarshalAs(UnmanagedType.LPStr)] string password);

		// Token: 0x06000037 RID: 55
		[DllImport("sntl_adminapi_windows.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_context_delete(IntPtr context);

		// Token: 0x06000038 RID: 56
		[DllImport("sntl_adminapi_windows.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_get(IntPtr context, [MarshalAs(UnmanagedType.LPStr)] string scope, [MarshalAs(UnmanagedType.LPStr)] string format, ref IntPtr info);

		// Token: 0x06000039 RID: 57
		[DllImport("sntl_adminapi_windows.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_set(IntPtr context, [MarshalAs(UnmanagedType.LPStr)] string action, ref IntPtr return_status);

		// Token: 0x0600003A RID: 58
		[DllImport("sntl_adminapi_windows.dll", CharSet = CharSet.Auto)]
		internal static extern void sntl_admin_free(IntPtr info);
	}
}
