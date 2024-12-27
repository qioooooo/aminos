using System;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000462 RID: 1122
	[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
	public sealed class SafeFileHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002D50 RID: 11600 RVA: 0x00098A9D File Offset: 0x00097A9D
		private SafeFileHandle()
			: base(true)
		{
		}

		// Token: 0x06002D51 RID: 11601 RVA: 0x00098AA6 File Offset: 0x00097AA6
		public SafeFileHandle(IntPtr preexistingHandle, bool ownsHandle)
			: base(ownsHandle)
		{
			base.SetHandle(preexistingHandle);
		}

		// Token: 0x06002D52 RID: 11602 RVA: 0x00098AB6 File Offset: 0x00097AB6
		protected override bool ReleaseHandle()
		{
			return Win32Native.CloseHandle(this.handle);
		}
	}
}
