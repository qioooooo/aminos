using System;
using System.Runtime.InteropServices;
using SafeNet.Sentinel.Internal.NativeMethodsEmb32;

namespace SafeNet.Sentinel.Internal
{
	// Token: 0x0200000F RID: 15
	internal class AdminApiDspEmb32 : IAdminApiDsp
	{
		// Token: 0x06000049 RID: 73 RVA: 0x00002C74 File Offset: 0x00000E74
		public AdminApiDspEmb32()
		{
			IntPtr zero = IntPtr.Zero;
		}

		// Token: 0x0600004A RID: 74 RVA: 0x00002C90 File Offset: 0x00000E90
		~AdminApiDspEmb32()
		{
		}

		// Token: 0x0600004B RID: 75 RVA: 0x00002CBC File Offset: 0x00000EBC
		public AdminStatus sntl_admin_context_new_scope(string scope)
		{
			return NativeMethods.sntl_admin_context_new_scope(ref this.context, scope);
		}

		// Token: 0x0600004C RID: 76 RVA: 0x00002CDC File Offset: 0x00000EDC
		public AdminStatus sntl_admin_context_new(string hostname, ushort port, string password)
		{
			return NativeMethods.sntl_admin_context_new(ref this.context, hostname, port, password);
		}

		// Token: 0x0600004D RID: 77 RVA: 0x00002CFC File Offset: 0x00000EFC
		public AdminStatus sntl_admin_context_delete()
		{
			return NativeMethods.sntl_admin_context_delete(this.context);
		}

		// Token: 0x0600004E RID: 78 RVA: 0x00002D1C File Offset: 0x00000F1C
		public AdminStatus sntl_admin_get(string scope, string format, ref string info)
		{
			IntPtr zero = IntPtr.Zero;
			AdminStatus adminStatus = NativeMethods.sntl_admin_get(this.context, scope, format, ref zero);
			info = Marshal.PtrToStringAnsi(zero);
			if (zero != IntPtr.Zero)
			{
				NativeMethods.sntl_admin_free(zero);
			}
			return adminStatus;
		}

		// Token: 0x0600004F RID: 79 RVA: 0x00002D68 File Offset: 0x00000F68
		public AdminStatus sntl_admin_set(string action, ref string return_status)
		{
			IntPtr zero = IntPtr.Zero;
			AdminStatus adminStatus = NativeMethods.sntl_admin_set(this.context, action, ref zero);
			return_status = Marshal.PtrToStringAnsi(zero);
			if (zero != IntPtr.Zero)
			{
				NativeMethods.sntl_admin_free(zero);
			}
			return adminStatus;
		}

		// Token: 0x06000050 RID: 80 RVA: 0x00002DB4 File Offset: 0x00000FB4
		public void setLibPath(string path)
		{
			IntPtr intPtr = IntPtr.Zero;
			if (path != null)
			{
				intPtr = Marshal.StringToHGlobalAnsi(path);
				NativeMethods.hasp_set_lib_path(intPtr);
			}
			else
			{
				NativeMethods.hasp_set_lib_path(0);
			}
			if (intPtr != IntPtr.Zero)
			{
				Marshal.FreeHGlobal(intPtr);
			}
		}

		// Token: 0x04000048 RID: 72
		private IntPtr context;
	}
}
