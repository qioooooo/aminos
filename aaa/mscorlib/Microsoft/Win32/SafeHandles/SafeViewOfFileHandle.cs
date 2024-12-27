using System;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000467 RID: 1127
	internal sealed class SafeViewOfFileHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002D5F RID: 11615 RVA: 0x00098B61 File Offset: 0x00097B61
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeViewOfFileHandle()
			: base(true)
		{
		}

		// Token: 0x06002D60 RID: 11616 RVA: 0x00098B6A File Offset: 0x00097B6A
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeViewOfFileHandle(IntPtr handle, bool ownsHandle)
			: base(ownsHandle)
		{
			base.SetHandle(handle);
		}

		// Token: 0x06002D61 RID: 11617 RVA: 0x00098B7A File Offset: 0x00097B7A
		protected override bool ReleaseHandle()
		{
			if (Win32Native.UnmapViewOfFile(this.handle))
			{
				this.handle = IntPtr.Zero;
				return true;
			}
			return false;
		}
	}
}
