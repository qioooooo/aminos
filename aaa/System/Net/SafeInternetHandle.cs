using System;
using System.Runtime.ConstrainedExecution;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000511 RID: 1297
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeInternetHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600281F RID: 10271 RVA: 0x000A57B8 File Offset: 0x000A47B8
		public SafeInternetHandle()
			: base(true)
		{
		}

		// Token: 0x06002820 RID: 10272 RVA: 0x000A57C1 File Offset: 0x000A47C1
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		protected override bool ReleaseHandle()
		{
			return UnsafeNclNativeMethods.WinHttp.WinHttpCloseHandle(this.handle);
		}
	}
}
