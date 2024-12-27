using System;
using System.Runtime.InteropServices;
using SafeNet.Sentinel.Internal.NativeMethods64;

namespace SafeNet.Sentinel.Internal
{
	// Token: 0x0200000D RID: 13
	internal class AdminApiDspWin64 : IAdminApiDsp
	{
		// Token: 0x0600003B RID: 59 RVA: 0x00002B30 File Offset: 0x00000D30
		public AdminApiDspWin64()
		{
			IntPtr zero = IntPtr.Zero;
		}

		// Token: 0x0600003C RID: 60 RVA: 0x00002B4C File Offset: 0x00000D4C
		~AdminApiDspWin64()
		{
		}

		// Token: 0x0600003D RID: 61 RVA: 0x00002B78 File Offset: 0x00000D78
		public AdminStatus sntl_admin_context_new_scope(string scope)
		{
			return NativeMethods.sntl_admin_context_new_scope(ref this.context, scope);
		}

		// Token: 0x0600003E RID: 62 RVA: 0x00002B98 File Offset: 0x00000D98
		public AdminStatus sntl_admin_context_new(string hostname, ushort port, string password)
		{
			return NativeMethods.sntl_admin_context_new(ref this.context, hostname, port, password);
		}

		// Token: 0x0600003F RID: 63 RVA: 0x00002BB8 File Offset: 0x00000DB8
		public AdminStatus sntl_admin_context_delete()
		{
			return NativeMethods.sntl_admin_context_delete(this.context);
		}

		// Token: 0x06000040 RID: 64 RVA: 0x00002BD8 File Offset: 0x00000DD8
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

		// Token: 0x06000041 RID: 65 RVA: 0x00002C24 File Offset: 0x00000E24
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

		// Token: 0x06000042 RID: 66 RVA: 0x00002C6D File Offset: 0x00000E6D
		public void setLibPath(string path)
		{
		}

		// Token: 0x04000047 RID: 71
		private IntPtr context;
	}
}
