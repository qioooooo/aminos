using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.DirectoryServices.ActiveDirectory
{
	// Token: 0x020000EC RID: 236
	[SuppressUnmanagedCodeSecurity]
	internal sealed class PolicySafeHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000736 RID: 1846 RVA: 0x00025E05 File Offset: 0x00024E05
		internal PolicySafeHandle(IntPtr value)
			: base(true)
		{
			base.SetHandle(value);
		}

		// Token: 0x06000737 RID: 1847 RVA: 0x00025E15 File Offset: 0x00024E15
		protected override bool ReleaseHandle()
		{
			return UnsafeNativeMethods.LsaClose(this.handle) == 0;
		}
	}
}
