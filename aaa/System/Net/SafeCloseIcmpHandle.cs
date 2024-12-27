using System;
using System.Net.NetworkInformation;
using System.Runtime.ConstrainedExecution;
using System.Security;
using Microsoft.Win32.SafeHandles;

namespace System.Net
{
	// Token: 0x02000510 RID: 1296
	[SuppressUnmanagedCodeSecurity]
	internal sealed class SafeCloseIcmpHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x0600281D RID: 10269 RVA: 0x000A5783 File Offset: 0x000A4783
		private SafeCloseIcmpHandle()
			: base(true)
		{
			this.IsPostWin2K = ComNetOS.IsPostWin2K;
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x000A5797 File Offset: 0x000A4797
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.MayFail)]
		protected override bool ReleaseHandle()
		{
			if (this.IsPostWin2K)
			{
				return UnsafeNetInfoNativeMethods.IcmpCloseHandle(this.handle);
			}
			return UnsafeIcmpNativeMethods.IcmpCloseHandle(this.handle);
		}

		// Token: 0x04002762 RID: 10082
		private bool IsPostWin2K;
	}
}
