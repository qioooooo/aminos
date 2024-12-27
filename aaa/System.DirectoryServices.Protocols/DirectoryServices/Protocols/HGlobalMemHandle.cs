using System;
using System.Runtime.InteropServices;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.DirectoryServices.Protocols
{
	// Token: 0x0200008D RID: 141
	[SuppressUnmanagedCodeSecurity]
	internal sealed class HGlobalMemHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06000317 RID: 791 RVA: 0x0000F45C File Offset: 0x0000E45C
		internal HGlobalMemHandle(IntPtr value)
			: base(true)
		{
			base.SetHandle(value);
		}

		// Token: 0x06000318 RID: 792 RVA: 0x0000F46C File Offset: 0x0000E46C
		protected override bool ReleaseHandle()
		{
			Marshal.FreeHGlobal(this.handle);
			return true;
		}
	}
}
