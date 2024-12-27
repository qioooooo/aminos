using System;
using System.Security.Permissions;
using Microsoft.Win32.SafeHandles;

namespace System.EnterpriseServices.Internal
{
	// Token: 0x020000DD RID: 221
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal sealed class SafeUserTokenHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000518 RID: 1304 RVA: 0x00011B38 File Offset: 0x00010B38
		internal SafeUserTokenHandle()
			: base(true)
		{
		}

		// Token: 0x06000519 RID: 1305 RVA: 0x00011B41 File Offset: 0x00010B41
		internal SafeUserTokenHandle(IntPtr existingHandle, bool ownsHandle)
			: base(ownsHandle)
		{
			base.SetHandle(existingHandle);
		}

		// Token: 0x0600051A RID: 1306 RVA: 0x00011B51 File Offset: 0x00010B51
		protected override bool ReleaseHandle()
		{
			return NativeMethods.CloseHandle(this.handle);
		}
	}
}
