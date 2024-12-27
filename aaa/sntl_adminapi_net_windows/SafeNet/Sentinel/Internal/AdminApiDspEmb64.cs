using System;
using System.Runtime.InteropServices;
using SafeNet.Sentinel.Internal.NativeMethodsEmb64;

namespace SafeNet.Sentinel.Internal
{
	// Token: 0x02000011 RID: 17
	internal class AdminApiDspEmb64 : IAdminApiDsp
	{
		// Token: 0x06000059 RID: 89 RVA: 0x00002E04 File Offset: 0x00001004
		public AdminApiDspEmb64()
		{
			IntPtr zero = IntPtr.Zero;
		}

		// Token: 0x0600005A RID: 90 RVA: 0x00002E20 File Offset: 0x00001020
		~AdminApiDspEmb64()
		{
		}

		// Token: 0x0600005B RID: 91 RVA: 0x00002E4C File Offset: 0x0000104C
		public AdminStatus sntl_admin_context_new_scope(string scope)
		{
			return NativeMethods.sntl_admin_context_new_scope(ref this.context, scope);
		}

		// Token: 0x0600005C RID: 92 RVA: 0x00002E6C File Offset: 0x0000106C
		public AdminStatus sntl_admin_context_new(string hostname, ushort port, string password)
		{
			return NativeMethods.sntl_admin_context_new(ref this.context, hostname, port, password);
		}

		// Token: 0x0600005D RID: 93 RVA: 0x00002E8C File Offset: 0x0000108C
		public AdminStatus sntl_admin_context_delete()
		{
			return NativeMethods.sntl_admin_context_delete(this.context);
		}

		// Token: 0x0600005E RID: 94 RVA: 0x00002EAC File Offset: 0x000010AC
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

		// Token: 0x0600005F RID: 95 RVA: 0x00002EF8 File Offset: 0x000010F8
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

		// Token: 0x06000060 RID: 96 RVA: 0x00002F44 File Offset: 0x00001144
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

		// Token: 0x04000049 RID: 73
		private IntPtr context;
	}
}
