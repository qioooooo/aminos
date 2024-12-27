using System;
using System.Runtime.InteropServices;

namespace SafeNet.Sentinel.Internal.NativeMethods64
{
	// Token: 0x0200000E RID: 14
	internal static class NativeMethods
	{
		// Token: 0x06000043 RID: 67
		[DllImport("sntl_adminapi_windows_x64.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_context_new_scope(ref IntPtr context, [MarshalAs(UnmanagedType.LPStr)] string scope);

		// Token: 0x06000044 RID: 68
		[DllImport("sntl_adminapi_windows_x64.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_context_new(ref IntPtr context, [MarshalAs(UnmanagedType.LPStr)] string hostname, ushort sntl_admin_u16_t, [MarshalAs(UnmanagedType.LPStr)] string password);

		// Token: 0x06000045 RID: 69
		[DllImport("sntl_adminapi_windows_x64.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_context_delete(IntPtr context);

		// Token: 0x06000046 RID: 70
		[DllImport("sntl_adminapi_windows_x64.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_get(IntPtr context, [MarshalAs(UnmanagedType.LPStr)] string scope, [MarshalAs(UnmanagedType.LPStr)] string format, ref IntPtr info);

		// Token: 0x06000047 RID: 71
		[DllImport("sntl_adminapi_windows_x64.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_set(IntPtr context, [MarshalAs(UnmanagedType.LPStr)] string action, ref IntPtr return_status);

		// Token: 0x06000048 RID: 72
		[DllImport("sntl_adminapi_windows_x64.dll", CharSet = CharSet.Auto)]
		internal static extern void sntl_admin_free(IntPtr info);
	}
}
