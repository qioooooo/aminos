using System;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Security;
using System.Security.Permissions;

namespace Microsoft.Win32.SafeHandles
{
	// Token: 0x020002AD RID: 685
	[SuppressUnmanagedCodeSecurity]
	[HostProtection(SecurityAction.LinkDemand, MayLeakOnAbort = true)]
	internal sealed class SafeEventHandle : SafeHandleZeroOrMinusOneIsInvalid
	{
		// Token: 0x06001699 RID: 5785 RVA: 0x00048273 File Offset: 0x00047273
		internal SafeEventHandle()
			: base(true)
		{
		}

		// Token: 0x0600169A RID: 5786
		[DllImport("kernel32.dll", CharSet = CharSet.Unicode)]
		internal static extern SafeEventHandle CreateEvent(HandleRef lpEventAttributes, bool bManualReset, bool bInitialState, string name);

		// Token: 0x0600169B RID: 5787
		[ReliabilityContract(Consistency.WillNotCorruptState, Cer.Success)]
		[DllImport("kernel32.dll", ExactSpelling = true, SetLastError = true)]
		private static extern bool CloseHandle(IntPtr handle);

		// Token: 0x0600169C RID: 5788 RVA: 0x0004827C File Offset: 0x0004727C
		protected override bool ReleaseHandle()
		{
			return SafeEventHandle.CloseHandle(this.handle);
		}
	}
}
