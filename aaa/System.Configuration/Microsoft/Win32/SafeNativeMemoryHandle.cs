using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace Microsoft.Win32
{
	// Token: 0x020000C8 RID: 200
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeNativeMemoryHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000784 RID: 1924 RVA: 0x000205CE File Offset: 0x0001F5CE
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeNativeMemoryHandle()
			: this(false)
		{
		}

		// Token: 0x06000785 RID: 1925 RVA: 0x000205D7 File Offset: 0x0001F5D7
		internal SafeNativeMemoryHandle(bool useLocalFree)
			: base(true)
		{
			this._useLocalFree = useLocalFree;
		}

		// Token: 0x06000786 RID: 1926 RVA: 0x000205E7 File Offset: 0x0001F5E7
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeNativeMemoryHandle(IntPtr handle, bool ownsHandle)
			: base(ownsHandle)
		{
			base.SetHandle(handle);
		}

		// Token: 0x06000787 RID: 1927 RVA: 0x000205F7 File Offset: 0x0001F5F7
		internal void SetDataHandle(IntPtr handle)
		{
			base.SetHandle(handle);
		}

		// Token: 0x06000788 RID: 1928 RVA: 0x00020600 File Offset: 0x0001F600
		protected override bool ReleaseHandle()
		{
			if (this.handle != IntPtr.Zero)
			{
				if (this._useLocalFree)
				{
					UnsafeNativeMethods.LocalFree(this.handle);
				}
				else
				{
					Marshal.FreeHGlobal(this.handle);
				}
				this.handle = IntPtr.Zero;
				return true;
			}
			return false;
		}

		// Token: 0x04000430 RID: 1072
		private bool _useLocalFree;
	}
}
