using System;
using System.Runtime.InteropServices;

namespace SafeNet.Sentinel.Internal.NativeMethodsEmb64
{
	// Token: 0x02000012 RID: 18
	internal static class NativeMethods
	{
		// Token: 0x06000061 RID: 97
		[DllImport("apidsp_windows_x64.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_context_new_scope(ref IntPtr context, [MarshalAs(UnmanagedType.LPStr)] string scope);

		// Token: 0x06000062 RID: 98
		[DllImport("apidsp_windows_x64.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_context_new(ref IntPtr context, [MarshalAs(UnmanagedType.LPStr)] string hostname, ushort sntl_admin_u16_t, [MarshalAs(UnmanagedType.LPStr)] string password);

		// Token: 0x06000063 RID: 99
		[DllImport("apidsp_windows_x64.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_context_delete(IntPtr context);

		// Token: 0x06000064 RID: 100
		[DllImport("apidsp_windows_x64.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_get(IntPtr context, [MarshalAs(UnmanagedType.LPStr)] string scope, [MarshalAs(UnmanagedType.LPStr)] string format, ref IntPtr info);

		// Token: 0x06000065 RID: 101
		[DllImport("apidsp_windows_x64.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus sntl_admin_set(IntPtr context, [MarshalAs(UnmanagedType.LPStr)] string action, ref IntPtr return_status);

		// Token: 0x06000066 RID: 102
		[DllImport("apidsp_windows_x64.dll", CharSet = CharSet.Auto)]
		internal static extern void sntl_admin_free(IntPtr info);

		// Token: 0x06000067 RID: 103
		[DllImport("apidsp_windows_x64.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus hasp_set_lib_path(IntPtr path);

		// Token: 0x06000068 RID: 104
		[DllImport("apidsp_windows_x64.dll", CharSet = CharSet.Auto)]
		internal static extern AdminStatus hasp_set_lib_path(int path);
	}
}
