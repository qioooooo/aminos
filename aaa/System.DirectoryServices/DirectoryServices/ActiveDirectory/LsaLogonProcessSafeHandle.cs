using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000ED RID: 237
	[SuppressUnmanagedCodeSecurity]
	internal sealed class LsaLogonProcessSafeHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000738 RID: 1848 RVA: 0x00025E25 File Offset: 0x00024E25
		private LsaLogonProcessSafeHandle()
			: base(true)
		{
		}

		// Token: 0x06000739 RID: 1849 RVA: 0x00025E2E File Offset: 0x00024E2E
		internal LsaLogonProcessSafeHandle(IntPtr value)
			: base(true)
		{
			base.SetHandle(value);
		}

		// Token: 0x0600073A RID: 1850 RVA: 0x00025E3E File Offset: 0x00024E3E
		protected override bool ReleaseHandle()
		{
			return NativeMethods.LsaDeregisterLogonProcess(this.handle) == 0;
		}
	}
}
