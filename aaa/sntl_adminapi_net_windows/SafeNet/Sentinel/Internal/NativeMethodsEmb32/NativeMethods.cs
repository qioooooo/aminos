using System;
using System.Runtime.InteropServices;

namespace SafeNet.Sentinel.Internal.NativeMethodsEmb32
{
	// Token: 0x02000010 RID: 16
	internal static class NativeMethods
	{
		// Token: 0x06000051 RID: 81
		[DllImport("apidsp_windows.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_context_new_scope(ref IntPtr context, [MarshalAs(UnmanagedType.LPStr)] string scope);

		// Token: 0x06000052 RID: 82
		[DllImport("apidsp_windows.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_context_new(ref IntPtr context, [MarshalAs(UnmanagedType.LPStr)] string hostname, ushort sntl_admin_u16_t, [MarshalAs(UnmanagedType.LPStr)] string password);

		// Token: 0x06000053 RID: 83
		[DllImport("apidsp_windows.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_context_delete(IntPtr context);

		// Token: 0x06000054 RID: 84
		[DllImport("apidsp_windows.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_get(IntPtr context, [MarshalAs(UnmanagedType.LPStr)] string scope, [MarshalAs(UnmanagedType.LPStr)] string format, ref IntPtr info);

		// Token: 0x06000055 RID: 85
		[DllImport("apidsp_windows.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_set(IntPtr context, [MarshalAs(UnmanagedType.LPStr)] string action, ref IntPtr return_status);

		// Token: 0x06000056 RID: 86
		[DllImport("apidsp_windows.dll", CharSet = CharSet.Auto)]
		internal static extern void sntl_admin_free(IntPtr info);

		// Token: 0x06000057 RID: 87
		[DllImport("apidsp_windows.dll")]
		internal static extern AdminStatus hasp_set_lib_path(IntPtr path);

		// Token: 0x06000058 RID: 88
		[DllImport("apidsp_windows.dll")]
		internal static extern AdminStatus hasp_set_lib_path(int path);
	}
}
