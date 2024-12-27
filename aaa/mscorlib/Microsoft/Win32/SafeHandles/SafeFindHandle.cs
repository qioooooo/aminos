using System;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x02000464 RID: 1124
	internal sealed class SafeFindHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002D56 RID: 11606 RVA: 0x00098AE9 File Offset: 0x00097AE9
		[SecurityPermission(SecurityAction.LinkDemand, UnmanagedCode = true)]
		internal SafeFindHandle()
			: base(true)
		{
		}

		// Token: 0x06002D57 RID: 11607 RVA: 0x00098AF2 File Offset: 0x00097AF2
		protected override bool ReleaseHandle()
		{
			return Win32Native.FindClose(this.handle);
		}
	}
}
