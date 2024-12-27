using System;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000518 RID: 1304
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeGlobalFree : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06002834 RID: 10292 RVA: 0x000A5BE0 File Offset: 0x000A4BE0
		private SafeGlobalFree()
			: base(true)
		{
		}

		// Token: 0x06002835 RID: 10293 RVA: 0x000A5BE9 File Offset: 0x000A4BE9
		private SafeGlobalFree(bool ownsHandle)
			: base(ownsHandle)
		{
		}

		// Token: 0x06002836 RID: 10294 RVA: 0x000A5BF2 File Offset: 0x000A4BF2
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.SafeNetHandles.GlobalFree(this.handle) == IntPtr.Zero;
		}
	}
}
