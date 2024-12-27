using System;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Win32
{
	// Token: 0x020000C7 RID: 199
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeCryptContextHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000781 RID: 1921 RVA: 0x00020596 File Offset: 0x0001F596
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeCryptContextHandle()
			: base(true)
		{
		}

		// Token: 0x06000782 RID: 1922 RVA: 0x0002059F File Offset: 0x0001F59F
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeCryptContextHandle(IntPtr handle, bool ownsHandle)
			: base(ownsHandle)
		{
			base.SetHandle(handle);
		}

		// Token: 0x06000783 RID: 1923 RVA: 0x000205AF File Offset: 0x0001F5AF
		protected override bool ReleaseHandle()
		{
			if (this.handle != IntPtr.Zero)
			{
				UnsafeNativeMethods.CryptReleaseContext(this, 0U);
				return true;
			}
			return false;
		}
	}
}
