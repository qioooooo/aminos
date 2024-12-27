using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x020002B5 RID: 693
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal sealed class SafeTimerHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x060016BB RID: 5819 RVA: 0x00048367 File Offset: 0x00047367
		internal SafeTimerHandle()
			: base(true)
		{
		}

		// Token: 0x060016BC RID: 5820
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern bool CloseHandle(IntPtr handle);

		// Token: 0x060016BD RID: 5821 RVA: 0x00048370 File Offset: 0x00047370
		protected override bool ReleaseHandle()
		{
			return SafeTimerHandle.CloseHandle(this.handle);
		}
	}
}
