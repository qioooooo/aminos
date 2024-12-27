using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x020002B4 RID: 692
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal sealed class SafeProcessHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060016B5 RID: 5813 RVA: 0x00048327 File Offset: 0x00047327
		internal SafeProcessHandle()
			: base(true)
		{
		}

		// Token: 0x060016B6 RID: 5814 RVA: 0x00048330 File Offset: 0x00047330
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeProcessHandle(IntPtr handle)
			: base(true)
		{
			base.SetHandle(handle);
		}

		// Token: 0x060016B7 RID: 5815
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		internal static extern SafeProcessHandle OpenProcess(int access, bool inherit, int processId);

		// Token: 0x060016B8 RID: 5816 RVA: 0x00048340 File Offset: 0x00047340
		internal void InitialSetHandle(IntPtr h)
		{
			this.handle = h;
		}

		// Token: 0x060016B9 RID: 5817 RVA: 0x00048349 File Offset: 0x00047349
		protected override bool ReleaseHandle()
		{
			return SafeNativeMethods.CloseHandle(this.handle);
		}

		// Token: 0x04001600 RID: 5632
		internal static SafeProcessHandle InvalidHandle = new SafeProcessHandle(IntPtr.Zero);
	}
}
