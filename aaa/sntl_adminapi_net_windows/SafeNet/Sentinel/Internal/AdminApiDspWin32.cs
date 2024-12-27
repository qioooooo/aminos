using System;
using System.Runtime.InteropServices;
using SafeNet.Sentinel.Internal.NativeMethods32;

namespace SafeNet.Sentinel.Internal
{
	// Token: 0x0200000B RID: 11
	internal class AdminApiDspWin32 : IAdminApiDsp
	{
		// Token: 0x0600002D RID: 45 RVA: 0x000029EC File Offset: 0x00000BEC
		public AdminApiDspWin32()
		{
			IntPtr zero = IntPtr.Zero;
		}

		// Token: 0x0600002E RID: 46 RVA: 0x00002A08 File Offset: 0x00000C08
		~AdminApiDspWin32()
		{
		}

		// Token: 0x0600002F RID: 47 RVA: 0x00002A34 File Offset: 0x00000C34
		public AdminStatus sntl_admin_context_new_scope(string scope)
		{
			return NativeMethods.sntl_admin_context_new_scope(ref this.context, scope);
		}

		// Token: 0x06000030 RID: 48 RVA: 0x00002A54 File Offset: 0x00000C54
		public AdminStatus sntl_admin_context_new(string hostname, ushort port, string password)
		{
			return NativeMethods.sntl_admin_context_new(ref this.context, hostname, port, password);
		}

		// Token: 0x06000031 RID: 49 RVA: 0x00002A74 File Offset: 0x00000C74
		public AdminStatus sntl_admin_context_delete()
		{
			return NativeMethods.sntl_admin_context_delete(this.context);
		}

		// Token: 0x06000032 RID: 50 RVA: 0x00002A94 File Offset: 0x00000C94
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

		// Token: 0x06000033 RID: 51 RVA: 0x00002AE0 File Offset: 0x00000CE0
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

		// Token: 0x06000034 RID: 52 RVA: 0x00002B29 File Offset: 0x00000D29
		public void setLibPath(string path)
		{
		}

		// Token: 0x04000046 RID: 70
		private IntPtr context;
	}
}
